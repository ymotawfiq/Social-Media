

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class AccountPolicyConfigurations : IEntityTypeConfiguration<AccountPolicy>
    {
        public void Configure(EntityTypeBuilder<AccountPolicy> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.PolicyId).IsRequired().HasColumnName("Policy Id");
            builder.HasOne(e => e.Policy).WithMany(e => e.AccountPolicies).HasForeignKey(e => e.PolicyId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasIndex(e => e.PolicyId).IsUnique();
        }
    }
}
