using System;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;

namespace PharmaGo.IBusinessLogic
{
	public interface IStockRequestManager
	{
		StockRequest CreateStockRequest(StockRequest stockRequest, string token);
		bool RejectStockRequest(int id);
		bool ApproveStockRequest(int id);
		IEnumerable<StockRequest> GetStockRequestsByEmployee(string token, StockRequestSearchCriteria searchCriteria);
        IEnumerable<StockRequest> GetStockRequestsByOwner(string token);
    }
}

