using ExportationModel.ExportDomain;

namespace PharmaGo.IBusinessLogic
{
    public interface IExportManager
    {
        IEnumerable<string> GetAllExporters();
        void ExportDrugs(string exporterName, IEnumerable<Parameter> parameters, string token);
        IEnumerable<Parameter> GetParameters(string exporterName);
    }
}
