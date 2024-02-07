using elearn.Data.ViewModel.QuestionVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Service.Interface
{
    public interface IQuizzService
    {
		Task<QuizzVM> GenerateQuizz(QuizzVM quizzvm);
		Task<QuizzVM> UpdateRating(QuizzVM quizzvm);

	}
}
