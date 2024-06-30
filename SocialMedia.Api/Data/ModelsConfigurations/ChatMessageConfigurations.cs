using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Data.ModelsConfigurations
{
    public class ChatMessageConfigurations : IEntityTypeConfiguration<ChatMessage>
    {
        public void Configure(EntityTypeBuilder<ChatMessage> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Chat).WithMany(e => e.ChatMessages).HasForeignKey(e => e.ChatId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.User).WithMany(e => e.ChatMessages).HasForeignKey(e => e.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Property(e => e.SenderId).IsRequired().HasColumnName("Message Sender Id");
            builder.Property(e => e.ChatId).IsRequired().HasColumnName("Chat Id");
            builder.Property(e => e.Message).IsRequired(false).HasColumnName("Message");
            builder.Property(e => e.Photo).IsRequired(false).HasColumnName("Photo");
            builder.Property(e => e.SentAt).IsRequired().HasDefaultValueSql("current_timestamp");
            builder.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("current_timestamp");
            builder.ToTable(t => t.HasCheckConstraint("MessagePhotoCheck",
                "(Photo is NOT null AND Message is null) OR (Photo is null AND Message is NOT null) OR " +
                "(Message is NOT null AND Photo is NOT null)"));
        }
    }
}
