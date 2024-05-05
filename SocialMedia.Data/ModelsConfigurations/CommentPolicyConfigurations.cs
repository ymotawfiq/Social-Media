

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class CommentPolicyConfigurations : IEntityTypeConfiguration<CommentPolicy>
    {
        public void Configure(EntityTypeBuilder<CommentPolicy> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.PolicyId).IsUnique();
            builder.Property(e => e.PolicyId).IsRequired().HasColumnName("Policy Id");
            builder.HasOne(e => e.Policy).WithMany(e => e.CommentPolicies).HasForeignKey(e => e.PolicyId);
        }
    }
}
