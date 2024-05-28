

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class SpecialCommentReactsConfigurations : IEntityTypeConfiguration<SpecialCommentReacts>
    {
        public void Configure(EntityTypeBuilder<SpecialCommentReacts> builder)
        {
            builder.HasKey(e => e.Id);
            builder.HasOne(e => e.React).WithMany(e => e.SpecialCommentReacts).HasForeignKey(e => e.ReactId);
            builder.Property(e => e.ReactId).IsRequired().HasColumnName("React Id");
            builder.HasIndex(e => e.ReactId).IsUnique();
        }
    }
}
