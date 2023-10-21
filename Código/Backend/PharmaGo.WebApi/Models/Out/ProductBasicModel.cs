using PharmaGo.Domain.Entities;

namespace PharmaGo.WebApi.Models.Out
{
    public class ProductBasicModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public PharmacyBasicModel Pharmacy { get; set; }

        public ProductBasicModel(Product p)
        {
            Id = p.Id;
            Code = p.Code;
            Name = p.Name;
            Price = p.Price;
            Pharmacy = new PharmacyBasicModel(p.Pharmacy);
        }
    }
}
