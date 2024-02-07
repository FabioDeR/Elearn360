using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class PathWayHasGroup : BaseModel
    {
        [ForeignKey("GroupId")]
        public Guid GroupId { get; set; }
        public Group Group { get; set; }

        [ForeignKey("PathWayId")]
        public Guid PathWayId { get; set; }
        public PathWay PathWay { get; set; }
    }
}
