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
    public class ProductRepository : BaseRepository<Product>
    {
        private readonly PharmacyGoDbContext _context;

        public ProductRepository(PharmacyGoDbContext context) : base(context)
        {
            _context = context;
        }

        public override void InsertOne(Product product)
        {
            _context.Entry(product).State = EntityState.Added;
            _context.Set<Product>().Add(product);
        }

        public override IEnumerable<Product> GetAllByExpression(Expression<Func<Product, bool>> expression)
        {
            return _context.Set<Product>().Include(x => x.Pharmacy).Where(expression);
        }

        public override Product GetOneByExpression(Expression<Func<Product, bool>> expression)
        {
            return _context.Set<Product>().Include("Pharmacy").FirstOrDefault(expression);
        }
    }
}
