using System;
using System.Linq.Expressions;
using LinqKit;
using PharmaGo.Domain.Entities;

namespace PharmaGo.Domain.SearchCriterias
{
	public class InvitationSearchCriteria
	{
		public string Pharmacy { get; set; }
		public string UserName { get; set; }
		public string Role { get; set; }

		public Expression<Func<Invitation, bool>> Criteria()
		{
			var result = PredicateBuilder.New<Invitation>(true);
			//Expression<Func<Invitation, bool>> result = exp => true;

			if (!string.IsNullOrEmpty(Pharmacy))
			{
				result.And(expression => expression.Pharmacy.Name.Contains(Pharmacy));
			}

			if (!string.IsNullOrEmpty(UserName))
			{
				result.And(expression => expression.UserName.Contains(UserName));
			}

			if (!string.IsNullOrEmpty(Role))
			{
				result.And(expression => expression.Role.Name.Contains(Role));
			}

			return result;
		}
	}
}

