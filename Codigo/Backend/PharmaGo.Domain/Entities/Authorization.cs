namespace PharmaGo.Domain.Entities
{
    public class Authorization
    {
        public Guid Token { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }

    }
}
