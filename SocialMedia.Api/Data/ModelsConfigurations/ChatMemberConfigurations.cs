using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Data.ModelsConfigurations
{
    public class ChatMemberConfigurations : IEntityTypeConfiguration<ChatMember>
    {
        public void Configure(EntityTypeBuilder<ChatMember> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.User).WithMany(e => e.ChatMembers1).HasForeignKey(e => e.MemberId);
            builder.HasOne(e => e.Chat).WithMany(e => e.ChatMembers).HasForeignKey(e => e.ChatId);
            builder.Property(e => e.ChatId).IsRequired().HasColumnName("Chat Id");
            builder.Property(e => e.MemberId).IsRequired().HasColumnName("Member 1 Id");
            builder.HasIndex(e => new { e.MemberId, e.ChatId }).IsUnique();
            builder.Property(e => e.IsMember).IsRequired();
        }
    }
}
