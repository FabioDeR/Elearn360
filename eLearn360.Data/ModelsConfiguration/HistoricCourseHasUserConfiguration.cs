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
    public class HistoricCourseHasUserConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasMany(u => u.Users)
                   .WithMany(c => c.Courses)
                   .UsingEntity<HistoricCourseHasUser>(
                a => a.HasOne(u => u.User)
                        .WithMany(hc => hc.HistoricCourseHasUsers)
                        .HasForeignKey(x => x.UserId),
                a => a.HasOne(c => c.Course)
                        .WithMany(hs => hs.HistoricCourseHasUsers)
                        .HasForeignKey(x => x.CourseId),
                a =>
                {
                    a.HasKey(t => new { t.Id });
                    a.Property(pt => pt.Id).ValueGeneratedOnAdd();
                    a.ToTable("HistoricCourseHasUser");
                });
        }
    }
}
