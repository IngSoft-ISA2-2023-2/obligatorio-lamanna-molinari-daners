using System;
using PharmaGo.Domain.SearchCriterias;

namespace PharmaGo.WebApi.Models.In
{
	public class InvitationSearchCriteriaModelRequest
	{
		public string? Pharmacy { get; set; }
		public string? UserName { get; set; }
		public string? Role { get; set; }

        public InvitationSearchCriteria ToEntity()
        {
            return new InvitationSearchCriteria()
            {
                Pharmacy = this.Pharmacy,
                UserName = this.UserName,
                Role = this.Role
            };
        }
    }
}

