using System;
using PharmaGo.Domain.Entities;

namespace PharmaGo.WebApi.Models.Out
{
	public class DrugModelResponse
	{
        public string Code { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }

        public DrugModelResponse(Drug drug)
        {
            Code = drug.Code;
            Name = drug.Name;
            Stock = drug.Stock;
        }
    }
}

