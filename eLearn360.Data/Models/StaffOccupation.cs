using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class StaffOccupation : BaseModel
    {
        [Required(ErrorMessage = "'Le champ 'Nom' est requis")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "Maximum 50 caractères autorisés pour 'Nom'")]
        public string Name { get; set; }


        [ForeignKey("OrganzationId")]
        public Guid OrganizationId { get; set; }
        public Organization Organization { get; set; }


        public virtual List<UserHasGroup> UserHasGroups { get; set; }
        public virtual List<StaffHasOccupationHasGroup> StaffHasOccupationHasGroups { get; set; }

    }
}
