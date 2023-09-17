using ExportationModel.ExportDomain;
using ExportationModel.Exceptions;
using ExportationModel.Interfaces;
using Newtonsoft.Json;

namespace JSONExporter
{
    public class JSONFormat : IFormat
    {
        public string GetFormatName()
        {
            return "JSON exporter";
        }

        public void Export(IEnumerable<DrugExportationModel> drugDtos, IEnumerable<Parameter> parameters)
        {
            string path = null;
            foreach (var parameter in parameters)
            {
                if (parameter.InputName == "path")
                    path = parameter.InputValue;
            }
            if (string.IsNullOrEmpty(path))
                throw new InvalidParameterException("The path to export json file is incorrect.");

            string json = JsonConvert.SerializeObject(drugDtos);
            string absolutePath = Path.GetFullPath(path);
            File.WriteAllText(absolutePath, json);
        }

        public IEnumerable<Parameter> GetParameters()
        {
            Parameter parameter = new Parameter()
            {
                InputName = "path",
                InputValue = ""
            };
            return new List<Parameter> { parameter };
        }
    }
}
