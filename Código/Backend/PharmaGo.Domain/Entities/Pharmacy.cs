using PharmaGo.Exceptions;
using System.Collections.Generic;
using System.Diagnostics;

namespace PharmaGo.Domain.Entities
{
    public class Pharmacy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Drug> Drugs { get; set; }

        public void ValidOrFail()
        {
            if (string.IsNullOrEmpty(Name) || Name.Length >= 50 || string.IsNullOrEmpty(Address)
                    || Users == null || Drugs == null)
            {
                throw new InvalidResourceException("The Pharmacy is not correctly created.");
            }
        }
    }
}