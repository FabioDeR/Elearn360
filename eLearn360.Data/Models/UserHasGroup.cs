using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class UserHasGroup : BaseModel
    {
        [ForeignKey("GroupId")]
        public Guid GroupId { get; set; }
        public Group Group { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public User User { get; set; }

        public bool IsHeadTeacher { get; set; } 

       
        public virtual List<StaffOccupation> StaffOccupations { get; set; }
        public virtual List<StaffHasOccupationHasGroup> StaffHasOccupationHasGroups { get; set; }


        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
