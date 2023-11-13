using PharmaGo.Domain.Entities;

namespace PharmaGo.IBusinessLogic
{
    public interface IPresentationManager
    {
        IEnumerable<Presentation> GetAll();
    }
}
