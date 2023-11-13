using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using PharmaGo.IDataAccess;

namespace PharmaGo.BusinessLogic
{
    public class RoleManager : IRoleManager
    {
        private readonly IRepository<Role> _roleRepository;

        public RoleManager(IRepository<Role> roleRepo)
        {
            _roleRepository = roleRepo;
        }

        public IEnumerable<Role> GetAll()
        {
            return _roleRepository.GetAllByExpression(expression => true);
        }
    }
}
