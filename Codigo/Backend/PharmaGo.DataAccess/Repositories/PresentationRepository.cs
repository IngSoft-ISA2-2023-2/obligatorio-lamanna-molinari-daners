using PharmaGo.Domain.Entities;

namespace PharmaGo.DataAccess.Repositories
{
    public class PresentationRepository : BaseRepository<Presentation>
    {
        private readonly PharmacyGoDbContext _context;

        public PresentationRepository(PharmacyGoDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
