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
    public class HistoricPathWayHasUserConfiguration : IEntityTypeConfiguration<PathWay>
    {
        public void Configure(EntityTypeBuilder<PathWay> builder)
        {
            builder.HasMany(u => u.Users)
                   .WithMany(p => p.PathWays)
                   .UsingEntity<HistoricPathWayHasUser>(
                a => a.HasOne(u => u.User)
                      .WithMany(hp => hp.HistoricPathWayHasUsers)
                      .HasForeignKey(x => x.UserId),
                a => a.HasOne(p => p.PathWay)
                      .WithMany(hp => hp.HistoricPathWayHasUsers)
                      .HasForeignKey(x => x.PathWayId),
                a =>
                {
                    a.HasKey(t => new { t.Id });
                    a.Property(pt => pt.Id).ValueGeneratedOnAdd();
                    a.ToTable("HistoricPathHasUser");
                });
        }
    }
}
