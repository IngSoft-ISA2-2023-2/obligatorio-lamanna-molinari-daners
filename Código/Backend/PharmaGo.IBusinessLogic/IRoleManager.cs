using PharmaGo.Domain.Entities;

namespace PharmaGo.IBusinessLogic
{
    public interface IRoleManager
    {
        IEnumerable<Role> GetAll();
    }
}
