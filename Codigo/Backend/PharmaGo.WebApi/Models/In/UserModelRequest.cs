using PharmaGo.Domain.Entities;

namespace PharmaGo.WebApi.Models.In
{
    public class UserModelRequest
    {
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; }

    }
}
