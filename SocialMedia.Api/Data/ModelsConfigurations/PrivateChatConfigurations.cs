using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Data.ModelsConfigurations
{
    public class PrivateChatConfigurations : IEntityTypeConfiguration<PrivateChat>
    {
        public void Configure(EntityTypeBuilder<PrivateChat> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Chat).WithMany(e => e.PrivateChats).HasForeignKey(e => e.ChatId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.User1).WithMany(e => e.PrivateChats1).HasForeignKey(e => e.User1Id)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.User2).WithMany(e => e.PrivateChats2).HasForeignKey(e => e.User2Id)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasIndex(e => new { e.User1Id, e.User2Id }).IsUnique();
            builder.HasIndex(e => new { e.User1Id, e.ChatId }).IsUnique();
            builder.HasIndex(e => new { e.User2Id, e.ChatId }).IsUnique();
            builder.Property(e => e.User2Id).IsRequired().HasColumnName("User 2 Id");
            builder.Property(e => e.User1Id).IsRequired().HasColumnName("User 1 Id");
            builder.Property(e => e.ChatId).IsRequired().HasColumnName("Chat Id");
        }
    }
}
