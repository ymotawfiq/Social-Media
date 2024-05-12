
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class PostViewConfigurations : IEntityTypeConfiguration<PostView>
    {
        public void Configure(EntityTypeBuilder<PostView> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.PostId).IsRequired().HasColumnName("Post Id");
            builder.Property(e => e.UserId).IsRequired().HasColumnName("User Id");
            builder.HasOne(e => e.Post).WithMany(e => e.PostViews).HasForeignKey(e => e.PostId);
            builder.HasOne(e => e.User).WithMany(e => e.PostViews).HasForeignKey(e => e.UserId);
            builder.Property(e => e.ViewNumber).HasDefaultValue(0).HasColumnName("Post view number");
            builder.HasIndex(e => e.PostId).IsUnique();
        }
    }
}
