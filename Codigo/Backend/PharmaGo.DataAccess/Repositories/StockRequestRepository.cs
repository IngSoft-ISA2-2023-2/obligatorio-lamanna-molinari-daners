using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PharmaGo.Domain.Entities;

namespace PharmaGo.DataAccess.Repositories
{
    public class StockRequestRepository : BaseRepository<StockRequest>
    {

        PharmacyGoDbContext _context;
        private DbSet<StockRequest> _stockRequests;
        public StockRequestRepository(PharmacyGoDbContext context) : base(context) {
            _context = context;
            _stockRequests = _context.Set<StockRequest>();
        }

        public int CountAsync()
        {
            return _stockRequests.Count();
        }

        public override StockRequest GetOneByExpression(Expression<Func<StockRequest, bool>> expression)
        {
            return _context.Set<StockRequest>()
                    .Include(p => p.Details).ThenInclude(p => p.Drug)
                    .FirstOrDefault(expression);
        }

        public override IEnumerable<StockRequest> GetAllBasicByExpression(Expression<Func<StockRequest, bool>> expression)
        {
            return _context.Set<StockRequest>()
                    .Include(p => p.Details).ThenInclude(p => p.Drug)
                    .Where(expression);
        }
    }
}
