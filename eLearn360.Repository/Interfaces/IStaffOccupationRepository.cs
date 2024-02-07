using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository.Interfaces
{
    public interface IStaffOccupationRepository : IRepository<Guid, StaffOccupation>
    {
        Task<List<StaffOccupation>> GetStaffOccupationByOrganizationId(Guid organizationId);
    }
}
