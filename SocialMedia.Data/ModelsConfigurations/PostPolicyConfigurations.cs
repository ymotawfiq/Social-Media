

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class PostPolicyConfigurations : IEntityTypeConfiguration<AccountPostsPolicy>
    {
        public void Configure(EntityTypeBuilder<AccountPostsPolicy> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.PolicyId).IsRequired().HasColumnName("Policy Id");
            builder.HasOne(e => e.Policy).WithMany(e => e.PostPolicies).HasForeignKey(e => e.PolicyId);
            builder.HasIndex(e => e.PolicyId).IsUnique();
        }
    }
}
