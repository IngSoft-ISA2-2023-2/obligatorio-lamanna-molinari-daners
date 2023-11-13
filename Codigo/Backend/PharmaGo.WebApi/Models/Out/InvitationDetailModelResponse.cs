using System;
using PharmaGo.Domain.Entities;

namespace PharmaGo.WebApi.Models.Out
{
	public class InvitationDetailModelResponse
	{
		public PharmacyBasicModel Pharmacy { get; set; }
		public string UserName { get; set; }
		public RoleBasicModel Role { get; set; }
		public string UserCode { get; set; }

		public InvitationDetailModelResponse(Invitation invitation)
		{
            this.Pharmacy = invitation.Pharmacy != null ? new PharmacyBasicModel(invitation.Pharmacy) : null;
            this.UserName = invitation.UserName;
            this.Role = new RoleBasicModel(invitation.Role);
            this.UserCode = invitation.UserCode;
        }
	}
}

