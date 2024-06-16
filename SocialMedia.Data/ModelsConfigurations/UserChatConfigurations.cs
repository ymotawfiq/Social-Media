

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    internal class UserChatConfigurations : IEntityTypeConfiguration<UserChat>
    {
        public void Configure(EntityTypeBuilder<UserChat> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => new { e.User1Id, e.User2Id }).IsUnique();
            builder.HasOne(e => e.User1).WithMany(e => e.User1Chats).HasForeignKey(e => e.User1Id);
            builder.HasOne(e => e.User2).WithMany(e => e.User2Chats).HasForeignKey(e => e.User2Id)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Property(e => e.User2Id).IsRequired();
            builder.Property(e => e.User1Id).IsRequired();
        }
    }
}
