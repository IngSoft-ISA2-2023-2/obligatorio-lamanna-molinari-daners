using PharmaGo.Domain.Entities;

namespace PharmaGo.WebApi.Models.In
{
    public class ProductModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PharmacyName { get; set; }

        public Product ToEntity()
        {
            return new Product()
            {
                Code = this.Code,
                Name = this.Name,
                Description = this.Description,
                Price = this.Price,
                Pharmacy = new Pharmacy() { Name = this.PharmacyName }
            };
        }
    }
}
