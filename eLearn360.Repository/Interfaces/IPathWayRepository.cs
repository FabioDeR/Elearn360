using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository.Interfaces
{
    public interface IPathWayRepository : IRepository<Guid, PathWay>
    {
        Task RemovePathWayHasCourse(PathWayHasCourse pathWayHasCourse);
        Task RemovePathWayHasGroup(PathWayHasGroup pathWayHasGroup);
        Task<List<PathWay>> GetPrivatePathWayByUserId(Guid userid);
        Task<List<PathWay>> GetPublicPathWayByUserId(Guid userid);
        Task UpdateOrDeleted(List<PathWayHasCourse> pathWayHasCourses);
        Task<List<PathWay>> GetPathWaysByGroupId(Guid GroupId);
        Task<List<PathWay>> GetPathWaysByOrgaId(Guid OrgaId, Guid Userid);
        Task<List<PathWayHasCourse>> GetIncludePathWayHasCourse(Guid pathid);
        Task<List<Guid>> GetCourseGuid(Guid pathid);
        Task<List<Guid>> GetGroupGuid(Guid pathid);
        Task<List<Group>> GetGroupByPathWayId(Guid pathWayId, Guid orgnizationId);
        Task<List<PathWay>> GetPathByPathid(Guid pathId, Guid userId);
        Task<List<PathWay>> GetPathByUserAndOrgaIdNoStudent(Guid userId, Guid organizationId);
        Task<int> GetPathPercentage(Guid pathId, Guid userId);
     
    }
}
