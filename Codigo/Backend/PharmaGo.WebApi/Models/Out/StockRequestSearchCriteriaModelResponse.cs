using System;
using PharmaGo.Domain.Entities;

namespace PharmaGo.WebApi.Models.Out
{
	public class StockRequestSearchCriteriaModelResponse
    {
		public int Id { get; set; }
		public string Status { get; set; }
		public DateTime RequestDate { get; set; }
		public ICollection<StockRequestSearchCriteriaDetailsModelResponse> Details { get; set; }

		public StockRequestSearchCriteriaModelResponse(StockRequest stockRequest)
		{
			this.Id = stockRequest.Id;
			this.RequestDate = stockRequest.RequestDate;
			this.Status = stockRequest.Status.ToString();
			this.Details = new List<StockRequestSearchCriteriaDetailsModelResponse>();
			foreach (var item in stockRequest.Details)
			{
				this.Details.Add(new StockRequestSearchCriteriaDetailsModelResponse(item));
			};
		}
    }
}

