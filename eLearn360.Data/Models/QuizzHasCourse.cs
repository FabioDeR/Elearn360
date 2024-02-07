using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class QuizzHasCourse : BaseModel
    {
        [ForeignKey("QuizzId")]
        public Guid QuizzId { get; set; }
        public Quizz Quizz { get; set; }

        [ForeignKey("CourseId")]
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
    }
}
