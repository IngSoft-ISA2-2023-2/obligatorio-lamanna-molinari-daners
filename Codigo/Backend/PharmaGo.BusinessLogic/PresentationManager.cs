using PharmaGo.Domain.Entities;
using PharmaGo.IBusinessLogic;
using PharmaGo.IDataAccess;

namespace PharmaGo.BusinessLogic
{
    public class PresentationManager : IPresentationManager
    {
        private readonly IRepository<Presentation> _presentationRepository;

        public PresentationManager(IRepository<Presentation> repository)
        {
            _presentationRepository = repository;
        }

        public IEnumerable<Presentation> GetAll()
        {
            return _presentationRepository.GetAllByExpression(s => s.Deleted == false);
        }
    }
}
