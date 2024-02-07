using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class BaseModel
    {
        [Key]
        public  Guid Id { get; set; }

        public DateTime? CreationDate { get; set; }
        public Guid CreationTrackingUserId { get; set; }

        public DateTime? DeleteDate { get; set; }
        public Guid DeleteTrackingUserId { get; set; }

        public DateTime? UpdateDate { get; set; }
        public Guid UpdateTrackingUserId { get; set; }
    }
}
