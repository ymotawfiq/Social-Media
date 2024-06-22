using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Org.BouncyCastle.Asn1.CryptoPro;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Data.ModelsConfigurations
{
    public class ChatConfigurations : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Policy).WithMany(e => e.Chats).HasForeignKey(e => e.PolicyId);
            builder.Property(e => e.PolicyId).IsRequired().HasColumnName("Chat Policy Id");
            builder.Property(e => e.Name).IsRequired(false).HasColumnName("Name");
            builder.Property(e => e.Description).IsRequired(false).HasColumnName("Description");
            builder.HasOne(e => e.User).WithMany(e => e.Chats).HasForeignKey(e => e.CreatorId);
            builder.Property(e => e.CreatorId).IsRequired(false).HasColumnName("Creator Id");
        }
    }
}
