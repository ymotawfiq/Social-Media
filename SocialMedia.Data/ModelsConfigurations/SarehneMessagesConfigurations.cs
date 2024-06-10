

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class SarehneMessagesConfigurations : IEntityTypeConfiguration<SarehneMessage>
    {
        public void Configure(EntityTypeBuilder<SarehneMessage> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.User).WithMany(e => e.SarehneMessages).HasForeignKey(e => e.ReceiverId);
            builder.Property(e => e.ReceiverId).IsRequired().HasColumnName("Receiver Id");
            builder.Property(e => e.Message).IsRequired().HasColumnName("Message")
                .HasMaxLength(1000);
            builder.Property(e => e.SenderName).IsRequired().HasDefaultValue("Anonymous");
            builder.HasOne(e => e.SarehneMessagePolicy).WithMany(e => e.SarehneMessages)
                .HasForeignKey(e => e.MessagePolicyId);
            builder.Property(e => e.MessagePolicyId).IsRequired().HasColumnName("Message Policy Id");
            builder.Property(e => e.SentAt).IsRequired().HasDefaultValue(DateTime.Now);
        }
    }
}
