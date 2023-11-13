using Microsoft.EntityFrameworkCore;
using PharmaGo.Domain.Entities;

namespace PharmaGo.DataAccess.Repositories
{
    public class RoleRepository : BaseRepository<Role>
    {

        PharmacyGoDbContext _context;
        private DbSet<Role> _roles;
        public RoleRepository(PharmacyGoDbContext context) : base(context) {
            _context = context;
        }
    }
}
