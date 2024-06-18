

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Data.ModelsConfigurations
{
    public class GroupRoleConfigurations : IEntityTypeConfiguration<GroupRole>
    {
        public void Configure(EntityTypeBuilder<GroupRole> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasIndex(e => e.RoleName).IsUnique();
            builder.Property(e => e.RoleName).IsRequired();
        }
    }
}
