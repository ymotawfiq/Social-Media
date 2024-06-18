

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Data.ModelsConfigurations
{
    public class BlockConfigurations : IEntityTypeConfiguration<Block>
    {
        public void Configure(EntityTypeBuilder<Block> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.User).WithMany(e => e.Blocks).HasForeignKey(e => e.UserId);
            builder.Property(e => e.UserId).IsRequired().HasColumnName("User Id");
            builder.Property(e => e.BlockedUserId).IsRequired().HasColumnName("Blocked User Id");
            builder.HasIndex(e => new { e.UserId, e.BlockedUserId }).IsUnique();
        }
    }
}
