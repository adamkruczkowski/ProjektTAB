namespace ProjektTabAPI.Entities.Domain
{
    public class ClientSimpleDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
        public string Login { get; set; }
        public string Phone { get; set; }
    }
}
