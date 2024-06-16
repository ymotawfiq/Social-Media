
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    internal class FriendsConfigurations : IEntityTypeConfiguration<Friend>
    {
        public void Configure(EntityTypeBuilder<Friend> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.FriendId).IsRequired().HasColumnName("Friend Id");
            builder.HasOne(e => e.User).WithMany(e => e.Friends).HasForeignKey(e => e.UserId);
            builder.Property(e => e.UserId).IsRequired().HasColumnName("User Id");
            builder.HasIndex(e => new { e.UserId, e.FriendId }).IsUnique();
        }
    }
}
