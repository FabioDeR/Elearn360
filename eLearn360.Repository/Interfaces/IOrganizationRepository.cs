using eLearn360.Data.VM.OrganizationVM.UserHasOccupationVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository.Interfaces
{
    public interface IOrganizationRepository : IRepository<Guid, Organization>
    {
        Task UpdateUserOccupation(UserHasOccupation userHasOccupation);
        Task<Organization> GetAllUserByOrganizationId(Guid organizationId);
        Task<(bool,string)> AddNewUserByOrganizationId(UserHasOccupationVM userHasOccupationvm);
        Task RemoveUser(Guid userid, Guid organizationid);
        Task RemoveUserHasOccupation(UserHasOccupation userHasOccupation);
    }
}
