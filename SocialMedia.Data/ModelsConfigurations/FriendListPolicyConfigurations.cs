

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class FriendListPolicyConfigurations : IEntityTypeConfiguration<FriendListPolicy>
    {
        public void Configure(EntityTypeBuilder<FriendListPolicy> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Policy).WithMany(e => e.FriendListPolicies).HasForeignKey(e => e.PolicyId);
            builder.Property(e => e.PolicyId).IsRequired().HasColumnName("Policy Id");
            builder.HasIndex(e => e.PolicyId).IsUnique();
        }
    }
}
