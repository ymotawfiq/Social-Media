
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    internal class ReactConfiguration : IEntityTypeConfiguration<React>
    {
        public void Configure(EntityTypeBuilder<React> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.ReactValue).IsRequired().HasColumnName("React Value");
            builder.HasIndex(e => e.ReactValue).IsUnique();
        }
    }
}
