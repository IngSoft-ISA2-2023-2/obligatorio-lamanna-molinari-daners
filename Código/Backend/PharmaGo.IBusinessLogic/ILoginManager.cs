using PharmaGo.Domain.Entities;

namespace PharmaGo.IBusinessLogic
{
    public interface ILoginManager
    {
        public Authorization Login(string userName, string password);
        public bool IsTokenValid(string token);
        public bool IsRoleValid(string[] roles, string token);
    }
}