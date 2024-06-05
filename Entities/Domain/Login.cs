namespace ProjektTabAPI.Entities.Domain
{
    public class Login
    {
        public Guid Id { get; set; }
        public bool Successful {  get; set; }
        public DateTime DateTime { get; set; }
        public Guid Id_Client {  get; set; }
        public Client Client { get; set; }
    }
}
