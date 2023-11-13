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
    public class DrugRepository : BaseRepository<Drug>
    {
        private readonly PharmacyGoDbContext _context;

        public DrugRepository(PharmacyGoDbContext context) : base(context) 
        {
            _context = context;
        }

        public override Drug GetOneByExpression(Expression<Func<Drug, bool>> expression)
        {
            return _context.Set<Drug>().Include("Pharmacy").Include("UnitMeasure").Include("Presentation").FirstOrDefault(expression);
        }

        public override void InsertOne(Drug drug)
        {
            _context.Entry(drug).State = EntityState.Added;
            _context.Set<Drug>().Add(drug);
        }

        public override IEnumerable<Drug> GetAllByExpression(Expression<Func<Drug, bool>> expression) {
            return _context.Set<Drug>().Include(x => x.Pharmacy).Where(expression);
        }
    }
}
