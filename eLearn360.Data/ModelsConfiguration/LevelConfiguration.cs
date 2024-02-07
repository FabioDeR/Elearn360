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
    public class LevelConfiguration : IEntityTypeConfiguration<Level>
    {
        public void Configure(EntityTypeBuilder<Level> builder)
        {
            builder.HasData(
            new Level { Id = Guid.NewGuid(), Name = "Novice" },
            new Level { Id = Guid.NewGuid(), Name = "Confirmé" },
            new Level { Id = Guid.NewGuid(), Name = "Expert" }
            );
        }
    }
}
