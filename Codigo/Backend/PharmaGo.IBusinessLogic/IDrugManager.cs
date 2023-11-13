using ExportationModel.ExportDomain;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;

namespace PharmaGo.IBusinessLogic
{
    public interface IDrugManager
    {
        IEnumerable<Drug> GetAll(DrugSearchCriteria drugSearchCriteria);
        Drug GetById(int id);
        Drug Create(Drug drug, string token);
        Drug Update(int id, Drug drug);
        void Delete(int id);
        IEnumerable<DrugExportationModel> GetDrugsToExport(string token);
        IEnumerable<Drug> GetAllByUser(string token);
    }
}
