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
    public class QuizzConfiguration : IEntityTypeConfiguration<Quizz>
    {
        public void Configure(EntityTypeBuilder<Quizz> builder)
        {
            //Question
            builder.HasMany(q => q.Questions)
                   .WithMany(q => q.Quizzs)
                   .UsingEntity<QuizzHasQuestion>(
                a => a.HasOne(q => q.Question)
                      .WithMany(qhq => qhq.QuizzHasQuestions)
                      .HasForeignKey(x => x.QuestionId),
                a => a.HasOne(q => q.Quizz)
                      .WithMany(qhq => qhq.QuizzHasQuestions)
                      .HasForeignKey(x => x.QuizzId),
                a =>
                {
                      a.HasKey(t => new { t.Id });
                      a.Property(pt => pt.Id).ValueGeneratedOnAdd();
                      a.ToTable("QuizzHasQuestion");
                });

            //Lesson
            builder.HasMany(l => l.Lessons)
                   .WithMany(q => q.Quizzs)
                   .UsingEntity<QuizzHasLesson>(
                a => a.HasOne(l => l.Lesson)
                      .WithMany(qhl => qhl.QuizzHasLessons)
                      .HasForeignKey(x => x.LessonId),
                a => a.HasOne(q => q.Quizz)
                      .WithMany(qhl => qhl.QuizzHasLessons)
                      .HasForeignKey(x => x.QuizzId),
                a =>
                {
                    a.HasKey(t => new { t.Id });
                    a.Property(pt => pt.Id).ValueGeneratedOnAdd();
                    a.ToTable("QuizzHasLesson");
                });

            //Section
            builder.HasMany(s => s.Sections)
                   .WithMany(q => q.Quizzs)
                   .UsingEntity<QuizzHasSection>(
                a => a.HasOne(s => s.Section)
                      .WithMany(qhs => qhs.QuizzHasSections)
                      .HasForeignKey(x => x.SectionId),
                a => a.HasOne(q => q.Quizz)
                      .WithMany(qhl => qhl.QuizzHasSections)
                      .HasForeignKey(x => x.QuizzId),
                a =>
                {
                    a.HasKey(t => new { t.Id });
                    a.Property(pt => pt.Id).ValueGeneratedOnAdd();
                    a.ToTable("QuizzHasSection");
                });

            //Course
            builder.HasMany(c => c.Courses)
                   .WithMany(q => q.Quizzs)
                   .UsingEntity<QuizzHasCourse>(
                a => a.HasOne(c => c.Course)
                      .WithMany(qhc => qhc.QuizzHasCourses)
                      .HasForeignKey(x => x.CourseId),
                a => a.HasOne(q => q.Quizz)
                      .WithMany(qhc => qhc.QuizzHasCourses)
                      .HasForeignKey(x => x.QuizzId),
                a =>
                {
                    a.HasKey(t => new { t.Id });
                    a.Property(pt => pt.Id).ValueGeneratedOnAdd();
                    a.ToTable("QuizzHasCourse");
                });

            //Path Way
            builder.HasMany(p => p.PathWays)
                   .WithMany(q => q.Quizzs)
                   .UsingEntity<QuizzHasPathWay>(
                a => a.HasOne(p => p.PathWay)
                      .WithMany(qhp => qhp.QuizzHasPathWays)
                      .HasForeignKey(x => x.PathWayId),
                a => a.HasOne(q => q.Quizz)
                      .WithMany(qhp => qhp.QuizzHasPathWays)
                      .HasForeignKey(x => x.QuizzId),
                a =>
                {
                    a.HasKey(t => new { t.Id });
                    a.Property(pt => pt.Id).ValueGeneratedOnAdd();
                    a.ToTable("QuizzHasPathWay");
                });
        }
    }
}
