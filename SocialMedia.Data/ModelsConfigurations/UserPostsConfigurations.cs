

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class UserPostsConfigurations : IEntityTypeConfiguration<UserPosts>
    {
        public void Configure(EntityTypeBuilder<UserPosts> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Post).WithMany(e => e.UserPosts).HasForeignKey(e => e.PostId);
            builder.HasOne(e => e.User).WithMany(e => e.UserPosts).HasForeignKey(e => e.UserId);
            builder.Property(e => e.UserId).IsRequired().HasColumnName("User Id");
            builder.Property(e => e.PostId).IsRequired().HasColumnName("Post Id");
            builder.HasIndex(e => e.PostId).IsUnique();
        }
    }
}
