

namespace PharmaGo.WebApi.Models.In
{
    public class PurchaseModelRequest
    {

        public string BuyerEmail { get; set; }
        public DateTime PurchaseDate { get; set; }
        public ICollection<PurchaseDetailModelRequest> Details { get; set; }

        public class PurchaseDetailModelRequest {
            public int PharmacyId { get; set; }
            public string Code { get; set; }
            public int Quantity { get; set; }
        }

    }
}
