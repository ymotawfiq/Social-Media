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
            builder.HasOne(e => e.User1).WithMany(e => e.ChatMembers1).HasForeignKey(e => e.Member1Id);
            builder.HasOne(e => e.User2).WithMany(e => e.ChatMembers2).HasForeignKey(e => e.Member2Id);
            builder.HasOne(e => e.Chat).WithMany(e => e.ChatMembers).HasForeignKey(e => e.ChatId);
            builder.Property(e => e.ChatId).IsRequired().HasColumnName("Chat Id");
            builder.Property(e => e.Member1Id).IsRequired().HasColumnName("Member 1 Id");
            builder.Property(e => e.Member2Id).IsRequired(false).HasColumnName("Member 2 Id");
            builder.HasIndex(e => new { e.Member1Id, e.ChatId }).IsUnique();
            builder.HasIndex(e => new { e.Member2Id, e.ChatId }).IsUnique();
            builder.HasIndex(e => new { e.Member2Id, e.Member1Id, e.ChatId }).IsUnique();
            builder.HasIndex(e => new { e.Member2Id, e.Member1Id }).IsUnique();
            builder.Property(e => e.IsMember).IsRequired();
        }
    }
}
