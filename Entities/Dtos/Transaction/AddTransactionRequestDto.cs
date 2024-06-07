namespace ProjektTabAPI.Entities.Dtos.Transaction
{
    public class AddTransactionRequestDto
    {
        public decimal Balance_before { get; set; }
        public decimal Amount { get; set; }

        public Guid Sender_BAId { get; set; }
        public Guid Recipient_BAId { get; set; }
    }
}
