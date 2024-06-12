

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
                   .ApplyConfiguration(new PostPolicyConfigurations())
                   .ApplyConfiguration(new SavedPostsConfigurations())
                   .ApplyConfiguration(new UserSavedPostsFoldersConfigurations())
                   .ApplyConfiguration(new SpecialPostReactsConfigurations())
                   .ApplyConfiguration(new SpecialCommentReactsConfigurations())
                   .ApplyConfiguration(new PostReactsConfigurations())
                   .ApplyConfiguration(new PostCommentsConfigurations())
                   .ApplyConfiguration(new PostCommentReplayConfigurations())
                   .ApplyConfiguration(new PageConfigurations())
                   .ApplyConfiguration(new UserPageConfiguration())
                   .ApplyConfiguration(new PagePostsConfigurations())
                   .ApplyConfiguration(new PageFollowersConfigurations())
                   .ApplyConfiguration(new GroupPolicyConfigurations())
                   .ApplyConfiguration(new GroupRoleConfigurations())
                   .ApplyConfiguration(new GroupConfigurations())
                   .ApplyConfiguration(new GroupMembersConfigurations())
                   .ApplyConfiguration(new GroupAccessRequestConfigurations())
                   .ApplyConfiguration(new GroupMemberRoleConfigurations())
                   .ApplyConfiguration(new GroupPostsConfigurations())
                   .ApplyConfiguration(new SarehneMessagesConfigurations())
                   .ApplyConfiguration(new SarehneMessagePolicyConfigurations());
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
        public DbSet<PostsPolicy> PostPolicies { get; set; }
        public DbSet<SavedPosts> SavedPosts { get; set; }
        public DbSet<UserSavedPostsFolders> UserSavedPostsFolders { get; set; }
        public DbSet<SpecialPostReacts> SpecialPostReacts { get; set; }
        public DbSet<SpecialCommentReacts> SpecialCommentReacts { get; set; }
        public DbSet<PostReacts> PostReacts { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostCommentReplay> PostCommentReplay { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<UserPage> UserPages { get; set; }
        public DbSet<PagePosts> PagePosts { get; set; }
        public DbSet<PageFollower> PageFollowers { get; set; }
        public DbSet<GroupPolicy> GroupPolicies { get; set; }
        public DbSet<GroupRole> GroupRoles { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<GroupAccessRequest> GroupAccessRequests { get; set; }
        public DbSet<GroupMemberRole> GroupMemberRoles { get; set; }
        public DbSet<GroupPost> GroupPosts { get; set; }
        public DbSet<SarehneMessage> SarehneMessages { get; set; }
        public DbSet<SarehneMessagePolicy> SarehneMessagePolicies { get; set; }

    }
}
