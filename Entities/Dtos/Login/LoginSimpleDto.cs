namespace ProjektTabAPI.Entities.Dtos.Login
{
    public class LoginSimpleDto
    {
        public Guid Id { get; set; }
        public bool Successfull { get; set; }
        public DateTime DateTime { get; set; }
    }
}
