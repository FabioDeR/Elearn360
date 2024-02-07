using eLearn360.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Service.Interface
{
    public interface ILessonService
    {
        Task<List<Lesson>> GetPrivateLesson(Guid userId);
        Task<List<Lesson>> GetPublicLesson(Guid userId);
        Task<HttpResponseMessage> DuplicateLesson(Guid lessonId, Guid userId);
        Task<Lesson> Add(Lesson lesson);
        Task<Lesson> GetById(Guid lessonId);
        Task Update(Lesson lesson);
        Task<List<Lesson>> GetLessonByPathId(Guid pathId, Guid userId);
        Task<HttpResponseMessage> PostHistoric(Guid userId, Guid itemId);
        Task<HttpResponseMessage> PostStartHistoric(Guid userId, Guid itemId);
        Task<HttpResponseMessage> PostEndHistoric(Guid userId, Guid itemId);
    }
}
