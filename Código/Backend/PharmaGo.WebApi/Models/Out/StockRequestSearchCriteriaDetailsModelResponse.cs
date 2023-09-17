using System;
using PharmaGo.Domain.Entities;

namespace PharmaGo.WebApi.Models.Out
{
	public class StockRequestSearchCriteriaDetailsModelResponse
    {
		public DrugModelResponse Drug { get; set; }
		public int Quantity { get; set; }

		public StockRequestSearchCriteriaDetailsModelResponse(StockRequestDetail stockRequestDetail)
		{
			this.Drug = new DrugModelResponse(stockRequestDetail.Drug);
			this.Quantity = stockRequestDetail.Quantity;
		}
	}
}

