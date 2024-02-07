using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class HistoricSectionHasUser : BaseModel
    {
        [ForeignKey("SectionId")]
        public Guid SectionId { get; set; }
        public Section Section { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        public User User { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
