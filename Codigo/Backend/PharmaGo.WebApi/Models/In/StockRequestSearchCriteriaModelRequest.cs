using System;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;

namespace PharmaGo.WebApi.Models.In
{
	public class StockRequestSearchCriteriaModelRequest
	{
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Code { get; set; }
        public string? Status { get; set; }

        public StockRequestSearchCriteria ToEntity()
        {
            return new StockRequestSearchCriteria()
            {
                FromDate = this.FromDate,
                ToDate = this.ToDate,
                Code = this.Code,
                Status = this.Status
            };
        }
    }
}

