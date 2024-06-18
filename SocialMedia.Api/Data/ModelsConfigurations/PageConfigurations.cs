

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Data.ModelsConfigurations
{
    public class PageConfigurations : IEntityTypeConfiguration<Page>
    {
        public void Configure(EntityTypeBuilder<Page> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Name).IsRequired().HasMaxLength(50);
            builder.Property(e => e.Description).IsRequired();
            builder.Property(e => e.CreatedAt).IsRequired();
            builder.HasOne(e => e.Creator).WithMany(e => e.Pages).HasForeignKey(e => e.CreatorId);
            builder.Property(e => e.CreatorId).IsRequired();
        }
    }
}
