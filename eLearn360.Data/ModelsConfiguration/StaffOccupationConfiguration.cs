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
    public class StaffOccupationConfiguration : IEntityTypeConfiguration<StaffOccupation>
    {
        public void Configure(EntityTypeBuilder<StaffOccupation> builder)
        {
            builder.HasMany(p => p.UserHasGroups)
                    .WithMany(p => p.StaffOccupations)
                    .UsingEntity<StaffHasOccupationHasGroup>(
                    j => j.HasOne(pt => pt.UserHasGroup)
                           .WithMany(t => t.StaffHasOccupationHasGroups)
                           .HasForeignKey(pt => pt.UserHasGroupId),
                    j => j.HasOne(pt => pt.StaffOccupation)
                           .WithMany(t => t.StaffHasOccupationHasGroups)
                           .HasForeignKey(pt => pt.StaffOccupationId),
                    j =>
                    {
                        j.HasKey(t => t.Id);
                        j.Property(i => i.Id).ValueGeneratedOnAdd();
                        j.ToTable("StaffHasOccupationHasGroups");
                    }
                
                );
        }
    }
}
