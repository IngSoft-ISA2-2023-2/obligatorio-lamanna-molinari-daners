using ExportationModel.ExportDomain;

namespace ExportationModel.Interfaces
{
    public interface IFormat
    {
        string GetFormatName();
        IEnumerable<Parameter> GetParameters();
        void Export(IEnumerable<DrugExportationModel> drugDtos, IEnumerable<Parameter> parameters);
    }
}
