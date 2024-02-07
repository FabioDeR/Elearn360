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
    public class QuestionConfiguration : IEntityTypeConfiguration<Question>
    {
        public void Configure(EntityTypeBuilder<Question> builder)
        {
            //Lesson
            builder.HasMany(l => l.Lessons)
                   .WithMany(q => q.Questions)
                   .UsingEntity<QuestionHasLesson>(
                a => a.HasOne(l => l.Lesson)
                        .WithMany(qhl => qhl.QuestionHasLessons)
                        .HasForeignKey(x => x.LessonId),
                a => a.HasOne(q => q.Question)
                        .WithMany(qhl => qhl.QuestionHasLessons)
                        .HasForeignKey(x => x.QuestionId),
                a =>
                {
                    a.HasKey(t => new { t.Id });
                    a.Property(pt => pt.Id).ValueGeneratedOnAdd();
                    a.ToTable("QuestionHasLesson");
                });

            //Section
            builder.HasMany(s => s.Sections)
                  .WithMany(q => q.Questions)
                  .UsingEntity<QuestionHasSection>(
               a => a.HasOne(s => s.Section)
                       .WithMany(qhs => qhs.QuestionHasSections)
                       .HasForeignKey(x => x.SectionId),
               a => a.HasOne(q => q.Question)
                       .WithMany(qhs => qhs.QuestionHasSections)
                       .HasForeignKey(x => x.QuestionId),
               a =>
               {
                   a.HasKey(t => new { t.Id });
                   a.Property(pt => pt.Id).ValueGeneratedOnAdd();
                   a.ToTable("QuestionHasSection");
               });

            //Course
            builder.HasMany(c => c.Courses)
                  .WithMany(q => q.Questions)
                  .UsingEntity<QuestionHasCourse>(
               a => a.HasOne(c => c.Course)
                       .WithMany(qhc => qhc.QuestionHasCourses)
                       .HasForeignKey(x => x.CourseId),
               a => a.HasOne(q => q.Question)
                       .WithMany(qhc => qhc.QuestionHasCourses)
                       .HasForeignKey(x => x.QuestionId),
               a =>
               {
                   a.HasKey(t => new { t.Id });
                   a.Property(pt => pt.Id).ValueGeneratedOnAdd();
                   a.ToTable("QuestionHasCourse");
               });

            //PathWay
            builder.HasMany(pw => pw.PathWays)
                  .WithMany(q => q.Questions)
                  .UsingEntity<QuestionHasPathWay>(
               a => a.HasOne(pw => pw.PathWay)
                       .WithMany(qhpw => qhpw.QuestionHasPathWays)
                       .HasForeignKey(x => x.PathWayId),
               a => a.HasOne(q => q.Question)
                       .WithMany(qhpw => qhpw.QuestionHasPathWays)
                       .HasForeignKey(x => x.QuestionId),
               a =>
               {
                   a.HasKey(t => new { t.Id });
                   a.Property(pt => pt.Id).ValueGeneratedOnAdd();
                   a.ToTable("QuestionHasPathWay");
               });

            //Answer
            builder.HasMany(a => a.Answers)
                   .WithOne(q => q.Question);

        }
    }
}
