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
    public class SectionsConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
           builder
          .HasMany(p => p.Courses)
          .WithMany(p => p.Sections)
          .UsingEntity<CourseHasSection>(
              j => j
                  .HasOne(pt => pt.Course)
                  .WithMany(t => t.CourseHasSections)
                  .HasForeignKey(pt => pt.CourseId),
              j => j
                  .HasOne(pt => pt.Section)
                  .WithMany(p => p.CourseHasSections)
                  .HasForeignKey(pt => pt.SectionId),
              j =>
              {
                  j.Property(pt => pt.Position);
                  j.HasKey(t => new { t.Id });
                  j.Property(pt => pt.Id).ValueGeneratedOnAdd();
                  j.ToTable("CourseHasSections");
              });

            builder
           .HasMany(p => p.Lessons)
           .WithMany(p => p.Sections)
           .UsingEntity<SectionHasLesson>(
               j => j
                   .HasOne(pt => pt.Lesson)
                   .WithMany(t => t.SectionHasLessons)
                   .HasForeignKey(pt => pt.LessonId),
               j => j
                   .HasOne(pt => pt.Section)
                   .WithMany(p => p.SectionHasLessons)
                   .HasForeignKey(pt => pt.SectionId),
               j =>
               {
                   j.Property(pt => pt.Position);
                   j.HasKey(t => new { t.Id });
                   j.Property(pt => pt.Id).ValueGeneratedOnAdd();
                   j.ToTable("SectionHasLessons");
               });
        }
    }
}
