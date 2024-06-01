

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class PostCommentReplayConfigurations : IEntityTypeConfiguration<PostCommentReplay>
    {
        public void Configure(EntityTypeBuilder<PostCommentReplay> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.PostComment).WithMany(e => e.PostCommentReplays)
                .HasForeignKey(e => e.PostCommentId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.User).WithMany(e => e.PostCommentReplays).HasForeignKey(e => e.UserId);
            builder.Property(e => e.UserId).IsRequired().HasColumnName("User Id");
            builder.Property(e => e.ReplayImage).IsRequired(false).HasColumnName("Replay_Image");
            builder.Property(e => e.PostCommentId).IsRequired().HasColumnName("Post comment Id");
            builder.Property(e => e.Replay).IsRequired(false).HasColumnName("Replay");
            builder.ToTable(e => e.HasCheckConstraint("EncureReplayAndReplayImageNotNull",
                $"(Replay is NOT null AND Replay_Image is NOT null) OR" +
                $" (Replay_Image is null AND Replay is NOT null)" +
                $" OR (Replay is null AND Replay_Image is NOT null)"));
            builder.HasOne(e => e.PostCommentReplayChildReplay).WithMany(e => e.PostCommentReplays)
                .HasForeignKey(e => e.PostCommentReplayId);
            builder.Property(e => e.PostCommentReplayId).IsRequired(false)
                .HasColumnName("Post Comment Replay Id");
        }
    }
}
