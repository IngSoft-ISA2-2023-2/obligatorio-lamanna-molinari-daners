using PharmaGo.Domain.Entities;
using PharmaGo.IBusinessLogic;
using PharmaGo.IDataAccess;

namespace PharmaGo.BusinessLogic
{
    public class UnitMeasureManager : IUnitMeasureManager
    {
        private readonly IRepository<UnitMeasure> _unitMeasureRepository;

        public UnitMeasureManager(IRepository<UnitMeasure> repository)
        {
            _unitMeasureRepository = repository;
        }

        public IEnumerable<UnitMeasure> GetAll()
        {
            return _unitMeasureRepository.GetAllByExpression(s => s.Deleted == false);
        }
    }
}
