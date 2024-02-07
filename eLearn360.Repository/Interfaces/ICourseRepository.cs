using eLearn360.Data.VM.CourseVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository.Interfaces
{
    public interface ICourseRepository : IRepository<Guid, Course>
    {
        Task<List<Course>> GetPrivateCourseByUserId(Guid userid);
        Task<List<Course>> GetPublicCourseByUserId(Guid userid);
        Task DuplicateCourse(Guid courseid, Guid userid);
        Task RemoveCouseHasSection(CourseHasSection courseHasSection);
        Task UpdateOrDeleted(List<CourseHasSection> courseHasSections);
        Task<(List<Course>,int)> GetCoursesByGroupeId(Guid groupeId);
        Task<List<Guid>> GetSectionGuid(Guid sectionid);
        Task<List<CourseHasSection>> GetIncludeCourseHasSection(Guid courseid);
        Task<List<Guid>> GetAllGuidByCourseId(Guid courseId);
        Task<Course> GetCourseTolesson(Guid courseId);
        Task<ViewContentVM> GetViewContentById(Guid id);
        Task<List<Course>> GetCourseByPathAndUserId(Guid pathId, Guid userId);
        Task<List<Course>> GetCourseByPathAndUserIdWithHistoricCourse(Guid pathId, Guid userId);
        Task<int> GetCoursePercentage(Guid courseId, Guid userId);
        Task<List<Guid>> AllGuidSeen(Guid courseId, Guid userId);
    }
}
