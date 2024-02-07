using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class Course : BaseModel
    {
        [Required(ErrorMessage = "'Le champ 'Nom' est requis")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "Maximum 50 caractères autorisés pour 'Nom'")]
        public string Name { get; set; }

        public bool PrivateCourse { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "'Le champ 'Contenu' est requis")]
        [DataType(DataType.Text)]
        public string Content { get; set; }


        [DataType(DataType.Text)]
        [MaxLength(150, ErrorMessage = "Maximum 150 caractères autorisés pour 'Description'")]
        public string Description { get; set; }



        #region One To Many
        [ForeignKey("CategoryId")]       
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        [ForeignKey("LevelId")]  
        public Guid LevelId { get; set; }
        public Level Level { get; set; }
      
        #endregion

        #region Relation M to M
        public virtual List<Section> Sections { get; set; }
        public virtual List<CourseHasSection> CourseHasSections { get; set; }

        public virtual List<PathWay> PathWays { get; set; }
        public virtual List<PathWayHasCourse> PathWayHasCourses { get; set; }

        public virtual List<User> Users { get; set; }
        public virtual List<HistoricCourseHasUser> HistoricCourseHasUsers { get; set; }

        public virtual List<Group> Groups { get; set; }
        public virtual List<CourseHasGroup> CourseHasGroups { get; set; }
        #endregion
        public virtual List<Question> Questions { get; set; }
        public virtual List<QuestionHasCourse> QuestionHasCourses { get; set; }

        public virtual List<Quizz> Quizzs { get; set; }
        public virtual List<QuizzHasCourse> QuizzHasCourses { get; set; } 
        

        


        public Course()
        {
            PrivateCourse = true;
        }
    }
}
