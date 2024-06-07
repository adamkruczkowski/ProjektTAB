using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProjektTabAPI.Entities.Dtos.Client;
using ProjektTabAPI.Repositories;

namespace ProjektTabAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController(IMapper mapper, IClientRepository clientRepository) : ControllerBase
    {
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var client = await clientRepository.GetClientById(id);
            if (client is null)
            {
                NotFound();
            }
            var clientDto = mapper.Map<ClientDto>(client);
            return Ok(clientDto);
        }
    }
}
