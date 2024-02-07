using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class UserHasFamilyLink : BaseModel
    {       

        public Guid ChildUserId { get; set; }
        public Guid FamilyLinkId { get; set; }
        public Guid ParentUserId { get; set; }



        [ForeignKey("ChildUserId")]
        public User ChildUser { get; set; }

        [ForeignKey("ParentUserId")]
        public User ParentUser { get; set; }

        [ForeignKey("FamilyLinkId")]
        public FamilyLink FamilyLink { get; set; }
    }
}
