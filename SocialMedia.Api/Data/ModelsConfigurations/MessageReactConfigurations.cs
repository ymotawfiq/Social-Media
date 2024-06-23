using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Data.ModelsConfigurations
{
    public class MessageReactConfigurations : IEntityTypeConfiguration<MessageReact>
    {
        public void Configure(EntityTypeBuilder<MessageReact> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Message).WithMany(e => e.MessageReacts).HasForeignKey(e => e.MessageId);
            builder.HasOne(e => e.React).WithMany(e => e.MessageReacts).HasForeignKey(e => e.ReactId);
            builder.HasOne(e => e.User).WithMany(e => e.MessageReacts).HasForeignKey(e => e.ReactedUserId);
            builder.HasIndex(e => new { e.ReactedUserId, e.MessageId }).IsUnique();
            builder.Property(e => e.MessageId).IsRequired().HasColumnName("Message Id");
            builder.Property(e => e.ReactId).IsRequired().HasColumnName("React Id");
            builder.Property(e => e.ReactedUserId).IsRequired().HasColumnName("Reacted User Id");
            builder.HasIndex(e => new { e.MessageId, e.ReactedUserId }).IsUnique();
        }
    }
}
