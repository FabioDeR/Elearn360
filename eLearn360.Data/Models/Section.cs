using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class Section : BaseModel
    {
        [Required(ErrorMessage = "'Le champ 'Nom' est requis")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "Maximum 50 caractères autorisés pour 'Nom'")]
        public string Name { get; set; }


    
        public bool PrivateSection { get; set; }


        [Required(ErrorMessage = "'Le champ 'Contenu' est requis")]
        [DataType(DataType.Text)]
        public string Content { get; set; }



        [DataType(DataType.Text)]
        [MaxLength(150, ErrorMessage = "Maximum 150 caractères autorisés pour 'Description'")]
        public string Description { get; set; }

      

        #region Relation M to M
        public virtual List<Lesson> Lessons { get; set; }
        public virtual List<SectionHasLesson> SectionHasLessons { get; set; }
        public virtual List<CourseHasSection> CourseHasSections { get; set; }
        public virtual List<Course> Courses { get; set; }

        public virtual List<User> Users { get; set; }
        public virtual List<HistoricSectionHasUser> HistoricSectionHasUsers { get; set; }
        public virtual List<Question>Questions { get; set; }
        public virtual List<QuestionHasSection> QuestionHasSections { get; set; }
        public virtual List<Quizz> Quizzs { get; set; }
        public virtual List<QuizzHasSection> QuizzHasSections { get; set; }
        #endregion


        public Section()
        {
            PrivateSection = true;
        }
    }
}
