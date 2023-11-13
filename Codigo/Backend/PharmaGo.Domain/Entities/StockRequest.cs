using PharmaGo.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaGo.Domain.Entities
{
    public class StockRequest
    {
        public int Id { get; set; }
        public StockRequestStatus Status { get; set; }
        public DateTime RequestDate { get; set; }
        public User Employee { get; set; }
        public ICollection<StockRequestDetail> Details { get; set; }

        public StockRequest()
        {
            this.RequestDate = DateTime.Now;
            this.Details = new List<StockRequestDetail>();
        }
    }

}
