using PharmaGo.Domain.Entities;

namespace PharmaGo.IBusinessLogic
{
    public interface IUsersManager
    {
        public User CreateUser(string UserName, string UserCode, string Email, string Password, string Address, DateTime RegistrationDate);
    }
}
