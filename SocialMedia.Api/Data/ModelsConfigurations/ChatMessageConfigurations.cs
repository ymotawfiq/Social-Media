

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
            builder.HasOne(e => e.User).WithMany(e => e.ChatMessages).HasForeignKey(e => e.SenderId);
            builder.HasOne(e => e.Chat).WithMany(e => e.ChatMessages).HasForeignKey(e => e.ChatId);
            builder.Property(e => e.ChatId).IsRequired().HasColumnName("Chat Id");
            builder.Property(e => e.SenderId).IsRequired().HasColumnName("Sender Id");
            builder.Property(e => e.Message).HasColumnName("Message");
            builder.Property(e => e.Photo).HasColumnName("Photo");
            builder.ToTable(t=>t.HasCheckConstraint("CheckPhotoAndImageNotPothNull",
                "(Photo is NOT null AND Message is null) OR (Photo is null AND Message is NOT null)"));
            builder.Property(e=>e.SentAt).HasDefaultValueSql("getdate()").IsRequired();
            builder.HasOne(e => e.MessageReplay).WithMany(e => e.MessageReplays)
                .HasForeignKey(e => e.MessageId);
            builder.Property(e => e.MessageId).IsRequired(false);
        }
    }
}
