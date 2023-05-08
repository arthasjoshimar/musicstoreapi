using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MusicStore.Entities;
using System.Reflection.Emit;

namespace MusicStore.DataAccess.Configurations
{
    public class ConcertConfigurations : IEntityTypeConfiguration<Concert>
    {
        public void Configure(EntityTypeBuilder<Concert> builder)
        {
            builder
               .Property(e => e.UnitPrice)
               .HasPrecision(11, 2);
        }
    }
}
