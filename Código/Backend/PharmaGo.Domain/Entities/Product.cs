using PharmaGo.Exceptions;
using System.Collections.Generic;

namespace PharmaGo.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public Pharmacy? Pharmacy { get; set; }

        public void ValidOrFail()
        {
            if (string.IsNullOrEmpty(Code) || string.IsNullOrEmpty(Name) || Price <= 0 || string.IsNullOrEmpty(Description))
            {
                throw new InvalidResourceException("The Drug was not correctly created.");
            }
            if (Name.Length > 30 || Description.Length > 70 || Code.Length != 5)
            {
                throw new InvalidResourceException("The Drug format is incorrect");
            }
        }
    }
}
