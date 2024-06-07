

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data.Extensions
{
    public static class ConvertFromDto
    {
        public static React ConvertFromReactDto_Add(AddReactDto addReactDto)
        {
            return new React
            {
                Id = Guid.NewGuid().ToString(),
                ReactValue = addReactDto.ReactValue
            };
        }

        public static React ConvertFromReactDto_Update(UpdateReactDto updateReactDto)
        {
            return new React
            {
                Id = updateReactDto.Id,
                ReactValue = updateReactDto.ReactValue
            };
        }

        public static FriendRequest ConvertFromFriendRequestDto_Add(
            AddFriendRequestDto addFriendRequestDto, SiteUser userWhoReceived)
        {
            return new FriendRequest
            {
                Id = Guid.NewGuid().ToString(),
                IsAccepted = false,
                UserWhoReceivedId = userWhoReceived.Id,
                UserWhoSendId = addFriendRequestDto.PersonIdOrUserNameOrEmail
            };
        }

        public static FriendRequest ConvertFromFriendRequestDto_Update(
            UpdateFriendRequestDto updateFriendRequestDto, SiteUser userWhoSent, SiteUser userWhoReceived)
        {
            return new FriendRequest
            {
                Id = updateFriendRequestDto.FriendRequestId,
                IsAccepted = updateFriendRequestDto.IsAccepted,
                UserWhoReceivedId = userWhoReceived.Id,
                UserWhoSendId = userWhoSent.Id
            };
        }

        public static Friend ConvertFromFriendtDto_Add(AddFriendDto addFriendDto)
        {
            return new Friend
            {
                Id = Guid.NewGuid().ToString(),
                FriendId =addFriendDto.FriendId,
                UserId = addFriendDto.UserId
            };
        }

        public static Follower ConvertFromFollowerDto_Add(FollowDto followDto, SiteUser user)
        {
            return new Follower
            {
                UserId = followDto.UserIdOrUserNameOrEmail,
                FollowerId = user.Id,
                Id = Guid.NewGuid().ToString()
            };
        }

        public static Block ConvertFromBlockDto_Add(AddBlockDto addBlockDto, SiteUser user)
        {
            return new Block
            {
                Id = Guid.NewGuid().ToString(),
                BlockedUserId = addBlockDto.UserIdOrUserNameOrEmail,
                UserId = user.Id
            };
        }

        public static Block ConvertFromBlockDto_Update(UpdateBlockDto updateBlockDto, SiteUser user)
        {
            return new Block
            {
                BlockedUserId = updateBlockDto.UserIdOrUserNameOrEmail,
                UserId = user.Id,
                Id = updateBlockDto.Id
            };
        }


        public static Policy ConvertFromPolicyDto_Add(AddPolicyDto addPolicyDto)
        {
            return new Policy
            {
                Id = Guid.NewGuid().ToString(),
                PolicyType = addPolicyDto.PolicyType
            };
        }


        public static Policy ConvertFromPolicyDto_Update(UpdatePolicyDto updatePolicyDto)
        {
            return new Policy
            {
                Id = updatePolicyDto.Id,
                PolicyType = updatePolicyDto.PolicyType
            };
        }

        public static ReactPolicy ConvertFromReactPolicyDto_Add(AddReactPolicyDto addReactPolicyDto)
        {
            return new ReactPolicy
            {
                Id = Guid.NewGuid().ToString(),
                PolicyId = addReactPolicyDto.PolicyId
            };
        }


        public static ReactPolicy ConvertFromReactPolicyDto_Update(UpdateReactPolicyDto updateReactPolicyDto)
        {
            return new ReactPolicy
            {
                Id = updateReactPolicyDto.Id,
                PolicyId = updateReactPolicyDto.PolicyId
            };
        }

        public static CommentPolicy ConvertFromCommentPolicyDto_Add(AddCommentPolicyDto addCommentPolicyDto)
        {
            return new CommentPolicy
            {
                Id = Guid.NewGuid().ToString(),
                PolicyId = addCommentPolicyDto.PolicyIdOrName
            };
        }

        public static CommentPolicy ConvertFromCommentPolicyDto_Update(
            UpdateCommentPolicyDto updateCommentPolicyDto)
        {
            return new CommentPolicy
            {
                Id = updateCommentPolicyDto.Id,
                PolicyId = updateCommentPolicyDto.PolicyIdOrName
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

        public static AccountPolicy ConvertFromAccountPolicyDto_Update(
            UpdateAccountPolicyDto updateAccountPolicyDto)
        {
            return new AccountPolicy
            {
                Id = updateAccountPolicyDto.Id,
                PolicyId = updateAccountPolicyDto.PolicyIdOrName
            };
        }


        public static Post ConvertFromCreatePostDto_Add(AddPostDto createPostDto, Policy policy,
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


        public static Post ConvertFromPostCommentPolicy_Update(
            UpdatePostCommentPolicyDto updatePostCommentPolicyDto)
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

        public static AccountPostsPolicy ConvertAccountPostsPolicyDto_Add(
            AddAccountPostsPolicyDto addAccountPostsPolicyDto)
        {
            return new AccountPostsPolicy
            {
                Id = Guid.NewGuid().ToString(),
                PolicyId = addAccountPostsPolicyDto.PolicyIdOrName
            };
        }

        public static AccountPostsPolicy ConvertAccountPostsPolicyDto_Update(
            UpdateAccountPostsPolicyDto updateAccountPostsPolicyDto)
        {
            return new AccountPostsPolicy
            {
                Id = updateAccountPostsPolicyDto.Id,
                PolicyId = updateAccountPostsPolicyDto.PolicyIdOrName
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
                SpecialPostReactId = addPostReactDto.ReactId,
                UserId = user.Id
            };
        }


        public static PostComment ConvertFromPostCommentDto_Add(AddPostCommentDto addPostCommentDto, 
            SiteUser user, string imageUniqueName)
        {
            return new PostComment
            {
                CommentImage = imageUniqueName,
                Id = Guid.NewGuid().ToString(),
                PostId = addPostCommentDto.PostId,
                UserId = user.Id,
                Comment = addPostCommentDto.Comment
            };
        }


        public static PostCommentReplay ConvertFromPostCommentReplayDto_Add(
            AddPostCommentReplayDto addPostCommentReplay, SiteUser user, string imageUniqueName)
        {
            return new PostCommentReplay
            {
                Id = Guid.NewGuid().ToString(),
                PostCommentId = addPostCommentReplay.PostCommentId,
                Replay = addPostCommentReplay.Replay,
                UserId = user.Id,
                ReplayImage = imageUniqueName
            };
        }

        public static PostCommentReplay ConvertFromCommentReplayToReplayDto_Add(
            AddReplayToReplayCommentDto replayToReplayCommentDto, SiteUser user,
            string imageUniqueName, string commentId)
        {
            return new PostCommentReplay
            {
                Id = Guid.NewGuid().ToString(),
                PostCommentId = commentId,
                Replay = replayToReplayCommentDto.Replay,
                UserId = user.Id,
                ReplayImage = imageUniqueName,
                PostCommentReplayId = replayToReplayCommentDto.CommentReplayId
            };
        }


        public static Page ConvertFromPageDto_Add(AddPageDto addPageDto)
        {
            return new Page
            {
                Description = addPageDto.Description,
                Name = addPageDto.Name,
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now
            };
        }

        public static Page ConvertFromPageDto_Update(UpdatePageDto updatePageDto)
        {
            return new Page
            {
                Description = updatePageDto.Description,
                Name = updatePageDto.Name,
                Id = updatePageDto.Id
            };
        }

        public static AddPostDto ConvertFromAddPagePostDtoToAddPostDto(AddPagePostDto addPagePostDto)
        {
            return new AddPostDto
            {
                Images = addPagePostDto.Images,
                PostContent = addPagePostDto.PostContent
            };
        }



        public static PageFollower ConvertFromFollowPageDto_Add(
            FollowPageDto followPageDto, SiteUser user)
        {
            return new PageFollower
            {
                FollowerId = user.Id,
                PageId = followPageDto.PageId,
                Id = Guid.NewGuid().ToString()
            };
        }

        public static PageFollower ConvertFromUnFollowPageDto_Delete(
                    UnFollowPageDto unFollowPageDto, SiteUser user)
        {
            return new PageFollower
            {
                FollowerId = user.Id,
                PageId = unFollowPageDto.PageId,
                Id = Guid.NewGuid().ToString()
            };
        }

        public static PageFollower ConvertFromFollowPageUserDto_Add(FollowPageUserDto followPageUserDto)
        {
            return new PageFollower
            {
                FollowerId = followPageUserDto.UserIdOrUserNameOrEmail,
                PageId = followPageUserDto.PageId,
                Id = Guid.NewGuid().ToString()
            };
        }


        public static GroupPolicy ConvertFromGroupPolicyDto_Add(AddGroupPolicyDto addGroupPolicyDto)
        {
            return new GroupPolicy
            {
                Id = Guid.NewGuid().ToString(),
                PolicyId = addGroupPolicyDto.PolicyIdOrName
            };
        }

        public static GroupPolicy ConvertFromGroupPolicyDto_Update(UpdateGroupPolicyDto updateGroupPolicyDto)
        {
            return new GroupPolicy
            {
                Id = updateGroupPolicyDto.Id,
                PolicyId = updateGroupPolicyDto.PolicyIdOrName
            };
        }


        public static GroupRole ConvertFromGroupRoleDto_Add(AddGroupRoleDto addGroupRoleDto)
        {
            return new GroupRole
            {
                Id = Guid.NewGuid().ToString(),
                RoleName = addGroupRoleDto.RoleName
            };
        }

        public static GroupRole ConvertFromGroupRoleDto_Update(UpdateGroupRoleDto updateGroupRoleDto)
        {
            return new GroupRole
            {
                Id = updateGroupRoleDto.Id,
                RoleName = updateGroupRoleDto.RoleName
            };
        }

    }
}
