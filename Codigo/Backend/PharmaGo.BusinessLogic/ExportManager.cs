using ExportationModel.ExportDomain;
using ExportationModel.Interfaces;
using PharmaGo.Exceptions;
using PharmaGo.IBusinessLogic;
using System.Reflection;

namespace PharmaGo.BusinessLogic
{
    public class ExportManager : IExportManager
    {
        private readonly IDrugManager _drugManager;

        public ExportManager(IDrugManager drugManager)
        {
            _drugManager = drugManager;
        }

        public void ExportDrugs(string exporterName, IEnumerable<Parameter> parameters, string token)
        {
            List<IFormat> exporters = ReadDlls();
            IFormat? desiredImplementation = exporters.FirstOrDefault(e => e.GetFormatName() == exporterName);
            if (desiredImplementation == null)
                throw new ResourceNotFoundException("The selected exporter could not be found.");

            IEnumerable<DrugExportationModel> drugsToExport = _drugManager.GetDrugsToExport(token);
            desiredImplementation.Export(drugsToExport, parameters);
        }

        public IEnumerable<string> GetAllExporters()
        {
            return ReadDlls().Select(exporter => exporter.GetFormatName()).ToList();
        }

        public IEnumerable<Parameter> GetParameters(string exporterName)
        {
            List<IFormat> exporters = ReadDlls();
            IFormat? desiredImplementation = exporters.FirstOrDefault(e => e.GetFormatName() == exporterName);
            if (desiredImplementation == null)
                throw new ResourceNotFoundException("The selected exporter could not be found.");

            return desiredImplementation.GetParameters();
        }

        private List<IFormat> ReadDlls()
        {
            List<IFormat> availableExporters = new List<IFormat>();
            string exportersPath = "./Exporters";
            string[] filePaths = Directory.GetFiles(exportersPath);

            foreach (string filePath in filePaths)
            {
                if (filePath.EndsWith(".dll"))
                {
                    FileInfo fileInfo = new FileInfo(filePath);
                    Assembly assembly = Assembly.LoadFile(fileInfo.FullName);

                    foreach (Type type in assembly.GetTypes())
                    {
                        if (typeof(IFormat).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                        {
                            IFormat exporter = (IFormat)Activator.CreateInstance(type);
                            if (exporter != null)
                                availableExporters.Add(exporter);
                        }
                    }
                }
            }
            return availableExporters;
        }
    }
}