

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Extensions
{
    public static class ConvertFromDto
    {
        public static React ConvertFromReactDto_Add(ReactDto reactDto)
        {
            return new React
            {
                Id = Guid.NewGuid().ToString(),
                ReactValue = reactDto.ReactValue
            };
        }

        public static React ConvertFromReactDto_Update(ReactDto reactDto)
        {
            if (reactDto.Id == null)
            {
                throw new NullReferenceException("React id must not be null");
            }
            return new React
            {
                Id = reactDto.Id,
                ReactValue = reactDto.ReactValue
            };
        }

        public static FriendRequest ConvertFromFriendRequestDto_Add(FriendRequestDto friendRequestDto)
        {
            return new FriendRequest
            {
                IsAccepted = false,
                UserWhoReceivedId = friendRequestDto.PersonId,
                UserWhoSendId = friendRequestDto.UserId
            };
        }

        public static FriendRequest ConvertFromFriendRequestDto_Update(FriendRequestDto friendRequestDto)
        {
            if (friendRequestDto.Id == null)
            {
                throw new NullReferenceException("Friend request id must not be null");
            }
            return new FriendRequest
            {
                Id = new Guid(friendRequestDto.Id),
                IsAccepted = friendRequestDto.IsAccepted,
                UserWhoReceivedId = friendRequestDto.PersonId,
                UserWhoSendId = friendRequestDto.UserId
            };
        }

        public static Friend ConvertFromFriendtDto_Add(FriendDto friendsDto)
        {
            return new Friend
            {
                FriendId =friendsDto.FriendId,
                UserId = friendsDto.UserId
            };
        }

        public static Follower ConvertFromFollowerDto_Add(FollowerDto followersDto)
        {
            return new Follower
            {
                UserId = followersDto.UserId,
                FollowerId = followersDto.FollowerId
            };
        }

        public static Block ConvertFromBlockDto_BlockUnBlock(BlockDto blockDto)
        {
            return new Block
            {
                BlockedUserId = blockDto.BlockedUserId,
                UserId = blockDto.UserId
            };
        }

        public static Block ConvertFromBlockDto_Update(BlockDto blockDto)
        {
            if (blockDto.Id == null)
            {
                throw new NullReferenceException("Block id must not be null");
            }
            return new Block
            {
                BlockedUserId = blockDto.BlockedUserId,
                UserId = blockDto.UserId
            };
        }


        public static Policy ConvertFromPolicyDto_Add(PolicyDto policyDto)
        {
            return new Policy
            {
                Id = Guid.NewGuid().ToString(),
                PolicyType = policyDto.PolicyType
            };
        }


        public static Policy ConvertFromPolicyDto_Update(PolicyDto policyDto)
        {
            if (policyDto.Id == null)
            {
                throw new NullReferenceException("Policy id must not be null");
            }
            return new Policy
            {
                Id = policyDto.Id.ToString(),
                PolicyType = policyDto.PolicyType
            };
        }

        public static ReactPolicy ConvertFromReactPolicyDto_Add(ReactPolicyDto reactPolicyDto)
        {
            return new ReactPolicy
            {
                Id = Guid.NewGuid().ToString(),
                PolicyId = reactPolicyDto.PolicyId
            };
        }


        public static ReactPolicy ConvertFromReactPolicyDto_Update(ReactPolicyDto reactPolicyDto)
        {
            if (reactPolicyDto.Id == null)
            {
                throw new NullReferenceException("React policy id must not be null");
            }
            return new ReactPolicy
            {
                Id = reactPolicyDto.Id,
                PolicyId = reactPolicyDto.PolicyId
            };
        }

        public static CommentPolicy ConvertFromCommentPolicyDto_Add(CommentPolicyDto commentPolicyDto)
        {
            return new CommentPolicy
            {
                Id = Guid.NewGuid().ToString(),
                PolicyId = commentPolicyDto.PolicyId
            };
        }

        public static CommentPolicy ConvertFromCommentPolicyDto_Update(CommentPolicyDto commentPolicyDto)
        {
            if (commentPolicyDto.Id == null)
            {
                throw new NullReferenceException("Comment policy id must not be null");
            }
            return new CommentPolicy
            {
                Id = commentPolicyDto.Id,
                PolicyId = commentPolicyDto.PolicyId
            };
        }

        public static AccountPolicy ConvertFromAccountPolicyDto_Add(AddAccountPolicyDto addAccountPolicyDto)
        {
            return new AccountPolicy
            {
                Id = Guid.NewGuid().ToString(),
                PolicyId = addAccountPolicyDto.PolicyIdOrName
            };
        }

        public static AccountPolicy 
            ConvertFromAccountPolicyDto_Update(UpdateAccountPolicyDto updateAccountPolicyDto)
        {
            return new AccountPolicy
            {
                Id = updateAccountPolicyDto.Id,
                PolicyId = updateAccountPolicyDto.PolicyIdOrName
            };
        }


        public static Post ConvertFromCreatePostDto_Add(CreatePostDto createPostDto, Policy policy,
            ReactPolicy reactPolicy, CommentPolicy commentPolicy)
        {
            return new Post
            {
                Id = Guid.NewGuid().ToString(),
                CommentPolicyId = commentPolicy.Id!,
                PolicyId = policy.Id!,
                PostedAt = DateTime.Now,
                Content = createPostDto.PostContent,
                ReactPolicyId = reactPolicy.Id!,
                UpdatedAt = DateTime.Now
            };
        }

        public static Post ConvertFromPostDto_Update(UpdatePostDto updatePostDto, PostDto postDto,
            Post oldPost)
        {
            return new Post
            {
                Id = postDto.PostId,
                CommentPolicyId = postDto.CommentPolicyId,
                PolicyId = postDto.PolicyId,
                PostedAt = oldPost.PostedAt,
                Content = updatePostDto.PostContent,
                ReactPolicyId = postDto.ReactPolicyId,
                UpdatedAt = DateTime.Now
            };
        }


        public static Post ConvertFromPostPolicy_Update(UpdatePostPolicyDto updatePostPolicyDto)
        {
            return new Post
            {
                Id = updatePostPolicyDto.PostId,
                PolicyId = updatePostPolicyDto.PolicyIdOrName,
            };
        }

        public static Post ConvertFromPostReactPolicy_Update(UpdatePostReactPolicyDto updatePostReactPolicy)
        {
            return new Post
            {
                Id = updatePostReactPolicy.PostId,
                PolicyId = updatePostReactPolicy.PolicyIdOrName,
            };
        }


        public static Post ConvertFromPostCommentPolicy_Update(UpdatePostCommentPolicyDto updatePostCommentPolicyDto)
        {
            return new Post
            {
                Id = updatePostCommentPolicyDto.PostId,
                PolicyId = updatePostCommentPolicyDto.PolicyIdOrName,
            };
        }


        public static FriendListPolicy ConvertFriendListPolicyDto_Update(
            UpdateFriendListPolicyDto updateFriendListPolicyDto)
        {
            return new FriendListPolicy
            {
                PolicyId = updateFriendListPolicyDto.PolicyIdOrName,
                Id = updateFriendListPolicyDto.Id
            };
        }

        public static FriendListPolicy ConvertFriendListPolicyDto_Add(
            AddFriendListPolicyDto addFriendListPolicyDto)
        {
            return new FriendListPolicy
            {
                Id = Guid.NewGuid().ToString(),
                PolicyId = addFriendListPolicyDto.PolicyIdOrName,
            };
        }

        public static AccountPostsPolicy ConvertAccountPostsPolicyDto_Add
            (AddAccountPostsPolicyDto addAccountPostsPolicyDto)
        {
            return new AccountPostsPolicy
            {
                Id = Guid.NewGuid().ToString(),
                PolicyId = addAccountPostsPolicyDto.PolicyIdOrName
            };
        }

        public static SpecialPostReacts ConvertSpecialPostReactsDto_Add(
            AddSpecialPostsReactsDto addSpecialPostsReactsDto)
        {
            return new SpecialPostReacts
            {
                Id = Guid.NewGuid().ToString(),
                ReactId = addSpecialPostsReactsDto.ReactId
            };
        }

        public static SpecialPostReacts ConvertSpecialPostReactsDto_Update(
            UpdateSpecialPostsReactsDto updateSpecialPostsReactsDto)
        {
            return new SpecialPostReacts
            {
                Id = updateSpecialPostsReactsDto.Id,
                ReactId = updateSpecialPostsReactsDto.ReactId
            };
        }

        public static SpecialCommentReacts ConvertSpecialCommentReactsDto_Add(
                AddSpecialCommentReactsDto addSpecialCommentReactsDto)
        {
            return new SpecialCommentReacts
            {
                Id = Guid.NewGuid().ToString(),
                ReactId = addSpecialCommentReactsDto.ReactId
            };
        }

        public static SpecialCommentReacts ConvertSpecialCommentReactsDto_Update(
            UpdateSpecialCommentReactsDto updateSpecialCommentReactsDto)
        {
            return new SpecialCommentReacts
            {
                Id = updateSpecialCommentReactsDto.Id,
                ReactId = updateSpecialCommentReactsDto.ReactId
            };
        }

        public static PostReacts ConvertFromPostReactsDto_Add(AddPostReactDto addPostReactDto, SiteUser user)
        {
            return new PostReacts
            {
                Id = Guid.NewGuid().ToString(),
                PostId = addPostReactDto.PostId,
                ReactId = addPostReactDto.ReactId,
                UserId = user.Id
            };
        }


        public static PostComments ConvertFromPostCommentDto_Add(AddPostCommentDto addPostCommentDto, 
            SiteUser user, string imageUniqueName)
        {
            return new PostComments
            {
                CommentImage = imageUniqueName,
                Id = Guid.NewGuid().ToString(),
                PostId = addPostCommentDto.PostId,
                UserId = user.Id,
                Comment = addPostCommentDto.Comment
            };
        }

    }
}
