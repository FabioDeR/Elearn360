using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class Category : BaseModel
    {
        [Required(ErrorMessage = " Le 'nom de la catégorie' est obligatoire")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "Le 'nom de la catégorie' ne peut comporter que maximum 50 caractères")]
        public string Name { get; set; }


        public List<Course> Courses { get; set; }
    }
}
