using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjektTabAPI.Data;
using ProjektTabAPI.Entities.Domain;
using ProjektTabAPI.Entities.Dtos.Transaction;
using ProjektTabAPI.Repositories;

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
    }
}
