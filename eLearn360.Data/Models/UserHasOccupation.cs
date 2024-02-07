using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class UserHasOccupation : BaseModel
	{        

		[ForeignKey("Organization")]
		public virtual Guid OrganizationId { get; set; }
		public virtual Organization Organization { get; set; }

		[ForeignKey("Occupation")]
		public virtual Guid OccupationId { get ; set ; }
		public virtual Occupation Occupation { get; set; }

        [ForeignKey("User")]
		public virtual Guid UserId { get; set; }
		public virtual User User { get; set; }

	}
}
