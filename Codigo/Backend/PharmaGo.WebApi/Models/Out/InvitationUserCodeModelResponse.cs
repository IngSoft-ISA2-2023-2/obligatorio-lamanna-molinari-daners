using System;
namespace PharmaGo.WebApi.Models.Out
{
	public class InvitationUserCodeModelResponse
	{
		public string UserCode { get; set; }

		public InvitationUserCodeModelResponse(string userCode)
		{
			this.UserCode = userCode;
		}
	}
}

