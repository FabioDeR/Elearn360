using eLearn360.Data.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.VM.ReportVM
{
    public class QuizzReportVM
    {
        public Guid QuizzId { get; set; }
        public string UserName { get; set; }
        public string levelName { get; set; }
        public DateTime? CreationDate { get; set; }
        public Guid UserId { get; set; }
        public int Rating { get; set; }
        public TypeEnum TypeEnum { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
    }
}
