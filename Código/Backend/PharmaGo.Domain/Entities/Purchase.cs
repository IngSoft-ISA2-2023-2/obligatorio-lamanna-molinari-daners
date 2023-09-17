
namespace PharmaGo.Domain.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string BuyerEmail { get; set; }
        public string TrackingCode { get; set; }
        public ICollection<PurchaseDetail> details { get; set; }

    }
}
