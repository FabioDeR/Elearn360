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
    public class HistoricLessonHasUsersConfiguration : IEntityTypeConfiguration<Lesson>
    {
        public void Configure(EntityTypeBuilder<Lesson> builder)
        {
            builder.HasMany(u => u.Users)
                   .WithMany(l => l.Lessons)
                   .UsingEntity<HistoricLessonHasUser>(
                a => a.HasOne(x => x.User)
                        .WithMany(y => y.HistoricLessonHasUsers)
                        .HasForeignKey(x => x.UserId),
                a => a.HasOne(x => x.Lesson)
                        .WithMany(y => y.HistoricLessonHasUsers)
                        .HasForeignKey(y => y.LessonId),
                a =>
                {
                    a.HasKey(t => new { t.Id });
                    a.Property(pt => pt.Id).ValueGeneratedOnAdd();
                    a.ToTable("HistoricLessonHasUser");
                });
        }
    }
}
