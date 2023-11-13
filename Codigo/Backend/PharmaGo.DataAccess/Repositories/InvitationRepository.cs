using Microsoft.EntityFrameworkCore;
using PharmaGo.Domain.Entities;
using System.Linq.Expressions;

namespace PharmaGo.DataAccess.Repositories
{
    public class InvitationRepository : BaseRepository<Invitation>
    {

        PharmacyGoDbContext _context;
        private DbSet<Invitation> _invitations;
        public InvitationRepository(PharmacyGoDbContext context) : base(context) {
            _context = context;
            _invitations = _context.Set<Invitation>();
        }

        public bool Exists(Invitation element)
        {
            bool exists = false;
            exists = _context.Set<Invitation>().Any<Invitation>(e => e.UserName == element.UserName && e.UserCode == element.UserCode);
            return exists;
        }

        public int CountAsync()
        {
			return _invitations.Count();
		}

        public override Invitation GetOneDetailByExpression(Expression<Func<Invitation, bool>> expression)
        {
            return _context.Set<Invitation>()
                .Include(x => x.Pharmacy)
                .Include(x => x.Role)
                .FirstOrDefault(expression);
        }

        public override IEnumerable<Invitation> GetAllBasicByExpression(Expression<Func<Invitation, bool>> expression)
        {
            return _context.Set<Invitation>()
                    .Include(p => p.Pharmacy)
                    .Include(p => p.Role)
                    .Where(expression);
        }
    }
}
