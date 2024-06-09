

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class GroupPostsConfigurations : IEntityTypeConfiguration<GroupPost>
    {
        public void Configure(EntityTypeBuilder<GroupPost> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Group).WithMany(e => e.GroupPosts).HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.Post).WithMany(e => e.GroupPosts).HasForeignKey(e => e.PostId);
            builder.HasOne(e => e.User).WithMany(e => e.GroupPosts).HasForeignKey(e => e.UserId);
            builder.Property(e => e.UserId).IsRequired().HasColumnName("User Id");
            builder.Property(e => e.PostId).IsRequired().HasColumnName("Post Id");
            builder.Property(e => e.GroupId).IsRequired().HasColumnName("Group Id");
        }
    }
}
