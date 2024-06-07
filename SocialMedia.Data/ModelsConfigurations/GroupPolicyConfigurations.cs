

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class GroupPolicyConfigurations : IEntityTypeConfiguration<GroupPolicy>
    {
        public void Configure(EntityTypeBuilder<GroupPolicy> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Policy).WithMany(e => e.GroupPolicies).HasForeignKey(e => e.PolicyId);
            builder.HasIndex(e => e.PolicyId).IsUnique();
            builder.Property(e => e.PolicyId).IsRequired().HasColumnName("Policy Id");
        }
    }
}
