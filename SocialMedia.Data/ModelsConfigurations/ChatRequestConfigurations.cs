

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    internal class ChatRequestConfigurations : IEntityTypeConfiguration<ChatRequest>
    {
        public void Configure(EntityTypeBuilder<ChatRequest> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.UserWhoReceivedRequestId, e.UserWhoSentRequestId }).IsUnique();
            builder.HasOne(e => e.UserWhoSent).WithMany(e => e.SentUserRequests)
                .HasForeignKey(e => e.UserWhoSentRequestId).OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(e => e.UserWhoReceived).WithMany(e => e.ReceivedUserRequests)
                .HasForeignKey(e => e.UserWhoReceivedRequestId);
            builder.Property(e => e.UserWhoReceivedRequestId).IsRequired();
            builder.Property(e => e.UserWhoSentRequestId).IsRequired();
            builder.Property(e => e.SentAt).IsRequired().HasDefaultValueSql("getdate()");
        }
    }
}
