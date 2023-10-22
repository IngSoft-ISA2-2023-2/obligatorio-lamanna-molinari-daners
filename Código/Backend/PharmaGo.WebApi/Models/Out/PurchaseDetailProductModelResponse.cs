using PharmaGo.Domain.Entities;

namespace PharmaGo.WebApi.Models.Out
{
    public class PurchaseDetailProductModelResponse
    {
        public int PurchaseId { get; set; }
        public int PurchaseDetailId { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int PharmacyId { get; set; }
        public string PharmacyName { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public PurchaseDetailProductModelResponse(int id, PurchaseDetailProduct product)
        {
            PurchaseId = id;
            PurchaseDetailId = product.Id;
            Status = product.Status;
            Price = product.Price;
            Quantity = product.Quantity;
            PharmacyId = product.Pharmacy.Id;
            PharmacyName = product.Pharmacy.Name;
            ProductCode = product.Product.Code;
            ProductName = product.Product.Name;
        }
    }
}
