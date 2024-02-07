using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class Question : BaseModel
    {
        [Required(ErrorMessage = "Le nom de la question est obligatoire")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Le contenu de la question est obligatoire")]
        public string Content { get; set; }

        public string ImageUrl { get; set; }

        public string Explanation { get; set; }

        [ForeignKey("LevelId")]
        public Guid LevelId { get; set; }
        public Level Level { get; set; }

        #region Many To Many
        public virtual List<Lesson> Lessons { get; set; }
        public virtual List<QuestionHasLesson> QuestionHasLessons { get; set; }
        public virtual List<Section> Sections { get; set; }
        public virtual List<QuestionHasSection> QuestionHasSections { get; set; }
        public virtual List<Course> Courses { get; set; }
        public virtual List<QuestionHasCourse> QuestionHasCourses { get; set; }
        public virtual List<PathWay> PathWays { get; set; }
        public virtual List<QuestionHasPathWay> QuestionHasPathWays { get; set; }
        public virtual List<Quizz> Quizzs { get; set; }
        public virtual List<QuizzHasQuestion> QuizzHasQuestions { get; set; }
        #endregion

        public virtual List<Answer> Answers { get; set; }
    }
}
