using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class QuestionHasLesson : BaseModel
    {
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
        public Guid QuestionId { get; set; }

        [ForeignKey("LessonId")]
        public Lesson Lesson { get; set; }
        public Guid LessonId { get; set; }
    }
}
