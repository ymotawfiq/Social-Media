

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Data.ModelsConfigurations;

namespace SocialMedia.Api.Data
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
                   .ApplyConfiguration(new PostConfigurations())
                   .ApplyConfiguration(new PostImagesConfigurations())
                   .ApplyConfiguration(new PostViewConfigurations())
                   .ApplyConfiguration(new SavedPostsConfigurations())
                   .ApplyConfiguration(new UserSavedPostsFoldersConfigurations())
                   .ApplyConfiguration(new PostReactsConfigurations())
                   .ApplyConfiguration(new PostCommentsConfigurations())
                   .ApplyConfiguration(new PageConfigurations())
                   .ApplyConfiguration(new PagePostsConfigurations())
                   .ApplyConfiguration(new PageFollowersConfigurations())
                   .ApplyConfiguration(new GroupRoleConfigurations())
                   .ApplyConfiguration(new GroupConfigurations())
                   .ApplyConfiguration(new GroupMembersConfigurations())
                   .ApplyConfiguration(new GroupAccessRequestConfigurations())
                   .ApplyConfiguration(new GroupMemberRoleConfigurations())
                   .ApplyConfiguration(new GroupPostsConfigurations())
                   .ApplyConfiguration(new SarehneMessagesConfigurations())
                   .ApplyConfiguration(new UserChatConfigurations())
                   .ApplyConfiguration(new ChatRequestConfigurations())
                   .ApplyConfiguration(new ChatMessageConfigurations())
                   .ApplyConfiguration(new ArchievedChatConfigurations())
                   .ApplyConfiguration(new MessageReactsConfigurations());
        }


        public DbSet<React> Reacts { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Block> Blocks { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostImages> PostImages { get; set; }
        public DbSet<PostView> PostViews { get; set; }
        public DbSet<SavedPosts> SavedPosts { get; set; }
        public DbSet<UserSavedPostsFolders> UserSavedPostsFolders { get; set; }
        public DbSet<PostReacts> PostReacts { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PagePost> PagePosts { get; set; }
        public DbSet<PageFollower> PageFollowers { get; set; }
        public DbSet<GroupRole> GroupRoles { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupMember> GroupMembers { get; set; }
        public DbSet<GroupAccessRequest> GroupAccessRequests { get; set; }
        public DbSet<GroupMemberRole> GroupMemberRoles { get; set; }
        public DbSet<GroupPost> GroupPosts { get; set; }
        public DbSet<SarehneMessage> SarehneMessages { get; set; }
        public DbSet<UserChat> UserChats { get; set; }
        public DbSet<ChatRequest> ChatRequests  { get; set; }
        public DbSet<ChatMessage> ChatMessages  { get; set; }
        public DbSet<ArchievedChat> ArchievedChats { get; set; }
        public DbSet<MessageReact> MessageReacts { get; set; }

    }
}
