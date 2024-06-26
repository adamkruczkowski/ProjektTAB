namespace ProjektTabAPI.Entities.Domain
{
    public class BankingAccount
    {
        public Guid Id { get; set; }
        public string Number { get; set; }
        public decimal Amount { get; set; }
        public bool Blocked { get; set; }
        public Guid Id_client {  get; set; }
        public Client Client { get; set; }
        public List<Transaction> T_Received { get; set; }
        public List<Transaction> T_Sent { get; set; }
    }
}
