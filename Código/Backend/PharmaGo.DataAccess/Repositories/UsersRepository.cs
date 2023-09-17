using Microsoft.EntityFrameworkCore;
using PharmaGo.Domain.Entities;
using System.Linq.Expressions;

namespace PharmaGo.DataAccess.Repositories
{
    public class UsersRepository : BaseRepository<User>
    {

        PharmacyGoDbContext _context;
        public UsersRepository(PharmacyGoDbContext context) : base(context) {
            _context = context;
        }

        public override bool Exists(User element)
        {
            bool exists = false;
            exists = _context.Set<User>().Any<User>(e => e.Id == element.Id);
            return exists;
        }

        public override User GetOneDetailByExpression(Expression<Func<User, bool>> expression) {

            return _context.Set<User>()
                    .Include(x => x.Pharmacy)
                    .Include(x => x.Role)
                    .FirstOrDefault(expression);
        }

    }
}
