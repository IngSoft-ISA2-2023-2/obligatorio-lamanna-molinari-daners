﻿
namespace PharmaGo.Domain.Entities
{
    public class User
    {

        public int Id { get; set; }
        public string UserName  { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Address { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public Role Role { get; set; }
        public Pharmacy? Pharmacy { get; set; }

    }
}
