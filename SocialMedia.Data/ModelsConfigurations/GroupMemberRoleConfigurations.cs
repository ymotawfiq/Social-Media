

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    internal class GroupMemberRoleConfigurations : IEntityTypeConfiguration<GroupMemberRole>
    {
        public void Configure(EntityTypeBuilder<GroupMemberRole> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.GroupMember).WithMany(e => e.GroupMemberRoles)
                .HasForeignKey(e => e.GroupMemberId);
            builder.HasOne(e => e.GroupRole).WithMany(e => e.GroupMemberRoles).HasForeignKey(e => e.RoleId);
            builder.HasIndex(e => new { e.RoleId, e.GroupMemberId }).IsUnique();
            builder.Property(e => e.GroupMemberId).IsRequired().HasColumnName("Group Member Id");
            builder.Property(e => e.RoleId).IsRequired().HasColumnName("Role Id");
        }
    }
}
