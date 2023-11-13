using System;
using PharmaGo.Domain.Entities;

namespace PharmaGo.WebApi.Models.Out
{
	public class RoleBasicModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

        public RoleBasicModel(Role role)
        {
            Id = role.Id;
            Name = role.Name;
        }
    }
}

