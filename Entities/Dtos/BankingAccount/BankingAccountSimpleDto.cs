namespace ProjektTabAPI.Entities.Dtos.BankingAccount
{
    public class BankingAccountSimpleDto
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public decimal Amount { get; set; }
        public bool Blocked { get; set; }
    }
}
