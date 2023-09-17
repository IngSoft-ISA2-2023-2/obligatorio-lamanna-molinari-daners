using PharmaGo.Domain.Entities;
using System.Linq.Expressions;

namespace PharmaGo.Domain.SearchCriterias
{
    public class PharmacySearchCriteria
    {
        public string? Name { get; set; }
        public string? Address { get; set; }

        public Expression<Func<Pharmacy,bool>> Criteria(Pharmacy pharmacy)
        {
            if(!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Address))
            {
                return p => p.Name == pharmacy.Name && p.Address == pharmacy.Address;
            }
            else if(string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Address))
            {
                return p => p.Address == pharmacy.Address;
            }
            else if(!string.IsNullOrEmpty(Name) && string.IsNullOrEmpty(Address))
            {
                return p => p.Name == pharmacy.Name;
            }
            else
            {
                return p => p.Name == p.Name;
            }
        }
    }
}
