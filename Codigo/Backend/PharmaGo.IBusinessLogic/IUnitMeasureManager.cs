using PharmaGo.Domain.Entities;

namespace PharmaGo.IBusinessLogic
{
    public interface IUnitMeasureManager
    {
        IEnumerable<UnitMeasure> GetAll();
    }
}
