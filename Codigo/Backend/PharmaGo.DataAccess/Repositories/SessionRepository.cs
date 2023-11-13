using PharmaGo.Domain.Entities;

namespace PharmaGo.DataAccess.Repositories
{
    public class SessionRepository : BaseRepository<Session>
    {
        PharmacyGoDbContext _context;
        public SessionRepository(PharmacyGoDbContext context) : base(context) {
            _context = context;
        }

        public override bool Exists(Session element)
        {
            bool exists = false;
            exists = _context.Set<Session>().Any<Session>(e => e.Id == element.Id);
            return exists;
        }

    }
}
