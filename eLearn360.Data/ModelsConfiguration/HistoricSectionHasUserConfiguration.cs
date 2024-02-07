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
    public class HistoricSectionHasUserConfiguration : IEntityTypeConfiguration<Section>
    {
        public void Configure(EntityTypeBuilder<Section> builder)
        {
            builder.HasMany(u => u.Users)
                   .WithMany(l => l.Sections)
                   .UsingEntity<HistoricSectionHasUser>(
                a => a.HasOne(u => u.User)
                        .WithMany(hs => hs.HistoricSectionHasUsers)
                        .HasForeignKey(x => x.UserId),
                a => a.HasOne(s => s.Section)
                        .WithMany(hs => hs.HistoricSectionHasUsers)
                        .HasForeignKey(x => x.SectionId),
                a =>
                {
                    a.HasKey(t => new { t.Id });
                    a.Property(pt => pt.Id).ValueGeneratedOnAdd();
                    a.ToTable("HistoricSectionHasUser");
                });
        }
    }
}
