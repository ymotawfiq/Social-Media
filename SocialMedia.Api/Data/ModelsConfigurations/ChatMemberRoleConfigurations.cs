using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Data.ModelsConfigurations
{
    public class ChatMemberRoleConfigurations : IEntityTypeConfiguration<ChatMemberRole>
    {
        public void Configure(EntityTypeBuilder<ChatMemberRole> builder)
        {
            builder.HasKey(e=>e.Id);
            builder.HasOne(e => e.ChatMember).WithMany(e => e.ChatMemberRoles)
                .HasForeignKey(e => e.ChatMemberId);
            builder.HasOne(e => e.Role).WithMany(e => e.ChatMemberRoles).HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(e => e.RoleId).IsRequired().HasColumnName("Role Id");
            builder.Property(e => e.ChatMemberId).IsRequired().HasColumnName("Chat Member Id");
            builder.HasIndex(e => new { e.ChatMemberId, e.RoleId }).IsUnique();
        }
    }
}
