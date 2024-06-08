

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class GroupMembersConfigurations : IEntityTypeConfiguration<GroupMember>
    {
        public void Configure(EntityTypeBuilder<GroupMember> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.User).WithMany(e => e.GroupMembers).HasForeignKey(e => e.MemberId);
            builder.HasOne(e => e.Group).WithMany(e => e.GroupMembers).HasForeignKey(e => e.GroupId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Property(e => e.GroupId).IsRequired().HasColumnName("Group Id");
            builder.Property(e => e.MemberId).IsRequired().HasColumnName("Member Id");
            builder.HasOne(e => e.Role).WithMany(e => e.GroupMembers).HasForeignKey(e => e.RoleId);
            builder.HasIndex(e => new { e.RoleId, e.MemberId, e.GroupId }).IsUnique();
            builder.Property(e => e.RoleId).IsRequired().HasColumnName("Role Id");
        }
    }
}
