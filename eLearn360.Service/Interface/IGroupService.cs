using eLearn360.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Service.Interface
{
    public interface IGroupService
    {
        Task<HttpResponseMessage> Add(Group group);
        Task<HttpResponseMessage> Update(Group group);
        Task<List<Group>> GetAll();
        Task<Group> GetById(Guid groupid);
        Task<HttpResponseMessage> Delete(Guid groupid);
        Task<List<Group>> GetByOrganizationId(Guid organizationId);
        Task<string> UploadGroupImage(MultipartFormDataContent content);
        Task<Group> GetAllUserByGroupId(Guid groupid);
        Task<Group> GetTeacherByGroupId(Guid groupid);
        Task<Group> GetStudentByGroupId(Guid groupid);
        Task<List<User>> GetallUsersNotInGroup(Guid groupid, Guid organizationid);
        Task<HttpResponseMessage> RemoveUserHasGroup(UserHasGroup userHasGroup);
        Task<List<Group>> GetGroupByOrgaAndUserId(Guid organizationid, Guid userid);
        Task<List<Group>> GetMyGroups(Guid userid, Guid organizationid);
    }
}
