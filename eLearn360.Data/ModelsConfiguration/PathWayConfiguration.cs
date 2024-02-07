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
    public class PathWayConfiguration : IEntityTypeConfiguration<PathWay>
    {
        public void Configure(EntityTypeBuilder<PathWay> builder)
        {

            builder
           .HasMany(p => p.Groups)
           .WithMany(p => p.PathWays)
           .UsingEntity<PathWayHasGroup>(
               j => j
                   .HasOne(pt => pt.Group)
                   .WithMany(t => t.PathWayHasGroups)
                   .HasForeignKey(pt => pt.GroupId),
               j => j
                   .HasOne(pt => pt.PathWay)
                   .WithMany(p => p.PathWayHasGroups)
                   .HasForeignKey(pt => pt.PathWayId),
               j =>
               {
                   j.HasKey(t => new { t.Id });
                   j.Property(pt => pt.Id).ValueGeneratedOnAdd();
                   j.ToTable("PathWayHasGroups");
               });
        }
    }
}
