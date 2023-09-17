using System;
using PharmaGo.Domain.Entities;

namespace PharmaGo.WebApi.Models.In
{
	public class InvitationModelRequest
	{
        public string? Pharmacy { get; set; }
        public string? UserName { get; set; }
		public string? UserCode { get; set; }
		public string? Role { get; set; }

		public Invitation ToEntity()
		{
			return new Invitation()
			{
				UserName = string.IsNullOrEmpty(this.UserName) ? null : this.UserName,
                UserCode = string.IsNullOrEmpty(this.UserCode) ? null : this.UserCode,
                Pharmacy = string.IsNullOrEmpty(Pharmacy) ? null : new Pharmacy() { Name = this.Pharmacy },
				Role = string.IsNullOrEmpty(this.Role) ? null : new Role() { Name = this.Role }
			};
		}
	}
}

