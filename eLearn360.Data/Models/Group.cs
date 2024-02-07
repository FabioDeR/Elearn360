using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class Group : BaseModel
    {
		[Required(ErrorMessage = "Le champ 'Nom' est requis")]
		[DataType(DataType.Text)]
		[MaxLength(50, ErrorMessage = "Le champ 'Nom' ne peut comporter que maximum 50 caractères")]
		public string Name { get; set; }

		public string ImageUrl { get; set; }

		[ForeignKey("OrganizationId")]		
		public Guid OrganizationId { get; set; }
		public Organization Organization { get; set; }

		[ForeignKey("FilialId")]
		public Guid? FilialId { get; set; }
		public Filial Filial { get; set; }

		// Many to many
		public virtual List<User> Users { get; set; }
		public virtual List<UserHasGroup> UserHasGroups { get; set; }
		public virtual List<PathWay> PathWays { get; set; }
		public virtual List<PathWayHasGroup> PathWayHasGroups { get; set; }

		public virtual List<Course> Courses { get; set; }
		public virtual List<CourseHasGroup> CourseHasGroups { get; set; }
	}
}
