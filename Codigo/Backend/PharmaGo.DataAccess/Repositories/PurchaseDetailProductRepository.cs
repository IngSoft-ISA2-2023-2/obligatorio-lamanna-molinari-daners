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
    public class PurchasesDetailProductRepository : BaseRepository<PurchaseDetailProduct>
    {
        private readonly PharmacyGoDbContext _context;

        public PurchasesDetailProductRepository(PharmacyGoDbContext context) : base(context)
        {
            _context = context;
        }

        public override bool Exists(PurchaseDetailProduct element)
        {
            bool exists = false;
            exists = _context.Set<PurchaseDetailProduct>().Any<PurchaseDetailProduct>(e => e.Id == element.Id);
            return exists;
        }

        public override PurchaseDetailProduct GetOneByExpression(Expression<Func<PurchaseDetailProduct, bool>> expression)
        {
            return _context.Set<PurchaseDetailProduct>()
                .Include(x => x.Pharmacy)
                .FirstOrDefault(expression);
        }
    }
}
