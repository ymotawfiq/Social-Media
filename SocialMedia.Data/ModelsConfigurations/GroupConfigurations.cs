
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    internal class GroupConfigurations : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            builder.HasOne(e => e.User).WithMany(e => e.Groups).HasForeignKey(e => e.CreatedUserId);
            builder.HasOne(e => e.GroupPolicy).WithMany(e => e.GroupPolicies)
                .HasForeignKey(e => e.GroupPolicyId);
            builder.Property(e => e.CreatedUserId).IsRequired().HasColumnName("Group Creator Id");
            builder.Property(e => e.GroupPolicyId).IsRequired().HasColumnName("Group Policy Id");
        }
    }
}
