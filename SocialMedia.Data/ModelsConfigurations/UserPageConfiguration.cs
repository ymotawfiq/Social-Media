

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class UserPageConfiguration : IEntityTypeConfiguration<UserPage>
    {
        public void Configure(EntityTypeBuilder<UserPage> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.User).WithMany(e => e.UserPages).HasForeignKey(e => e.UserId);
            builder.Property(e => e.UserId).IsRequired().HasColumnName("User Id");
            builder.HasOne(e => e.Page).WithMany(e => e.UserPages).HasForeignKey(e => e.PageId);
            builder.Property(e => e.PageId).IsRequired().HasColumnName("Page Id");
            builder.HasIndex(e => new { e.PageId, e.UserId }).IsUnique();
        }
    }
}
