

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class SiteUserConfigurations : IEntityTypeConfiguration<SiteUser>
    {
        public void Configure(EntityTypeBuilder<SiteUser> builder)
        {
            builder.Property(e => e.FirstName).IsRequired().HasColumnName("First Name");
            builder.Property(e => e.LastName).IsRequired().HasColumnName("Last Name");
            builder.HasIndex(e => e.UserName).IsUnique();
            builder.HasIndex(e => e.Email).IsUnique();
            builder.Property(e => e.Email).IsRequired().HasColumnName("Email");
            builder.Property(e => e.UserName).IsRequired().HasColumnName("User Name");
            builder.Property(e => e.DisplayName).IsRequired().HasColumnName("Display Name");
            builder.Property(e => e.IsFriendListPrivate).IsRequired()
                .HasColumnName("Is Friend List Private").HasDefaultValue(true);
            builder.HasOne(e => e.AccountPolicy).WithMany(e => e.Users).HasForeignKey(e => e.AccountPolicyId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Property(e => e.AccountPolicyId).IsRequired().HasColumnName("User Account Policy Id");
        }
    }
}
