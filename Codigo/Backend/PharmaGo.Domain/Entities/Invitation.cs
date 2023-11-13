using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaGo.Domain.Entities
{
    public class Invitation
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime Created { get; set; }
        public Pharmacy? Pharmacy { get; set; }
        public Role Role { get; set; }

        public Invitation()
        {
            Created = DateTime.Now;
        }
    }
}
