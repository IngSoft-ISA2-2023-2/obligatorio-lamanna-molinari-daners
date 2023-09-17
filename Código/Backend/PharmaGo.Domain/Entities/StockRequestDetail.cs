namespace PharmaGo.Domain.Entities
{
    public class StockRequestDetail
    {
        public int Id { get; set; }
        public Drug Drug { get; set; }
        public int Quantity { get; set; }
    }
}