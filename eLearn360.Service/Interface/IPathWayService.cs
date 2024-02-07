using eLearn360.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Service.Interface
{
    public interface IPathWayService
    {
        Task<Guid> Add(PathWay pathWay);
        Task<PathWay> GetById(Guid id);
        Task<HttpResponseMessage> Delete(Guid pathid);
        Task<List<Guid>> GetCourseGuid(Guid pathid);
        Task<List<Guid>> GetGroupGuid(Guid pathid);
        Task<List<PathWayHasCourse>> GetIncludePathWayHasCourse(Guid pathid);
        Task<List<PathWay>> GetPrivatePathWay(Guid userId);
        Task<List<PathWay>> GetPublicPathWay(Guid userId);
        Task RemovePathWayHasCourse(PathWayHasCourse pathWayHasCourse);
        Task RemovePathWayHasGroup(PathWayHasGroup pathWayHasGroup);
        Task Update(PathWay pathWay);
        Task UpdateOrDelete(List<PathWayHasCourse> pathWayHasCourses);
        Task<string> UploadPathWayImage(MultipartFormDataContent content);
        Task<List<PathWay>> GetPathByPathid(Guid pathId, Guid userId);

        //A REVOIR
        Task<List<PathWay>> GetPathByOrganizationId(Guid organizationid, Guid userid);

        //Teacher
        Task<List<PathWay>> GetPathByOrgaIdAndUserIdNotStudent(Guid organizationid, Guid userid);
    }
}
