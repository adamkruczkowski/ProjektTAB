namespace ProjektTabAPI.Entities.Dtos.BankingAccount
{
    public class AddBankingAccountDto
    {
        public string Number { get; set; }
        public decimal Amount { get; set; }
        public bool Blocked { get; set; }

        public Guid Id_client { get; set; }
    }
}
