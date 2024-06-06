
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class PageFollowersConfigurations : IEntityTypeConfiguration<PageFollower>
    {
        public void Configure(EntityTypeBuilder<PageFollower> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Page).WithMany(e => e.PageFollowers).HasForeignKey(e => e.PageId);
            builder.HasOne(e => e.User).WithMany(e => e.PageFollowers).HasForeignKey(e => e.FollowerId);
            builder.Property(e => e.FollowerId).IsRequired().HasColumnName("Follower Id");
            builder.Property(e => e.PageId).IsRequired().HasColumnName("Page Id");
            builder.HasIndex(e => new { e.PageId, e.FollowerId }).IsUnique();
        }
    }
}
