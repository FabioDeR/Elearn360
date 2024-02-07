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
    public class UserHasOccupationConfiguration : IEntityTypeConfiguration<UserHasOccupation>
    {
        public void Configure(EntityTypeBuilder<UserHasOccupation> builder)
        {
            builder.HasOne(m => m.User)
               .WithMany(x => x.UserHasOccupations)
               .HasForeignKey(t => t.UserId);

            builder.HasOne(m => m.Occupation)
                .WithMany(x => x.UserHasOccupations)
                .HasForeignKey(t => t.OccupationId);

            builder.HasOne(m => m.Organization)
                .WithMany(x => x.UserHasOccupations)
                .HasForeignKey(t => t.OrganizationId);

            builder.Property(i => i.Id).ValueGeneratedOnAdd();
        }
    }
}
