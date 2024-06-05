namespace ProjektTabAPI.Entities.Domain
{
    public class Client
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int Age {  get; set; }
        public string Login {  get; set; }
        public string Password { get; set; }
        public string Phone {  get; set; }

        public List<Login> Logins { get; set; }
        public List<BankingAccount> BankingAccounts { get; set; }
    }
}
