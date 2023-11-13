using PharmaGo.Domain.Entities;
using PharmaGo.Domain.Enums;

namespace PharmaGo.WebApi.Models.Out
{
    public class DrugDetailModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Symptom { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool Prescription { get; set; }
        public string UnitMeasure { get; set; }
        public string Presentation { get; set; }
        public PharmacyBasicModel Pharmacy { get; set; }

        public DrugDetailModel(Drug drug)
        {
            Id = drug.Id;
            Code = drug.Code;
            Name = drug.Name;
            Symptom = drug.Symptom;
            Quantity = drug.Quantity;
            Price = drug.Price;
            Stock = drug.Stock;
            Prescription = drug.Prescription;
            UnitMeasure = drug.UnitMeasure.Name;
            Presentation = drug.Presentation.Name;
            Pharmacy = new PharmacyBasicModel(drug.Pharmacy);
        }
    }
}
