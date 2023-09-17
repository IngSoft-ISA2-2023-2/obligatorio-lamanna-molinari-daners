using System;
using PharmaGo.Domain.Entities;

namespace PharmaGo.WebApi.Models.Out
{
	public class StockRequestModelResponse
	{
		public bool Created { get; set; }
		
		public StockRequestModelResponse(StockRequest stockRequest)
		{
			this.Created = true;
		}
    }
}

