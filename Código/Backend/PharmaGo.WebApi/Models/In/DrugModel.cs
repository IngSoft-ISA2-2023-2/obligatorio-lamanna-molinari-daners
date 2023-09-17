using PharmaGo.Domain.Entities;

namespace PharmaGo.WebApi.Models.In
{
    public class DrugModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Symptom { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public bool Prescription { get; set; }
        public int UnitMeasureId { get; set; }
        public int PresentationId { get; set; }
        public string PharmacyName { get; set; }

        public Drug ToEntity()
        {
            return new Drug()
            {
                Code = this.Code,
                Name = this.Name,
                Symptom = this.Symptom,
                Quantity = this.Quantity,
                Price = this.Price,
                Stock = 0,
                Prescription = this.Prescription,
                Deleted = false,
                UnitMeasure = new UnitMeasure()
                {
                    Id = this.UnitMeasureId
                },
                Presentation = new Presentation()
                {
                    Id = this.PresentationId
                },
                Pharmacy = new Pharmacy() { Name = this.PharmacyName }
            };
        }
    }
}
