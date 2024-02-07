using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.VM.UserVM
{
    public class UserVM
    {  
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
     
        public string LastName { get; set; }      

        public string ImageUrl { get; set; }
      
        public string LoginMail { get; set; }      

    }
}
