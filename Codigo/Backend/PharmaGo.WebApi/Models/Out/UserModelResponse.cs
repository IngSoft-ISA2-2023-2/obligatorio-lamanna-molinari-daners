using PharmaGo.Domain.Entities;
using System.Net;

namespace PharmaGo.WebApi.Models.Out
{
    public class UserModelResponse
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime? RegistrationDate { get; set; }

        public UserModelResponse(User user)
        {
            UserName = user.UserName;
            Email = user.Email;
            Address = user.Address;
            RegistrationDate = user.RegistrationDate;

        }

    }
}
