

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class ReactPolicyConfigurations : IEntityTypeConfiguration<ReactPolicy>
    {
        public void Configure(EntityTypeBuilder<ReactPolicy> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.PolicyId).IsUnique();
            builder.HasOne(e => e.Policy).WithMany(e => e.ReactPolicies).HasForeignKey(e => e.PolicyId);
            builder.Property(e => e.PolicyId).IsRequired().HasColumnName("Policy Id");
        }
    }
}
