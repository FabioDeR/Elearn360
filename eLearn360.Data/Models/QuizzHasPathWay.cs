using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class QuizzHasPathWay : BaseModel
    {
        [ForeignKey("QuizzId")]
        public Guid QuizzId { get; set; }
        public Quizz Quizz { get; set; }

        [ForeignKey("PathWayId")]
        public Guid PathWayId { get; set; }
        public PathWay PathWay { get; set; }
    }
}
