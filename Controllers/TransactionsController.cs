using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjektTabAPI.Entities.Domain;
using ProjektTabAPI.Entities.Dtos.Transaction;
using ProjektTabAPI.Repositories;

namespace ProjektTabAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController(IMapper mapper, ITransactionRepository transactionRepository) : ControllerBase
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
        public async Task<IActionResult> GetById(Guid id)
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
        public async Task<IActionResult> Create(AddTransactionRequestDto transactionDto)
        {
            var transaction = mapper.Map<Transaction>(transactionDto);
            var addedTransaction = await transactionRepository.Create(transaction);
            if (addedTransaction is null)
            {
                return NotFound("Coś poszło nie tak, nie dodano transakcji");
            }
            var addedTransactionDto = mapper.Map<TransactionDto>(addedTransaction);
            return Ok(addedTransactionDto);
        }
    }
}
