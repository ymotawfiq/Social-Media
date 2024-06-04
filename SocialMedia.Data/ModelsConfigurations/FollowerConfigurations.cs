
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class FollowerConfigurations : IEntityTypeConfiguration<Follower>
    {
        public void Configure(EntityTypeBuilder<Follower> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.FollowerId).IsRequired().HasColumnName("Follower Id");
            builder.Property(e => e.UserId).IsRequired().HasColumnName("User Id");
            builder.HasOne(e => e.User).WithMany(e => e.Followers).HasForeignKey(e => e.UserId);
            builder.HasIndex(e => new { e.FollowerId, e.UserId }).IsUnique();
        }
    }
}
