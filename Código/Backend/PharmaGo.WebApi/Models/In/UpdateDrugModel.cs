using PharmaGo.Domain.Entities;
using PharmaGo.Domain.Enums;

namespace PharmaGo.WebApi.Models.In
{
    public class UpdateDrugModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Symptom { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool Prescription { get; set; }

        public Drug ToEntity()
        {
            return new Drug()
            {
                Code = this.Code,
                Name = this.Name,
                Symptom = this.Symptom,
                Quantity = this.Quantity,
                Price = this.Price,
                Prescription = this.Prescription,
                Stock = 0,
                Deleted = false,
                UnitMeasure = new UnitMeasure(),
                Presentation = new Presentation(),
                Pharmacy = new Pharmacy(),
            };
        }
    }
}
