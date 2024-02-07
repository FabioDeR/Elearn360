using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class Filial : BaseModel
    {
		[Required(ErrorMessage = "Le champ 'Nom' est requis")]
		[DataType(DataType.Text)]
		[MaxLength(50, ErrorMessage = "Le champ 'Nom' ne peut comporter que maximum 50 caractères")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Le champ 'Adresse' est requis")]
		[DataType(DataType.Text)]
		[MaxLength(75, ErrorMessage = "Le champ 'Adresse' ne peut comporter que maximum 75 caractères")]
		public string Address { get; set; }

		[Required(ErrorMessage = "Le champ 'Ville' est requis")]
		[DataType(DataType.Text)]
		[MaxLength(50, ErrorMessage = "Le champ 'Ville' ne peut comporter que maximum 50 caractères")]
		public string City { get; set; }

		[Required(ErrorMessage = "Le champ 'Pays' est requis")]
		[DataType(DataType.Text)]
		[MaxLength(50, ErrorMessage = "Le champ 'Pays' ne peut comporter que maximum 50 caractères")]
		public string Country { get; set; }


		[Required(ErrorMessage = "Le champ 'Code Postal' est requis")]
		[DataType(DataType.Text)]
		[MaxLength(12, ErrorMessage = "Le champ 'Code Postal' ne peut comporter que maximum 12 caractères")]
		[MinLength(4, ErrorMessage = "Le champ 'Code Postal' doit comporter minimum 4 caractères")]
		public string ZipCode { get; set; }


		[Required(ErrorMessage = "Le champ 'Téléphone' est requis")]
		[Phone]
		[MinLength(9, ErrorMessage = "Le champ 'Téléphone' doit comporter minimum 9 caractères")]
		public string Phone { get; set; }


		[Required(ErrorMessage = "Le champ 'Email' est requis")]
		[DataType(DataType.EmailAddress)]
		[MaxLength(100, ErrorMessage = "Le champ 'Email' ne peut comporter que maximum 100 caractères")]
		public string Email { get; set; }


		[DataType(DataType.Text)]
		[MaxLength(100, ErrorMessage = "Le champ 'Site Web' ne peut comporter que maximum 100 caractères")]
		public string Website { get; set; }

		public string ImageUrl { get; set; }

		[ForeignKey("OrganizationId")]	
		public Guid OrganizationId { get; set; }
		public Organization Organization { get; set; }

		// Many to many
		public virtual List<Group> Groups { get; set; }
	}
}
