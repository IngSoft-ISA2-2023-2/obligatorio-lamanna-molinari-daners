using PharmaGo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PharmaGo.Domain.SearchCriterias
{
    public class ProductSearchCriteria
    {

        public string? Name { get; set; }
        public int? PharmacyId { get; set; }

        public Expression<Func<Product, bool>> Criteria(Product drug)
        {
            if (!string.IsNullOrEmpty(Name) && PharmacyId != null)
            {
                return d => d.Name == drug.Name && d.Deleted == false && d.Pharmacy == drug.Pharmacy;
            }
            else if (string.IsNullOrEmpty(Name) && PharmacyId != null)
            {
                return d => d.Pharmacy == drug.Pharmacy && d.Deleted == false ;
            }
            else if (!string.IsNullOrEmpty(Name) && PharmacyId == null)
            {
                return d => d.Name == drug.Name && d.Deleted == false;
            }
            else
            {
                return d => d.Name == d.Name && d.Deleted == false;
            }
        }
    }
}
