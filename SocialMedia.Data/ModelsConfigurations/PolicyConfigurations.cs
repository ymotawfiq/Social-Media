

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class PolicyConfigurations : IEntityTypeConfiguration<Policy>
    {
        public void Configure(EntityTypeBuilder<Policy> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.PolicyType).IsRequired().HasColumnName("Policy Type");
            builder.HasIndex(e => e.PolicyType).IsUnique();
        }
    }
}
