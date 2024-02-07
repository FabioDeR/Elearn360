using eLearn360.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Service.Interface
{
    public interface IStaffHasOccupationService
    {
        Task<HttpResponseMessage> Add(StaffOccupation staffoccupation);
        Task<HttpResponseMessage> Delete(Guid staffoccupationid);
        Task<List<StaffOccupation>> GetAll();
        Task<List<StaffOccupation>> GetByOrganizationId(Guid organizationid);
        Task<StaffOccupation> GetById(Guid staffoccupationid);
        Task<HttpResponseMessage> Update(StaffOccupation staffoccupation);
    }
}
