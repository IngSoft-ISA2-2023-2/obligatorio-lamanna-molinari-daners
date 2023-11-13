using System;
using PharmaGo.Domain.Entities;
using System.Linq.Expressions;
using System.Net;
using System.Xml.Linq;
using System.Reflection;

namespace PharmaGo.Domain.SearchCriterias
{
	public class StockRequestSearchCriteria
	{
        public int EmployeeId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }

        public Expression<Func<StockRequest, bool>> Criteria()
        {
            Expression<Func<StockRequest, bool>> result = p => p.Employee.Id == EmployeeId;

            if (FromDate.HasValue && ToDate.HasValue && !string.IsNullOrEmpty(Code) && !string.IsNullOrEmpty(Status))
            {
                result = p => p.Employee.Id == EmployeeId &&
                p.RequestDate >= FromDate && p.RequestDate <= ToDate &&
                p.Details.Any(d => d.Drug.Code == Code) &&
                p.Status == (Enums.StockRequestStatus)Enum.Parse(typeof(Enums.StockRequestStatus), Status);
            }
            else if (FromDate.HasValue && ToDate.HasValue && !string.IsNullOrEmpty(Code))
            {
                result = p => p.Employee.Id == EmployeeId &&
                p.RequestDate >= FromDate && p.RequestDate <= ToDate &&
                p.Details.Any(d => d.Drug.Code == Code);
            }
            else if (FromDate.HasValue && ToDate.HasValue && !string.IsNullOrEmpty(Status))
            {
                result = p => p.Employee.Id == EmployeeId &&
                p.RequestDate >= FromDate && p.RequestDate <= ToDate &&
                p.Status == (Enums.StockRequestStatus)Enum.Parse(typeof(Enums.StockRequestStatus), Status);
            }
            else if (!string.IsNullOrEmpty(Code) && !string.IsNullOrEmpty(Status))
            {
                result = p => p.Employee.Id == EmployeeId &&
                p.Details.Any(d => d.Drug.Code == Code) &&
                p.Status == (Enums.StockRequestStatus)Enum.Parse(typeof(Enums.StockRequestStatus), Status);
            }
            else if (FromDate.HasValue && ToDate.HasValue)
            {
                result = p => p.Employee.Id == EmployeeId && 
                p.RequestDate >= FromDate && p.RequestDate <= ToDate;
            }
            else if (!string.IsNullOrEmpty(Code))
            {
                result = p => p.Employee.Id == EmployeeId && 
                p.Details.Any(d => d.Drug.Code == Code);
            }
            else if (!string.IsNullOrEmpty(Status))
            {
                result = p => p.Employee.Id == EmployeeId && 
                p.Status == (Enums.StockRequestStatus)Enum.Parse(typeof(Enums.StockRequestStatus), Status);
            }

            return result;
        }
    }
}

