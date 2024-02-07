using eLearn360.Data.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository.Interfaces
{
    public interface ILessonRepository : IRepository<Guid, Lesson>
    {

        Task<List<Lesson>> GetPrivateLessonByUserId(Guid userid);
        Task<List<Lesson>> GetPublicLessonByUserId(Guid userid);
        Task DuplicateLesson(Guid lesssonid,Guid userid);
        Task PostStartHistoric(Guid userId, Guid ItemId);
        Task UpdateEndHistoric(Guid userId, Guid lessonId);
        Task<LessonReportVM> GetLessonReport(Guid userid, Guid groupid);
        Task<List<Lesson>> GetLessonByPathid(Guid pathid, Guid userId);


    }
}
