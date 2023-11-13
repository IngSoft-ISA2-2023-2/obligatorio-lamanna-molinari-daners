using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExportationModel.ExportDomain
{
    public class DrugExportationModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Symptom { get; set; }
        public bool Deleted { get; set; }
    }
}
