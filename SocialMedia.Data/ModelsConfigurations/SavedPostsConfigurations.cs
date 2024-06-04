

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class SavedPostsConfigurations : IEntityTypeConfiguration<SavedPosts>
    {
        public void Configure(EntityTypeBuilder<SavedPosts> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.User).WithMany(e => e.SavedPosts).HasForeignKey(e => e.UserId);
            builder.HasOne(e => e.UserSavedPostsFolder).WithMany(e => e.SavedPosts)
                .HasForeignKey(e => e.FolderId);
            builder.HasOne(e => e.Post).WithMany(e => e.SavedPosts).HasForeignKey(e => e.PostId);
            builder.Property(e => e.FolderId).IsRequired().HasColumnName("Folder Id");
            builder.Property(e => e.PostId).IsRequired().HasColumnName("Post Id");
            builder.Property(e => e.UserId).IsRequired().HasColumnName("User Id");
            builder.HasIndex(e => new { e.PostId, e.FolderId }).IsUnique();
        }
    }
}
