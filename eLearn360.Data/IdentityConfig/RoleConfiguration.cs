using eLearn360.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLearn360.Data.IdentityConfig
{
    public class RoleConfiguration : IEntityTypeConfiguration<Occupation>
    {
        public void Configure(EntityTypeBuilder<Occupation> builder)
        {
            builder.HasData(
            new Occupation
            {
                Id = Guid.NewGuid(),
                OccupationName = "SuperAdmin",
                NormalizedName = "SuperAdmin"
            },
             new Occupation
             {
                 Id = Guid.NewGuid(),
                 OccupationName = "Admin",
                 NormalizedName = "Admin"
             },
              new Occupation
              {
                  Id = Guid.NewGuid(),
                  OccupationName = "Professor",
                  NormalizedName = "Professeur"
              },
               new Occupation
               {
                   Id = Guid.NewGuid(),
                   OccupationName = "Visitor",
                   NormalizedName = "Visiteur"
               },
                new Occupation
                {
                    Id = Guid.NewGuid(),
                    OccupationName = "Student",
                    NormalizedName = "Étudiant"
                },
                 new Occupation
                 {
                     Id = Guid.NewGuid(),
                     OccupationName = "Tutor",
                     NormalizedName = "Tuteur"
                 }
              );
        }
    }
}
