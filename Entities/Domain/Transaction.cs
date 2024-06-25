namespace ProjektTabAPI.Entities.Domain
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public decimal Balance_before { get; set; }
        public decimal Amount { get; set; }
        public string Title { get; set; }

        public Guid Sender_BAId { get; set; }
        public BankingAccount Sender { get; set; }
        public Guid Recipient_BAId { get; set; }
        public BankingAccount Recipient { get; set; }
    }
}
