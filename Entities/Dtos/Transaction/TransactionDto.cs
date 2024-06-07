using ProjektTabAPI.Entities.Dtos.BankingAccount;

namespace ProjektTabAPI.Entities.Dtos.Transaction
{
    public class TransactionDto
    {
        public Guid Id { get; set; }
        public decimal Balance_before { get; set; }
        public decimal Amount { get; set; }

        public BankingAccountDto Sender { get; set; }
        public BankingAccountDto Recipient { get; set; }
    }
}
