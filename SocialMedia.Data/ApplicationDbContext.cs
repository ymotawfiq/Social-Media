

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Data.ModelsConfigurations;

namespace SocialMedia.Data
{
    public class ApplicationDbContext : IdentityDbContext<SiteUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //SeedRoles(builder);
            ApplyModelsConfigurations(builder);
        }

        //private void SeedRoles(ModelBuilder builder)
        //{
        //    builder.Entity<IdentityRole>().HasData(
        //        new IdentityRole { ConcurrencyStamp = "1", Name = "Admin", NormalizedName = "Admin" },
        //        new IdentityRole { ConcurrencyStamp = "2", Name = "User", NormalizedName = "User" },
        //        new IdentityRole { ConcurrencyStamp = "3", Name = "Owner", NormalizedName = "Owner" },
        //        new IdentityRole { ConcurrencyStamp = "4", Name = "Moderator", NormalizedName = "Moderator" },
        //        new IdentityRole { ConcurrencyStamp = "5", Name = "GroupMember", NormalizedName = "GroupMember" }
        //        );
        //}

        private void ApplyModelsConfigurations(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ReactConfiguration())
                   .ApplyConfiguration(new FriendRequestsConfiguration())
                   .ApplyConfiguration(new FriendsConfigurations())
                   .ApplyConfiguration(new SiteUserConfigurations())
                   .ApplyConfiguration(new FollowerConfigurations())
                   .ApplyConfiguration(new BlockConfigurations())
                   .ApplyConfiguration(new PolicyConfigurations())
                   .ApplyConfiguration(new ReactPolicyConfigurations())
                   .ApplyConfiguration(new CommentPolicyConfigurations())
                   .ApplyConfiguration(new PostConfigurations())
                   .ApplyConfiguration(new UserPostsConfigurations())
                   .ApplyConfiguration(new PostImagesConfigurations())
                   .ApplyConfiguration(new AccountPolicyConfigurations())
                   .ApplyConfiguration(new PostViewConfigurations())
                   .ApplyConfiguration(new FriendListPolicyConfigurations())
                   .ApplyConfiguration(new PostPolicyConfigurations());
        }


        public DbSet<React> Reacts { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<ReactPolicy> ReactPolicies { get; set; }
        public DbSet<CommentPolicy> CommentPolicies { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<UserPosts> UserPosts { get; set; }
        public DbSet<PostImages> PostImages { get; set; }
        public DbSet<AccountPolicy> AccountPolicies { get; set; }
        public DbSet<PostView> PostViews { get; set; }
        public DbSet<FriendListPolicy> FriendListPolicies { get; set; }
        public DbSet<AccountPostsPolicy> PostPolicies { get; set; }
    }
}
