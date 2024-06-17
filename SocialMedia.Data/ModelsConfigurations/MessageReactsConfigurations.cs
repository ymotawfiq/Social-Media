

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    internal class MessageReactsConfigurations : IEntityTypeConfiguration<MessageReact>
    {
        public void Configure(EntityTypeBuilder<MessageReact> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.User).WithMany(e => e.MessageReacts).HasForeignKey(e => e.ReactedUserId);
            builder.HasOne(e => e.React).WithMany(e => e.MessageReacts).HasForeignKey(e => e.ReactId);
            builder.HasOne(e => e.ChatMessage).WithMany(e => e.MessageReacts).HasForeignKey(e => e.MessageId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Property(e => e.MessageId).IsRequired().HasColumnName("Message Id");
            builder.Property(e => e.ReactedUserId).IsRequired().HasColumnName("Reacted User Id");
            builder.Property(e => e.ReactId).IsRequired().HasColumnName("React Id");
        }
    }
}
