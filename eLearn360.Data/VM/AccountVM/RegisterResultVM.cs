using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.VM.AccountVM
{
    public class RegisterResultVM
    {        
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}
