using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class Lesson : BaseModel
    {
        [Required(ErrorMessage = "'Le champ 'Nom' est requis")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "Maximum 50 caractères autorisés pour 'Nom'")]
        public string Name { get; set; }     

        public bool PrivateLesson { get; set; }


        [Required(ErrorMessage = "'Le champ 'Contenu' est requis")]
        [DataType(DataType.Text)]
        public string Content { get; set; }


        [Required(ErrorMessage = "'Le champ 'Description' est requis")]
        [DataType(DataType.Text)]
        [MaxLength(150, ErrorMessage = "Maximum 150 caractères autorisés pour 'Description'")]
        public string Description { get; set; }


        #region Relation M to M
        public virtual List<Section> Sections { get; set; }
        public virtual List<SectionHasLesson> SectionHasLessons { get; set; }
        public virtual List<User> Users { get; set; }
        public virtual List<HistoricLessonHasUser> HistoricLessonHasUsers { get; set; }
        public virtual List<Question> Questions { get; set; }
        public virtual List<QuestionHasLesson> QuestionHasLessons { get; set; }
        public virtual List<Quizz> Quizzs { get; set; }
        public virtual List<QuizzHasLesson> QuizzHasLessons { get; set; }
        #endregion

        public Lesson()
        {
            PrivateLesson = true;
        }
    }
}
