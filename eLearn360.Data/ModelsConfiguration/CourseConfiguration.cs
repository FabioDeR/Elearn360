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
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder
           .HasMany(p => p.PathWays)
           .WithMany(p => p.Courses)
           .UsingEntity<PathWayHasCourse>(
               j => j
                   .HasOne(pt => pt.PathWay)
                   .WithMany(t => t.PathWayHasCourses)
                   .HasForeignKey(pt => pt.PathWayId),
               j => j
                   .HasOne(pt => pt.Course)
                   .WithMany(p => p.PathWayHasCourses)
                   .HasForeignKey(pt => pt.CourseId),
               j =>
               {
                   j.Property(pt => pt.Position);
                   j.HasKey(t => new { t.Id });
                   j.Property(pt => pt.Id).ValueGeneratedOnAdd();
                   j.ToTable("PathWayHasCourses");
               });

            builder
                .HasMany(p => p.Groups)
                .WithMany(p => p.Courses)
                .UsingEntity<CourseHasGroup>(
                j => j.HasOne(pt => pt.Group)
                      .WithMany(t => t.CourseHasGroups)
                      .HasForeignKey(pt => pt.GroupId),
                j => j.HasOne(pt => pt.Course)
                      .WithMany(t => t.CourseHasGroups)
                      .HasForeignKey(pt => pt.CourseId),
                j =>
                {
                    j.Property(pt => pt.Id).ValueGeneratedNever();
                    j.HasKey(p => p.Id);
                    j.ToTable("CourseHasGroups");
                });  
                    
                
                

        }
    }
}
