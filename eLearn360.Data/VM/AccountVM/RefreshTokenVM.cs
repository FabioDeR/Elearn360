using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.VM.AccountVM
{
    public class RefreshTokenVM
    {
        public Guid UserId { get; set; }
        public Guid OrganizationId { get; set; }
    }
}
