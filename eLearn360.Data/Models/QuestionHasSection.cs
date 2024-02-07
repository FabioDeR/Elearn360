using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class QuestionHasSection : BaseModel
    {
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
        public Guid QuestionId { get; set; }

        [ForeignKey("SectionId")]
        public Section Section { get; set; }
        public Guid SectionId { get; set; }
    }
}
