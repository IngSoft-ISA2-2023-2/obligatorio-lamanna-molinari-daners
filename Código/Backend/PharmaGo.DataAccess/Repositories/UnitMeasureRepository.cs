using PharmaGo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaGo.DataAccess.Repositories
{
    public class UnitMeasureRepository : BaseRepository<UnitMeasure>
    {
        private readonly PharmacyGoDbContext _context;

        public UnitMeasureRepository(PharmacyGoDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
