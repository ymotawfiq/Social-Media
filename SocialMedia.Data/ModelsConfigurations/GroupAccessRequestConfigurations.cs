
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    internal class GroupAccessRequestConfigurations : IEntityTypeConfiguration<GroupAccessRequest>
    {
        public void Configure(EntityTypeBuilder<GroupAccessRequest> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.User).WithMany(e => e.GroupAccessRequests).HasForeignKey(e => e.UserId);
            builder.HasOne(e => e.Group).WithMany(e => e.GroupAccessRequests).HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasIndex(e => new { e.GroupId, e.UserId }).IsUnique();
            builder.Property(e => e.UserId).IsRequired().HasColumnName("User Id");
            builder.Property(e => e.GroupId).IsRequired().HasColumnName("Group Id");
        }
    }
}
