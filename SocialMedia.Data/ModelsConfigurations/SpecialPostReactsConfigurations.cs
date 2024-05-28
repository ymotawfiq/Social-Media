

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class SpecialPostReactsConfigurations : IEntityTypeConfiguration<SpecialPostReacts>
    {
        public void Configure(EntityTypeBuilder<SpecialPostReacts> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.React).WithMany(e => e.SpecialPostReacts).HasForeignKey(e => e.ReactId);
            builder.Property(e => e.ReactId).IsRequired().HasColumnName("React Id");
            builder.HasIndex(e => e.ReactId).IsUnique();
        }
    }
}
