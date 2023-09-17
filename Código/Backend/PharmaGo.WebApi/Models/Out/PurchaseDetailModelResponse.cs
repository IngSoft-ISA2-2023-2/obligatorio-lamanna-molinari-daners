using PharmaGo.Domain.Entities;

namespace PharmaGo.WebApi.Models.Out
{
    public class PurchaseDetailModelResponse
    {
        public int PurchaseId { get; set; }
        public int PurchaseDetailId { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int PharmacyId { get; set; }
        public string PharmacyName { get; set; }
        public string DrugCode { get; set; }
        public string DrugName { get; set; }
        public PurchaseDetailModelResponse(int id, PurchaseDetail detail) {
            PurchaseId = id;
            PurchaseDetailId = detail.Id;
            Status = detail.Status;
            Price = detail.Price;
            Quantity = detail.Quantity;
            PharmacyId = detail.Pharmacy.Id;
            PharmacyName = detail.Pharmacy.Name;
            DrugCode = detail.Drug.Code;
            DrugName = detail.Drug.Name;
        }
    }
}
