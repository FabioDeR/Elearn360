using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.VM.AccountVM
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "'Le champ 'Prénom' est requis")]
        [DataType(DataType.Text)]
        [MaxLength(25, ErrorMessage = "Maximum 25 caractères autorisés pour 'Prénom'")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "'Le champ 'Nom' est requis")]
        [DataType(DataType.Text)]
        [MaxLength(40, ErrorMessage = "Maximum 40 caractères autorisés pour 'Nom'")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "'Le champ 'Date de Naissance' est requis")]
        [DataType(DataType.DateTime)]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "'Le champ 'Pays' est requis")]
        [DataType(DataType.Text)]
        [MaxLength(25, ErrorMessage = "Maximum 25 caractères autorisés pour 'Pays'")]
        public string Country { get; set; }

        [Required(ErrorMessage = "'Le champ 'Adresse' est requis")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "Maximum 50 caractères autorisés pour 'Adresse'")]
        public string Address { get; set; }


        [Required(ErrorMessage = "'Le champ 'Code Postal' est requis")]
        [MinLength(4, ErrorMessage = "Minimum 4 caractères autorisés pour le 'Code Postal'")]
        [MaxLength(12, ErrorMessage = "Maximum 12 caractères autorisés pour le 'Code Postal'")]
        public string ZipCode { get; set; }


        [Required(ErrorMessage = "Le champ 'Ville' est requis")]
        [DataType(DataType.Text)]
        [MaxLength(40, ErrorMessage = "Maximum 40 caractères autorisés pour la 'Ville'")]
        public string City { get; set; }


        [Required(ErrorMessage = "Le champ 'Téléphone' est requis")]
        [Phone]
        [MinLength(9, ErrorMessage = "Minimum 9 caractères autorisés pour le 'Téléphone'")]
        public string Phone { get; set; }

        [Required]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "'Le champ 'Mail' est requis")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(100, ErrorMessage = "Maximum 100 caractères autorisés pour 'Mail'")]
        public string LoginMail { get; set; }

        [MaxLength(150, ErrorMessage = "Maximum 100 caractères autorisés pour 'ImageUrl'")]
        public string ImageUrl { get; set; }

        public Guid GenderId { get; set; }

        public RegisterVM()
        {
            Birthday = DateTime.Now;
        }
    }
}
