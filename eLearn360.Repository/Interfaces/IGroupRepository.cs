using eLearn360.Data.VM.GroupVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository.Interfaces
{
    public interface IGroupRepository : IRepository<Guid, Group>
    {
        Task RemoveUserHasGroup(UserHasGroup userHasGroup);
        Task Graduate(UserHasGroup userHasGroup);
        Task Promote(PromoteVM promoteVM);
        Task<List<Group>> GetGroupsByOrganizationId(Guid OrganizationId);
        Task<List<Group>> GetStudentGroupsByUserAndOrgaId(Guid organizationId, Guid userId);
        Task<Group> GetAllUserByGroupId(Guid groupId);
        Task<Group> GetTeacherList(Guid groupId);
        Task<Group> GetStudentList(Guid groupId);
        Task<List<User>> GetUserstNotInGroup(Guid groupid, Guid organizationid);
        Task<List<Group>> GetMyGroups(Guid userid, Guid organizationid);
    }
}
