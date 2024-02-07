using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class QuizzHasSection : BaseModel
    {
        [ForeignKey("QuizzId")]
        public Guid QuizzId { get; set; }
        public Quizz Quizz { get; set; }

        [ForeignKey("SectionId")]
        public Guid SectionId { get; set; }
        public Section Section { get; set; }
    }
}
