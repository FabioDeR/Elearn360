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
    internal class OccupationConfiguration : IEntityTypeConfiguration<Occupation>
    {
        public void Configure(EntityTypeBuilder<Occupation> builder)
        {
            builder.HasMany(o => o.Organizations)
                   .WithMany(o => o.Occupations)
                   .UsingEntity<UserHasOccupation>(
                   J => J
                         .HasOne(u => u.Organization)
                         .WithMany(o => o.UserHasOccupations)
                         .HasForeignKey(u => u.OrganizationId),

                    J => J.HasOne(u => u.Occupation)
                         .WithMany(o => o.UserHasOccupations)
                         .HasForeignKey(u => u.OccupationId),
                    J =>
                    {
                        J.HasKey(k => new {  k.Id  });
                        J.Property(pt => pt.Id).ValueGeneratedOnAdd();
                        J.ToTable("UserHasOccupations");
                    }
               );
            builder.HasMany(o => o.Users)
                   .WithMany(o => o.Occupations)
                   .UsingEntity<UserHasOccupation>(
                   J => J
                         .HasOne(u => u.User)
                         .WithMany(o => o.UserHasOccupations)
                         .HasForeignKey(u => u. UserId),
                    J => J.HasOne(u => u.Occupation)
                         .WithMany(o => o.UserHasOccupations)
                         .HasForeignKey(u => u.OccupationId),
                    J =>
                    {
                        J.HasKey(k => new { k.Id });
                        J.Property(pt => pt.Id).ValueGeneratedOnAdd();
                        J.ToTable("UserHasOccupations");
                    }
               );
        }
    }
}
