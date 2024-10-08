

using Microsoft.AspNetCore.Identity;
using SocialMedia.Api.Data.Models.ApiResponseModel;

namespace SocialMedia.Api.Data.Models.Authentication
{
    public class SiteUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string? RefreshToken { get; set; } = null!;
        public DateTime? RefreshTokenExpiry { get; set; }
        public string FriendListPolicyId { get; set; } = null!;
        public string AccountPolicyId { get; set; } = null!;
        public string AccountPostPolicyId { get; set; } = null!;
        public string ReactPolicyId { get; set; } = null!;
        public string CommentPolicyId { get; set; } = null!;
        public List<Post>? Posts { get; set; }
        public Policy? AccountPolicy { get; set; }
        public Policy? ReactPolicy { get; set; }
        public Policy? PostPolicy { get; set; }
        public Policy? CommentPolicy { get; set; }
        public Policy? FriendListPolicy { get; set; }
        public List<FriendRequest>? FriendRequests { get; set; }
        public List<Friend>? Friends { get; set; }
        public List<Follower>? Followers { get; set; }
        public List<Block>? Blocks { get; set; }
        public List<SavedPosts>? SavedPosts { get; set; }
        public List<UserSavedPostsFolders>? UserSavedPostsFolders { get; set; }
        public List<PostReacts>? PostReacts { get; set; }
        public List<PostComment>? PostComments { get; set; }
        public List<PageFollower>? PageFollowers { get; set; }
        public LinkedList<Group>? Groups { get; set; }
        public List<GroupMember>? GroupMembers { get; set; }
        public List<GroupAccessRequest>? GroupAccessRequests { get; set; }
        public List<GroupPost>? GroupPosts { get; set; }
        public List<SarehneMessage>? SarehneMessages { get; set; }
        public List<Page>? Pages { get; set; }
        public List<Chat>? Chats { get; set; }
        public List<ChatMember>? ChatMembers1 { get; set; }
        public List<PrivateChat>? PrivateChats1 { get; set; }
        public List<PrivateChat>? PrivateChats2 { get; set; }
        public List<ChatMessage>? ChatMessages { get; set; }
        public List<MessageReact>? MessageReacts { get; set; }
        public List<ArchievedChat>? ArchievedChats { get; set; }

    }
}
