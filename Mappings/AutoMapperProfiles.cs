using AutoMapper;
using ProjektTabAPI.Entities.Domain;
using ProjektTabAPI.Entities.Dtos.BankingAccount;
using ProjektTabAPI.Entities.Dtos.Client;
using ProjektTabAPI.Entities.Dtos.Login;
using ProjektTabAPI.Entities.Dtos.Transaction;

namespace ProjektTabAPI.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Client, ClientSimpleDto>().ReverseMap();
            CreateMap<Client, LoginCredentialsDto>().ReverseMap();
            CreateMap<Client, ClientDto>().ReverseMap();

            CreateMap<Login, LoginSimpleDto>().ReverseMap();
            CreateMap<BankingAccount, BankingAccountSimpleDto>().ReverseMap();
            CreateMap<BankingAccount, BankingAccountDto>().ReverseMap();    

            CreateMap<Transaction, TransactionDto>().ReverseMap();
        }
    }
}
