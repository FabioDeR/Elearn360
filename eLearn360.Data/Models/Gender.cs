using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class Gender : BaseModel
    {
		[Required(ErrorMessage = "Le champ 'Nom' est obligatoire")]
		[DataType(DataType.Text)]
		[MaxLength(50, ErrorMessage = "Le champ 'Nom' ne peut comporter que maximum 50 caractères")]
		public string Name { get; set; }


		[MaxLength(5, ErrorMessage = "L'abréviation ne peut comporter que maximum 5 caractères")]
		public string Abbreviated { get; set; }

		
	}
}
