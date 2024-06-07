using ProjektTabAPI.Entities.Domain;
using ProjektTabAPI.Entities.Dtos.BankingAccount;
using ProjektTabAPI.Entities.Dtos.Login;

namespace ProjektTabAPI.Entities.Dtos.Client
{
    public class ClientDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Login { get; set; }
        public string Phone { get; set; }

        public List<LoginSimpleDto> Logins { get; set; }
        public List<BankingAccountSimpleDto> BankingAccounts { get; set; }
    }
}
