using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class Organization : BaseModel
    {
		[Required(ErrorMessage = "Le champ 'Nom' est requis")]
		[DataType(DataType.Text)]
		[MaxLength(50, ErrorMessage = "Le champ 'Nom' ne peut comporter que maximum 50 caractères")]
		public string Name { get; set; }

		[DataType(DataType.Text)]
		[MaxLength(75, ErrorMessage = "Le champ 'Adresse' ne peut comporter que maximum 75 caractères")]
		public string Address { get; set; }

		[DataType(DataType.Text)]
		[MaxLength(50, ErrorMessage = "Le champ 'Ville' ne peut comporter que maximum 50 caractères")]
		public string City { get; set; }

		[DataType(DataType.Text)]
		[MaxLength(50, ErrorMessage = "Le champ 'Pays' ne peut comporter que maximum 50 caractères")]
		public string Country { get; set; }


		[DataType(DataType.Text)]
		[MaxLength(12, ErrorMessage = "Le champ 'Code Postal' ne peut comporter que maximum 12 caractères")]
		public string ZipCode { get; set; }

		[Phone]
		public string Phone { get; set; }


		[DataType(DataType.EmailAddress)]
		[MaxLength(100, ErrorMessage = "Le champ 'Email' ne peut comporter que maximum 100 caractères")]
		public string Email { get; set; }


		[DataType(DataType.Text)]
		[MaxLength(100, ErrorMessage = "Le champ 'Site Web' ne peut comporter que maximum 100 caractères")]
		public string Website { get; set; }


		public string ImageUrl { get; set; }
		/*Many To Many*/
		public virtual List<Occupation> Occupations { get; set; }
		public virtual List<User> Users { get; set; }
		public virtual List<UserHasOccupation> UserHasOccupations { get; set; }
		// One to many
		public virtual List<Filial> Filials { get; set; }
		public virtual List<Group> Groups { get; set; }
	
	

	}
}
