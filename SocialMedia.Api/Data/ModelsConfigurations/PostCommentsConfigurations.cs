
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Data.ModelsConfigurations
{
    public class PostCommentsConfigurations : IEntityTypeConfiguration<PostComment>
    {
        public void Configure(EntityTypeBuilder<PostComment> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Post).WithMany(e => e.PostComments).HasForeignKey(e => e.PostId);
            builder.HasOne(e => e.User).WithMany(e => e.PostComments).HasForeignKey(e => e.UserId);
            builder.HasOne(e => e.BaseComment).WithMany(e => e.Replays).HasForeignKey(e => e.CommentId);
            builder.Property(e => e.CommentId).IsRequired(false).HasColumnName("Base Comment Id");
            builder.Property(e => e.UserId).IsRequired().HasColumnName("User Id");
            builder.Property(e => e.PostId).IsRequired().HasColumnName("Post Id");
            builder.Property(e => e.Comment).IsRequired().HasMaxLength(500);
            builder.Property(e => e.CommentImage).IsRequired(false);
            builder.HasIndex(e => e.CommentImage).IsUnique();
        }
    }
}
