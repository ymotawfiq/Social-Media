

using Microsoft.AspNetCore.Identity;

namespace SocialMedia.Data.Models.Authentication
{
    public class SiteUser : IdentityUser
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string? RefreshToken { get; set; } = null!;
        public DateTime? RefreshTokenExpiry { get; set; }
        public bool IsFriendListPrivate { get; set; }
        public string? FriendListPolicyId { get; set; } = null!;
        public string? AccountPolicyId { get; set; } = null!;
        public string? AccountPostPolicyId { get; set; } = null!;
        public string? ReactPolicyId { get; set; } = null!;
        public string? CommentPolicyId { get; set; } = null!;
        public AccountPolicy? AccountPolicy { get; set; }
        public ReactPolicy? ReactPolicy { get; set; }
        public AccountPostsPolicy? PostPolicy { get; set; }
        public CommentPolicy? CommentPolicy { get; set; }
        public FriendListPolicy? FriendListPolicy { get; set; }
        public List<FriendRequest>? FriendRequests { get; set; }
        public List<Friend>? Friends { get; set; }
        public List<Follower>? Followers { get; set; }
        public List<Block>? Blocks { get; set; }
        public List<UserPosts>? UserPosts { get; set; }
        public List<SavedPosts>? SavedPosts { get; set; }
        public List<UserSavedPostsFolders>? UserSavedPostsFolders { get; set; }
        public List<PostReacts>? PostReacts { get; set; }
        public List<PostComment>? PostComments { get; set; }
        public List<PostCommentReplay>? PostCommentReplays { get; set; }

    }
}
