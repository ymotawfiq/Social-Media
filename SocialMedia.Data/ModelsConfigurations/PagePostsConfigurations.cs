

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class PagePostsConfigurations : IEntityTypeConfiguration<PagePost>
    {
        public void Configure(EntityTypeBuilder<PagePost> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Page).WithMany(e => e.PagePosts).HasForeignKey(e => e.PageId);
            builder.HasOne(e => e.Post).WithMany(e => e.PagePosts).HasForeignKey(e => e.PostId);
            builder.Property(e => e.PostId).IsRequired().HasColumnName("Post Id");
            builder.Property(e => e.PageId).IsRequired().HasColumnName("Page Id");
        }
    }
}
