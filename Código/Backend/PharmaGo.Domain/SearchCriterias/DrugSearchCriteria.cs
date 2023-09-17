using PharmaGo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace PharmaGo.Domain.SearchCriterias
{
    public class DrugSearchCriteria
    {
        public string? Name { get; set; }
        public int? PharmacyId { get; set; }

        public Expression<Func<Drug, bool>> Criteria(Drug drug)
        {
            if (!string.IsNullOrEmpty(Name) && PharmacyId != null)
            {
                return d => d.Name == drug.Name && d.Deleted == false && d.Pharmacy == drug.Pharmacy && d.Stock > 0;
            }
            else if (string.IsNullOrEmpty(Name) && PharmacyId != null)
            {
                return d => d.Pharmacy == drug.Pharmacy && d.Deleted == false && d.Stock > 0;
            }
            else if (!string.IsNullOrEmpty(Name) && PharmacyId == null)
            {
                return d => d.Name == drug.Name && d.Deleted == false && d.Stock > 0;
            }
            else
            {
                return d => d.Name == d.Name && d.Deleted == false && d.Stock > 0;
            }
        }
    }
}
