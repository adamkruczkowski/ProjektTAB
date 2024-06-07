using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjektTabAPI.Entities.Domain;
using ProjektTabAPI.Entities.Dtos.BankingAccount;
using ProjektTabAPI.Repositories;

namespace ProjektTabAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BankingAccountController(IMapper mapper, IBankingAccountRepository bankingAccountRepository) : ControllerBase
    {
        [HttpGet]
        [Route("all/{user_id:Guid}")]
        public async Task<IActionResult> GetAllFromUserId([FromRoute] Guid user_id)
        {
            var bankingAccs = await bankingAccountRepository.GetAllFromUserId(user_id);
            if (bankingAccs is null) 
            {
                return NotFound();
            }
            var bankingAccsDto = mapper.Map<List<BankingAccountDto>>(bankingAccs);
            return Ok(bankingAccsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var bankingAcc = await bankingAccountRepository.GetById(id);
            if (bankingAcc is null)
            {
                return NotFound("Nie znaleziono szukanego konta bankowego");
            }
            var bankingAccDto = mapper.Map<BankingAccountDto>(bankingAcc);
            return Ok(bankingAccDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddBankingAccountDto addBankingAccountDto)
        {
            var bankingAcc = mapper.Map<BankingAccount>(addBankingAccountDto);
            var newBankingAcc = await bankingAccountRepository.Create(bankingAcc);
            if (newBankingAcc is null)
            {
                return NotFound("Coś poszło nie tak, nie dodano konta bankowego");
            }
            var newBankingAccDto = mapper.Map<BankingAccountDto>(newBankingAcc);
            return Ok(newBankingAccDto);
        }
    }
}
