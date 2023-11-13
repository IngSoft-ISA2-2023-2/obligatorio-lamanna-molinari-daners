using System;
using PharmaGo.Domain.Entities;

namespace PharmaGo.WebApi.Models.Out
{
	public class InvitationModelResponse
	{
		public string UserName { get; set; }
		public string UserCode { get; set; }

		public InvitationModelResponse(Invitation invitation)
		{
			this.UserName = invitation.UserName;
			this.UserCode = invitation.UserCode;
		}
	}
}

