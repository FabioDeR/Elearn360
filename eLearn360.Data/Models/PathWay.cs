using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class PathWay : BaseModel
    {
        [Required(ErrorMessage = "Le champ 'Nom' est requis")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "Maximum 50 caractères autorisés pour 'Nom'")]
        public string Name { get; set; }

        public bool PrivatePath { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "'Le champ 'Contenu' est requis")]
        [DataType(DataType.Text)]
        public string Content { get; set; }


        [DataType(DataType.Text)]
        [MaxLength(150, ErrorMessage = "Maximum 150 caractères autorisés pour 'Description'")]
        public string Description { get; set; }

        [ForeignKey("CategoryId")]
        [Required]     
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        [ForeignKey("LevelId")]
        [Required]      
        public Guid LevelId { get; set; }
        public Level Level { get; set; }

       
        #region Relation M to M
        public virtual List<PathWayHasCourse> PathWayHasCourses { get; set; }
        public virtual List<Course> Courses { get; set; }

        public virtual List<Group> Groups { get; set; }
        public virtual List<PathWayHasGroup> PathWayHasGroups { get; set; }

        public virtual List<User> Users { get; set; }
        public virtual List<HistoricPathWayHasUser> HistoricPathWayHasUsers { get; set; }
        public virtual List<Question> Questions { get; set; }
        public virtual List<QuestionHasPathWay> QuestionHasPathWays { get; set; }
        public virtual List<Quizz> Quizzs { get; set; }
        public virtual List<QuizzHasPathWay> QuizzHasPathWays { get; set; }

        #endregion



        public PathWay()
        {
            PrivatePath = true;
        }
    }
}
