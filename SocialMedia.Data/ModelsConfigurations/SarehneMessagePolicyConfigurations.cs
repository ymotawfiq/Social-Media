

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class SarehneMessagePolicyConfigurations : IEntityTypeConfiguration<SarehneMessagePolicy>
    {
        public void Configure(EntityTypeBuilder<SarehneMessagePolicy> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Policy).WithMany(e => e.SarehneMessagePolicies)
                .HasForeignKey(e => e.PolicyId);
            builder.Property(e => e.PolicyId).IsRequired().HasColumnName("Policy Id");
            builder.HasIndex(e => e.PolicyId).IsUnique();
        }
    }
}
