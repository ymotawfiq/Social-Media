

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Data.ModelsConfigurations
{
    public class PostImagesConfigurations : IEntityTypeConfiguration<PostImages>
    {
        public void Configure(EntityTypeBuilder<PostImages> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.Post).WithMany(e => e.PostImages).HasForeignKey(e => e.PostId);
            builder.Property(e => e.PostId).IsRequired().HasColumnName("Post Id");
            builder.Property(e => e.ImageUrl).IsRequired().HasColumnName("Image Url");
            builder.HasIndex(e => e.ImageUrl).IsUnique();
        }
    }
}
