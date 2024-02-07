using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository.Interfaces
{
    public interface IQuestionRepository : IRepository<Guid, Question>
    {
        Task<List<Question>> GetByLessonId(Guid id);
        Task<List<Question>> GetBySectionId(Guid id);
        Task<List<Question>> GetByCourseId(Guid id);
        Task<List<Question>> GetByPathId(Guid id);
        Task<Question> GetByQuestionIdWithInclude(Guid id);
        Task RemoveQuestionHasLesson(QuestionHasLesson questionHasLesson);
        Task RemoveQuestionHasSection(QuestionHasSection questionHasSection);
        Task RemoveQuestionHasCourse(QuestionHasCourse questionHasCourse);
        Task RemoveQuestionHasPathWay(QuestionHasPathWay questionHasPathWay);
    }
}
