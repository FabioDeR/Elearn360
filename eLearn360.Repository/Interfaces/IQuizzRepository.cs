using elearn.Data.ViewModel.QuestionVM;
using eLearn360.Data.VM.ReportVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Repository.Interfaces
{
    public interface IQuizzRepository : IRepository<Guid, Quizz>
    {
        Task<Quizz> GetByIdWithInclude(Guid id);
        Task<List<QuizzReportVM>> GetReportByUserId(Guid userid);
        Task<List<QuizzReportVM>> GetReportByGroupId(Guid groupid);
        Task<QuizzVM> QuizzGeneration(QuizzVM quizzVM);
        Task<QuizzVM> UpdateQuizzRating(QuizzVM quizzvm);

        Task RemoveQuizzHasLesson(QuizzHasLesson quizzHasLesson);
        Task RemoveQuizzHasSection(QuizzHasSection quizzHasSection);
        Task RemoveQuizzHasCourse(QuizzHasCourse quizzHasCourse);
        Task RemoveQuizzHasPathWay(QuizzHasPathWay quizzHasPathWay);


    }
}
