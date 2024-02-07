using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository
{
    public class StaffOccupationRepository : Repository<Guid, StaffOccupation>, IStaffOccupationRepository
    {
        private readonly DbSet<StaffOccupation> _entities;
        private readonly Elearn360DBContext _context;

        public StaffOccupationRepository(Elearn360DBContext context) : base(context)
        {
            _context = context;
            _entities = _context.Set<StaffOccupation>();

        }

        public async override Task<IEnumerable<StaffOccupation>> Get()
        {
            IEnumerable<StaffOccupation> StaffOccupations = await _entities.Where(x => x.DeleteDate == null).ToListAsync();
            return StaffOccupations;
        }

        public async Task<List<StaffOccupation>> GetStaffOccupationByOrganizationId(Guid organizationId)
        {
            try
            {
                List<StaffOccupation> StaffOccupations = await _entities.Where(x => x.DeleteDate == null && x.OrganizationId == organizationId).Select(st => new StaffOccupation()
                {
                    Id = st.Id,
                    Name = st.Name

                }).ToListAsync();
                return StaffOccupations;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }
    }
}
