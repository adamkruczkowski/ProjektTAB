using AutoMapper;
using iTextSharp.text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjektTabAPI.Data;
using ProjektTabAPI.Entities.Domain;
using ProjektTabAPI.Entities.Dtos.Transaction;
using ProjektTabAPI.Repositories;
using System.Reflection.Metadata;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Tls;

namespace ProjektTabAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController(IMapper mapper, ITransactionRepository transactionRepository, IClientRepository clientRepository, IBankingAccountRepository bankingRepository) : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var transactions = await transactionRepository.GetAll();
            if (transactions is null)
            {
                return NotFound();
            }
            var transactionsDto = mapper.Map<List<TransactionDto>>(transactions);
            return Ok(transactionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var transaction = await transactionRepository.GetById(id);
            if (transaction is null)
            {
                return NotFound("Nie znaleziono szukanej transakcji");
            }
            var transactionDto = mapper.Map<TransactionDto>(transaction);
            return Ok(transactionDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddTransactionRequestDto transactionDto)
        {
            //var transaction = mapper.Map<Transaction>(transactionDto);
            //var addedTransaction = await transactionRepository.Create(transaction);
            // if (addedTransaction is null)
            // {
            //     return NotFound("Coś poszło nie tak, nie dodano transakcji");
            // }
            var senderClientAccount = await bankingRepository.GetById(transactionDto.Sender_BAId);
            var recipientClientAccount = await bankingRepository.GetById(transactionDto.Recipient_BAId);
            if (senderClientAccount is null || recipientClientAccount is null)
            {
                return NotFound("Coś poszło nie tak, nie znaleziono konta");
            }
            if (senderClientAccount.Blocked || recipientClientAccount.Blocked)
            {
                return Unauthorized("Twoje konto jest zablokowane");
            }
            if (senderClientAccount.Amount < transactionDto.Amount)
            {
                return Unauthorized("Saldo konta jest niższe niż podana kwota");
            }
            if (transactionDto.Amount <= 0)
            {
                return Unauthorized("Kwota przelewu nie może być niższa, bądź równa 0");
            }
            if (senderClientAccount.Id == recipientClientAccount.Id)
            {
                return Unauthorized("Nie możesz wysłać przelewu do samego siebie");
            }

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                Balance_before = transactionDto.Balance_before,
                Amount = transactionDto.Amount,
                Title = transactionDto.Title,
                Sender_BAId = transactionDto.Sender_BAId,
                Sender = senderClientAccount,
                CreatedAt = DateTime.Now,
                Recipient_BAId = transactionDto.Recipient_BAId,
                Recipient = recipientClientAccount
            };
            await transactionRepository.Create(transaction);
            await transactionRepository.DoTransfer(senderClientAccount, recipientClientAccount, transactionDto.Amount);

            var addedTransactionDto = mapper.Map<TransactionDto>(transaction);
            return Ok(addedTransactionDto);
        }

        [HttpGet]
        [Route("BankingAccount/all/{BA_id:Guid}")]
        public async Task<IActionResult> GetAllByBAId([FromRoute] Guid BA_id)
        {
            var transactions = await transactionRepository.GetAllByBAId(BA_id);
            if (transactions is null)
            {
                return NotFound();
            }
            transactions.Sort((x, y) => DateTime.Compare(y.CreatedAt, x.CreatedAt));
            var transactionsDto = mapper.Map<List<TransactionDto>>(transactions);
            return Ok(transactionsDto);
        }

        [HttpGet]
        [Route("BankingAccount/sent/{BA_id:Guid}")]
        public async Task<IActionResult> GetSendByBAId([FromRoute] Guid BA_id)
        {
            var transactions = await transactionRepository.GetSentByBAId(BA_id);
            if (transactions is null)
            {
                return NotFound();
            }
            var transactionsDto = mapper.Map<List<TransactionDto>>(transactions);
            return Ok(transactionsDto);
        }

        [HttpGet]
        [Route("BankingAccount/received/{BA_id:Guid}")]
        public async Task<IActionResult> GetReceivedByBAId([FromRoute] Guid BA_id)
        {
            var transactions = await transactionRepository.GetReceivedByBAId(BA_id);
            if (transactions is null)
            {
                return NotFound();
            }
            var transactionsDto = mapper.Map<List<TransactionDto>>(transactions);
            return Ok(transactionsDto);
        }

        [HttpGet]
        [Route("GeneratePDF/{BA_id:Guid}")]
        public async Task<IActionResult> GeneratePDF([FromRoute] Guid BA_id)
        {
            var transactions = await transactionRepository.GetAllByBAId(BA_id);
            if (transactions is null)
            {
                return NotFound();
            }
            transactions.Sort((x, y) => DateTime.Compare(y.CreatedAt, x.CreatedAt));
            var transactionsDto = mapper.Map<List<TransactionDto>>(transactions);

            var client = await clientRepository.GetClientById(BA_id);
            var bankingAcc = await bankingRepository.GetById(BA_id);

            string fontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arial.ttf");

            // Generowanie PDF
            using (var stream = new MemoryStream())
            {
                iTextSharp.text.Document pdfDoc = new iTextSharp.text.Document(PageSize.A4, 10f, 10f, 20f, 20f);
                PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                BaseFont baseFont = BaseFont.CreateFont(fontPath, BaseFont.CP1250, BaseFont.EMBEDDED);
                Font font = new Font(baseFont, 12, Font.NORMAL);

                pdfDoc.Add(new Paragraph("PolBank"));
                pdfDoc.Add(new Paragraph("Wyciąg z konta", font));
                pdfDoc.Add(new Paragraph($"Wygenerowano: {DateTime.Now}"));
                pdfDoc.Add(new Paragraph("\n"));
                if (transactions[0].Sender_BAId == BA_id)
                {
                    pdfDoc.Add(new Paragraph($"Klient: {transactions[0].Sender.Client.Name + " " + transactions[0].Sender.Client.Surname}", font));
                    pdfDoc.Add(new Paragraph($"Email klienta: {transactions[0].Sender.Client.Email}", font));
                }
                else
                {
                    pdfDoc.Add(new Paragraph($"Klient: {transactions[0].Recipient.Client.Name + transactions[0].Recipient.Client.Surname}", font));
                    pdfDoc.Add(new Paragraph($"Email klienta: {transactions[0].Recipient.Client.Email}", font));
                }
                pdfDoc.Add(new Paragraph($"Id konta bankowego: {bankingAcc.Id}"));
                pdfDoc.Add(new Paragraph($"Numer konta bankowego: {bankingAcc.Number}"));
                pdfDoc.Add(new Paragraph($"Stan konta bankowego: {bankingAcc.Amount} zł", font));
                pdfDoc.Add(new Paragraph("\n"));
                pdfDoc.Add(new Paragraph("\n"));

                PdfPTable table = new PdfPTable(7); // 7 kolumn
                table.AddCell(new PdfPCell(new Phrase("ID transakcji", font)));
                table.AddCell(new PdfPCell(new Phrase("Kwota", font)));
                table.AddCell(new PdfPCell(new Phrase("Tytuł", font)));
                table.AddCell(new PdfPCell(new Phrase("Data przelewu", font)));
                table.AddCell(new PdfPCell(new Phrase("Stan konta przed operacją", font)));
                table.AddCell(new PdfPCell(new Phrase("Stan konta po operacji", font)));
                table.AddCell(new PdfPCell(new Phrase("ID konta nadawcy", font)));
                table.AddCell(new PdfPCell(new Phrase("ID konta odbiorcy", font)));

                foreach (var transaction in transactionsDto)
                {
                    table.AddCell(new PdfPCell(new Phrase(transaction.Id.ToString(), font)));
                    table.AddCell(new PdfPCell(new Phrase(transaction.Amount.ToString("C"), font)));
                    table.AddCell(new PdfPCell(new Phrase(transaction.Title, font)));
                    table.AddCell(new PdfPCell(new Phrase(transaction.CreatedAt.ToString("g"), font)));
                    table.AddCell(new PdfPCell(new Phrase(transaction.Balance_before.ToString("C"), font)));
                    table.AddCell(new PdfPCell(new Phrase((transaction.Balance_before + transaction.Amount).ToString("C"), font)));
                    table.AddCell(new PdfPCell(new Phrase(transaction.Sender.Id.ToString(), font)));
                    table.AddCell(new PdfPCell(new Phrase(transaction.Recipient.Id.ToString(), font)));
                }

                pdfDoc.Add(table);
                pdfDoc.Close();

                return File(stream.ToArray(), "application/pdf", "Transactions_Report.pdf");
            }
        }
    }
}
