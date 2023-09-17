using Microsoft.EntityFrameworkCore;
using PharmaGo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PharmaGo.DataAccess.Repositories
{
    public class PurchasesRepository : BaseRepository<Purchase>
    {

        PharmacyGoDbContext _context;
        public PurchasesRepository(PharmacyGoDbContext context) : base(context)
        {
            _context = context;
        }

        public override bool Exists(Purchase element)
        {
            bool exists = false;
            exists = _context.Set<Purchase>().Any<Purchase>(e => e.Id == element.Id);
            return exists;
        }

        public override void DeleteOne(Purchase elem)
        {
            _context.Set<Purchase>().Remove(elem);
        }

        public override IEnumerable<Purchase> GetAllByExpression(Expression<Func<Purchase, bool>> expression)
        {
            return _context.Set<Purchase>()
                .Include(x => x.details).ThenInclude(d => d.Drug)
                .Include(x => x.details).ThenInclude(p => p.Pharmacy)
                .Where(expression).OrderBy(p => p.PurchaseDate);
        }

        public override IEnumerable<Purchase> GetAllBasicByExpression(Expression<Func<Purchase, bool>> expression)
        {
            return _context.Set<Purchase>()
                .Where(expression).OrderBy(p => p.PurchaseDate);
        }
        public override Purchase GetOneDetailByExpression(Expression<Func<Purchase, bool>> expression)
        {
            return _context.Set<Purchase>()
                .Include(x => x.details).ThenInclude(d => d.Drug)
                .Include(x => x.details).ThenInclude(d => d.Pharmacy)
                .FirstOrDefault(expression);
        }
    }
}
