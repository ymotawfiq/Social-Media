

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class PostConfigurations : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.PostPolicy).WithMany(e => e.Posts).HasForeignKey(e => e.PostPolicyId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.ReactPolicy).WithMany(e => e.Posts).HasForeignKey(e => e.ReactPolicyId);
            builder.HasOne(e => e.CommentPolicy).WithMany(e => e.Posts).HasForeignKey(e => e.CommentPolicyId);
            builder.HasOne(e => e.User).WithMany(e => e.Posts).HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Property(e => e.UserId).IsRequired().HasColumnName("User Id");
            builder.Property(e => e.CommentPolicyId).IsRequired().HasColumnName("Comment Policy Id");
            builder.Property(e => e.ReactPolicyId).IsRequired().HasColumnName("React Policy Id");
            builder.Property(e => e.PostPolicyId).IsRequired().HasColumnName("Post Policy Id");
            builder.Property(e => e.PostedAt).IsRequired().HasColumnName("Posted At Date");
            builder.Property(e => e.UpdatedAt).IsRequired().HasColumnName("Post Update Date");
            builder.Property(e => e.Content).IsRequired().HasColumnName("Post content");
        }
    }
}
