using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.Models
{
    public class Occupation : IdentityRole<Guid>
    {
        [Required(ErrorMessage = "'Le champ 'Nom' est requis")]
        [DataType(DataType.Text)]
        [MaxLength(50, ErrorMessage = "Maximum 50 caractères autorisés pour 'Nom'")]
        public virtual string OccupationName { get; set; }

        public DateTime? CreationDate { get; set; }
        public Guid CreationTrackingUserId { get; set; }

        public DateTime? DeleteDate { get; set; }
        public Guid DeleteTrackingUserId { get; set; }

        public DateTime? UpdateDate { get; set; }
        public Guid UpdateTrackingUserId { get; set; }

        // Identity
        public override string Name => OccupationName;        

        public virtual List<User> Users { get; set; }
        public virtual List<Organization> Organizations { get; set; }
        public virtual List<UserHasOccupation> UserHasOccupations { get; set; }



    }
}
