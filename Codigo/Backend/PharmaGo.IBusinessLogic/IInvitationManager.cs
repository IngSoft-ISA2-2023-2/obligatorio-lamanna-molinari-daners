using System;
using PharmaGo.Domain.Entities;
using PharmaGo.Domain.SearchCriterias;

namespace PharmaGo.IBusinessLogic
{
	public interface IInvitationManager
	{
		Invitation CreateInvitation(string token, Invitation invitation);
		IEnumerable<Invitation> GetAllInvitations(InvitationSearchCriteria searchCriteria);
		Invitation UpdateInvitation(int id, Invitation invitation);
		string CreateUserCode();
		Invitation GetById(int id);
    }
}

