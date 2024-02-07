using eLearn360.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elearn.Data.ViewModel.QuestionVM
{
    public class QuizzVM
    {
        public TypeEnum TypeEnum { get; set; }
        public Guid QuizzId {get; set;}
        public Guid UserId  {get; set;}
        public Guid Idx { get; set; }
        public Guid LevelId { get; set; }

        [Range(2, 50, ErrorMessage = "Selectionnez un nombre entre 2 et 50")]
        public int nbQuestion { get; set; } = 5;
        public int Rating { get; set; }
        public List<QuestionVM> QuestionVMs { get; set; }
        public List<BadAnswerVM> BadAnswerVMs { get; set; }

    }
    public class QuestionVM
    {
        public Guid QuestionId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string Explanation { get; set; }
        public List<AnswerVM> AnswerVMs { get; set; }
    }

    public class AnswerVM
    {
        public Guid AnswerId { get; set; }
        public string Content { get; set; }
        public int Position { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsSelected { get; set; }
    }

    public class BadAnswerVM
    {
        public string Question { get; set; }
        public List<string> GoodAnswer { get; set; }
        public List<string> StudentAnswer { get; set; }
        public string Explaination { get; set; }
    }
}
