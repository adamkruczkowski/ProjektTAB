namespace ProjektTabAPI.Entities.Domain
{
    public class VerificationCode
    {
        public Guid Id { get; set; }
        public Guid ClientId { get; set; }
        public string Code { get; set; }
        public DateTime ExpirationTime { get; set; }
    }
}
