using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class User : IdentityUser<Guid>
    {
        [Required(ErrorMessage = "Le champ 'Prénom' est requis")]
        [DataType(DataType.Text)]
        [MaxLength(25, ErrorMessage = "Maximum 25 caractères autorisés pour 'Prénom'")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Le champ 'Nom' est requis")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "Maximum 50 caractères autorisés pour 'Nom'")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Le champ 'Date de Naissance' est requis")]
        [DataType(DataType.DateTime)]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Le champ 'Pays' est requis")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "Maximum 50 caractères autorisés pour 'Pays'")]
        public string Country { get; set; }

        [Required(ErrorMessage = "Le champ 'Adresse' est requis")]
        [DataType(DataType.Text)]
        [MaxLength(75, ErrorMessage = "Maximum 75 caractères autorisés pour 'Adresse'")]
        public string Address { get; set; }


        [Required(ErrorMessage = "Le champ 'Code Postal' est requis")]
        [MinLength(4, ErrorMessage = "Minimum 4 caractères autorisés pour le 'Code Postal'")]
        [MaxLength(12, ErrorMessage = "Maximum 12 caractères autorisés pour le 'Code Postal'")]
        public string ZipCode { get; set; }

        public string ImageUrl { get; set; }

        [Required(ErrorMessage = "Le champ 'Ville' est requis")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "Maximum 50 caractères autorisés pour la 'Ville'")]
        public string City { get; set; }


        [Required(ErrorMessage = "Le champ 'Téléphone' est requis")]
        [Phone]
        [MinLength(9, ErrorMessage = "Minimum 9 caractères autorisés pour le 'Téléphone'")]
        public string Phone { get; set; }


        [Required(ErrorMessage = "Le champ 'Mail' est requis")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(100, ErrorMessage = "Maximum 100 caractères autorisés pour 'Email'")]
        public string LoginMail { get; set; }

        public DateTime? CreationDate { get; set; }
        public Guid CreationTrackingUserId { get; set; }

        public DateTime? DeleteDate { get; set; }
        public Guid DeleteTrackingUserId { get; set; }

        public DateTime? UpdateDate { get; set; }
        public Guid UpdateTrackingUserId { get; set; }

        [ForeignKey("GenderId")]        
        public Guid GenderId { get; set; }
        public virtual Gender Gender { get; set; }

        // Many to many       

        public virtual List<Occupation> Occupations { get; set; }

        public virtual List<Group> Groups { get; set; }
        public virtual List<UserHasGroup> UserHasGroups { get; set; }

        public virtual List<Organization> Organizations { get; set; }
        public virtual List<UserHasOccupation> UserHasOccupations { get; set; }
        public virtual List<Lesson> Lessons { get; set; }
        public virtual List<HistoricLessonHasUser> HistoricLessonHasUsers { get; set; }
        public virtual List<Section> Sections { get; set; }
        public virtual List<HistoricSectionHasUser> HistoricSectionHasUsers { get; set; }
        public virtual List<Course> Courses { get; set; }
        public virtual List<HistoricCourseHasUser> HistoricCourseHasUsers { get; set; }
        public List<PathWay> PathWays { get; set; }
        public List<HistoricPathWayHasUser> HistoricPathWayHasUsers { get; set; }


        //Identity
        [JsonIgnore]
        public override string NormalizedEmail => LoginMail;
        public override string Email => LoginMail;
        public override string UserName => LoginMail;
        public override string PhoneNumber => Phone;
    }
}
