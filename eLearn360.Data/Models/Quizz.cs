using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class Quizz : BaseModel
    {
        public float Rating { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public User User { get; set; }

        #region n:m
        public virtual List<Lesson> Lessons { get; set; }
        public virtual List<QuizzHasLesson> QuizzHasLessons { get; set; }
        public virtual List<Section> Sections { get; set; }
        public virtual List<QuizzHasSection> QuizzHasSections { get; set; }
        public virtual List<Course> Courses { get; set; }
        public virtual List<QuizzHasCourse> QuizzHasCourses { get; set; }
        public virtual List<PathWay> PathWays { get; set; }
        public virtual List<QuizzHasPathWay> QuizzHasPathWays { get; set; }
        public virtual List<Question> Questions { get; set; }
        public virtual List<QuizzHasQuestion> QuizzHasQuestions { get; set; }
        #endregion
    }
}
