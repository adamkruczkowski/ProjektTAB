using AutoMapper;
using ProjektTabAPI.Entities.Domain;
using ProjektTabAPI.Entities.Dtos.Client;

namespace ProjektTabAPI.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Client, ClientSimpleDto>().ReverseMap();
            CreateMap<Client, LoginCredentialsDto>().ReverseMap();
        }
    }
}
