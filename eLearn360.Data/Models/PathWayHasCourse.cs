using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class PathWayHasCourse : BaseModel
    {
        public int Position { get; set; }

        [ForeignKey("CourseId")]
        public Guid CourseId { get; set; }
        public Course Course { get; set; }

        [ForeignKey("PathWayId")]
        public Guid PathWayId { get; set; }

        public PathWay PathWay { get; set; }
    }
}
