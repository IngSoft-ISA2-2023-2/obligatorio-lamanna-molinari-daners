using PharmaGo.Domain.Entities;

namespace PharmaGo.WebApi.Models.In
{
    public class UpdateProductModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        
        public Product ToEntity()
        {
            return new Product()
            {
                Code = this.Code,
                Name = this.Name,
                Description = this.Description,
                Price = this.Price,
                Deleted = false,
                Pharmacy = new Pharmacy(),
            };
        }
    }
}
