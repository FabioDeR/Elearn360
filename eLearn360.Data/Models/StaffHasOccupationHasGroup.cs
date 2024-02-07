using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class StaffHasOccupationHasGroup : BaseModel
    {
        [ForeignKey("UserHasGroupId")]
        public Guid UserHasGroupId { get; set; }
        public UserHasGroup UserHasGroup { get; set; }

        [ForeignKey("StaffOccupationId")]
        public Guid StaffOccupationId { get; set; }
        public StaffOccupation StaffOccupation { get; set; }
        
    }
}
