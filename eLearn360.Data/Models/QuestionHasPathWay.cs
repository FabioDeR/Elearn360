using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class QuestionHasPathWay : BaseModel
    {
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
        public Guid QuestionId { get; set; }

        [ForeignKey("PathWayId")]
        public PathWay PathWay { get; set; }
        public Guid PathWayId { get; set; }
    }
}
