using Microsoft.EntityFrameworkCore;
using PharmaGo.Domain.Entities;
using System.Linq.Expressions;

namespace PharmaGo.DataAccess.Repositories
{
    public class PharmacyRepository : BaseRepository<Pharmacy>
    {
        PharmacyGoDbContext _context;

        public PharmacyRepository(PharmacyGoDbContext context) : base(context)
        {
            _context = context;
        }

        public override Pharmacy GetOneByExpression(Expression<Func<Pharmacy, bool>> expression)
        {
            return _context.Set<Pharmacy>().Include("Users").Include("Drugs").FirstOrDefault(expression);
        }
    }
}
