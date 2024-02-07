using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.VM.OrganizationVM.UserHasOccupationVM
{
    public class UserHasOccupationVM
    {
        [Required]
        public string LoginMail { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }

        public Guid GenderId { get; set; }

        [Required]
        public Guid OrganizationId { get; set; }

        [Required]
        public List<Guid> OccupationId { get; set; }
    }
}
