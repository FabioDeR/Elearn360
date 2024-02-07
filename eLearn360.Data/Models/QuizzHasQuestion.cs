using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class QuizzHasQuestion : BaseModel
    {
        [ForeignKey("QuizzId")]
        public Guid QuizzId { get; set; }
        public Quizz Quizz { get; set; }

        [ForeignKey("QuestionId")]
        public Guid QuestionId { get; set; }
        public Question Question { get; set; }

        public bool IsCorrectAnswer { get; set; }
    }
}
