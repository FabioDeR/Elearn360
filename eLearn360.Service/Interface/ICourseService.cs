using eLearn360.Data.Models;
using eLearn360.Data.VM.CourseVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Service.Interface
{
    public interface ICourseService
    {
        Task<Guid> Add(Course course);
        Task<HttpResponseMessage> DuplicateCourse(Guid courseid, Guid userid);
        Task<Course> GetById(Guid id);
        Task<List<Course>> GetPrivateCourse(Guid userId);
        Task<List<Course>> GetPublicCourse(Guid userId);
        Task Update(Course course);
        Task<List<Guid>> GetSectionGuid(Guid courseid);
        Task<List<CourseHasSection>> GetIncludeCourseHasSection(Guid courseId);
        Task UpdateOrDelete(List<CourseHasSection> courseHasSections);
        Task RemoveCourseHasSection(CourseHasSection courseHasSection);
        Task<string> UploadCourseImage(MultipartFormDataContent content);
        Task<List<Course>> GetCourseByPathAndUserIdWithHistoricCourse(Guid pathid, Guid userid);
        Task<List<Course>> GetCourseByPathAndUserId(Guid pathid, Guid userid);

        //ViewCourse
        Task<List<Guid>> GetAllGuidSeen(Guid courseid, Guid userid);
        Task<ViewContentVM> GetViewContentById(Guid id);
        Task<List<Guid>> GetAllGuidByCourseId(Guid courseid);
        Task<Course> GetCourseTolesson(Guid courseid);
    }
}
