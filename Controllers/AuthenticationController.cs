using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjektTabAPI.Entities.Domain;
using ProjektTabAPI.Entities.Dtos.Client;
using ProjektTabAPI.Repositories;

namespace ProjektTabAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IMapper mapper, IClientRepository clientRepository) : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentialsDto loginCredencials)
        {
            var foundClient = await clientRepository.GetClientByLogin(loginCredencials.Login);

            if (foundClient is null)
            {
                return NotFound("Nie znaleziono użytkownika z podanym loginem");
            }

            if (foundClient.NumberOfTries == 5)
            {
                foundClient.Blocked = true;
                await clientRepository.SaveChangesAsync();
            }

            if (foundClient.Blocked)
            {
                return Unauthorized("Twoje konto zostało zablokowane");
            }

            if (foundClient.Password == loginCredencials.Password)
            {
                var foundClientDto = mapper.Map<ClientSimpleDto>(foundClient);
                foundClient.NumberOfTries = 0;
                await clientRepository.SaveChangesAsync();
                return Ok(foundClientDto);

                // 2FA
                //return Ok($"Wysłano na twoją skrzynkę pocztową {foundClient.Email} wiadomość. Przepisz otrzymane cyfry aby kontynuować.");
            }
            else
            {
                foundClient.NumberOfTries++;
                await clientRepository.SaveChangesAsync();
                return BadRequest($"Niepoprawne hasło pozostało {5 - foundClient.NumberOfTries} prób logowania.");
            }
        }
    }
}
