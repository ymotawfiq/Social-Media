

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.ModelsConfigurations
{
    public class SiteUserConfigurations : IEntityTypeConfiguration<SiteUser>
    {
        public void Configure(EntityTypeBuilder<SiteUser> builder)
        {
            builder.Property(e => e.FirstName).IsRequired().HasColumnName("First Name");
            builder.Property(e => e.LastName).IsRequired().HasColumnName("Last Name");
            builder.HasIndex(e => e.UserName).IsUnique();
            builder.HasIndex(e => e.Email).IsUnique();
            builder.Property(e => e.Email).IsRequired().HasColumnName("Email");
            builder.Property(e => e.UserName).IsRequired().HasColumnName("User Name");
            builder.Property(e => e.DisplayName).IsRequired().HasColumnName("Display Name");
            builder.HasOne(e => e.AccountPolicy).WithMany(e => e.UserAccountPolicies)
                .HasForeignKey(e => e.AccountPolicyId);
            builder.HasOne(e => e.ReactPolicy).WithMany(e => e.UserReactPolicies)
                .HasForeignKey(e => e.ReactPolicyId);
            builder.HasOne(e => e.CommentPolicy).WithMany(e => e.UserCommentPolicies)
                .HasForeignKey(e => e.CommentPolicyId);
            builder.HasOne(e => e.PostPolicy).WithMany(e => e.UserPostPolicies)
                .HasForeignKey(e => e.AccountPostPolicyId);
            builder.HasOne(e => e.FriendListPolicy).WithMany(e => e.UserFriendListPolicies)
                .HasForeignKey(e => e.FriendListPolicyId);
            builder.Property(e => e.AccountPolicyId).HasColumnName("Account Policy Id");
            builder.Property(e => e.ReactPolicyId).HasColumnName("React Policy Id");
            builder.Property(e => e.CommentPolicyId).HasColumnName("Comment Policy Id");
            builder.Property(e => e.FriendListPolicyId).HasColumnName("Friend list Policy Id");
            builder.Property(e => e.AccountPostPolicyId).HasColumnName("Account post Policy Id");
        }
    }
}
