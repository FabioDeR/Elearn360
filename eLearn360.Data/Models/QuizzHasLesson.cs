using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class QuizzHasLesson : BaseModel
    {
        [ForeignKey("QuizzId")]
        public Guid QuizzId { get; set; }
        public Quizz Quizz { get; set; }

        [ForeignKey("LessonId")]
        public Guid LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}
