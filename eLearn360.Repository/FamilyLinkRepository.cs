using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository
{
    public class FamilyLinkRepository : Repository<Guid, FamilyLink>, IFamilyLinkRepository
    {

        private readonly DbSet<FamilyLink> _entities;
        private readonly Elearn360DBContext _context;

        public FamilyLinkRepository(Elearn360DBContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<FamilyLink>();
        }
    }
}
