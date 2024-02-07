using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class Level : BaseModel
    {
        [Required(ErrorMessage = "Le 'nom du niveau' est obligatoire")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "Le 'nom du niveau' ne peut comporter que maximum 50 caractères")]
        public string Name { get; set; }


        public virtual List<Course> Courses { get; set; }
    }
}
