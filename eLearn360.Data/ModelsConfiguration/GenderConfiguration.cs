using eLearn360.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.ModelsConfiguration
{
    public class GenderConfiguration : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.HasData(
              new Gender { Id = Guid.NewGuid(), Name = "Femme", Abbreviated = "Mme." },
              new Gender { Id = Guid.NewGuid(), Name = "Homme", Abbreviated = "M." },
              new Gender { Id = Guid.NewGuid(), Name = "Autre", Abbreviated = "X." }
          );
        }
    }
}
