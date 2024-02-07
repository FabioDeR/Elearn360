using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class CourseHasGroup : BaseModel
    {      

        [ForeignKey("CourseId")]
        public virtual Guid CourseId { get; set; }
        public virtual Course Course { get; set; }

        [ForeignKey("GroupId")]
        public virtual Guid GroupId { get; set; }

        public virtual Group Group { get; set; }
    }
}
