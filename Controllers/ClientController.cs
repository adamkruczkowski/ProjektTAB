using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjektTabAPI.Entities.Domain;
using ProjektTabAPI.Entities.Dtos.Client;
using ProjektTabAPI.Repositories;

namespace ProjektTabAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController(IMapper mapper, IClientRepository clientRepository) : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginCredentialsDto loginCredencials)
        {
            var foundClient = await clientRepository.GetClientByLogin(loginCredencials.Login);
            if (foundClient is null) 
            {
                return NotFound();
            }
            if (foundClient.Password == loginCredencials.Password)
            {
                var foundClientDto = mapper.Map<ClientSimpleDto>(foundClient);
                return Ok(foundClientDto);
            } 
            else
            {
                return BadRequest("Niepoprawne hasło");
            }
        }
    }
}
