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
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasMany(p => p.Users)
                .WithMany(p => p.Groups)
                .UsingEntity<UserHasGroup>(
                j => j.HasOne(pt => pt.User)
                      .WithMany(t => t.UserHasGroups)
                      .HasForeignKey(t => t.UserId),
                j => j.HasOne(pt => pt.Group)
                       .WithMany(t => t.UserHasGroups)
                       .HasForeignKey(pt => pt.GroupId),
                j =>
                {
                    j.Property(pt => pt.IsHeadTeacher).HasDefaultValue(false);
                    j.Property(pt => pt.Id).ValueGeneratedOnAdd();
                    j.HasKey(p => p.Id);
                    j.ToTable("UserHasGroups");
                }
                );
        }
    }
}
