

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
            AddFriendRequestDto addFriendRequestDto, SiteUser userWhoSent)
        {
            return new FriendRequest
            {
                Id = Guid.NewGuid().ToString(),
                IsAccepted = false,
                UserWhoReceivedId = addFriendRequestDto.PersonIdOrUserNameOrEmail,
                UserWhoSendId = userWhoSent.Id
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


        public static Post ConvertFromCreatePostDto_Add(AddPostDto createPostDto, Policy postsPolicy,
            Policy reactPolicy, Policy commentPolicy, SiteUser user)
        {
            return new Post
            {
                Id = Guid.NewGuid().ToString(),
                CommentPolicyId = commentPolicy.Id!,
                PostPolicyId = postsPolicy.Id!,
                PostedAt = DateTime.Now,
                Content = createPostDto.PostContent,
                ReactPolicyId = reactPolicy.Id!,
                UpdatedAt = DateTime.Now,
                UserId = user.Id
            };
        }



        public static PostReacts ConvertFromPostReactsDto_Add(AddPostReactDto addPostReactDto, SiteUser user)
        {
            return new PostReacts
            {
                Id = Guid.NewGuid().ToString(),
                PostId = addPostReactDto.PostId,
                PostReactId = addPostReactDto.ReactId,
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

        public static Group ConvertFromGroupDto_Add(AddGroupDto addGroupDto, SiteUser user)
        {
            return new Group
            {
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                Name = addGroupDto.Name,
                Description = addGroupDto.Description,
                GroupPolicyId = addGroupDto.GroupPolicyIdOrName,
                CreatedUserId = user.Id
            };
        }

        public static Group ConvertFromGroupDto_Update(UpdateGroupDto updateGroupDto, SiteUser user,
            Group group)
        {
            return new Group
            {
                Id = updateGroupDto.Id,
                Name = updateGroupDto.Name,
                Description = updateGroupDto.Description,
                CreatedUserId = user.Id,
                GroupPolicyId = group.GroupPolicyId,
                CreatedAt = group.CreatedAt
            };
        }

        public static Group ConvertFromGroupDto_Update(
            UpdateExistGroupPolicyDto updateExistGroupPolicyDto, Group group)
        {
            return new Group
            {
                Id = updateExistGroupPolicyDto.GroupId,
                GroupPolicyId = updateExistGroupPolicyDto.GroupPolicyIdOrName,
                CreatedUserId = group.CreatedUserId,
                Name = group.Name,
                Description = group.Description,
                CreatedAt = group.CreatedAt
            };
        }


        public static SarehneMessage ConvertFromSendSarehneMessageDto(
            SendSarahaMessageDto sendSarahaMessageDto, SiteUser user, SiteUser receiver, 
                Policy policy)
        {
            if (sendSarahaMessageDto.ShareYourName && user != null)
            {
                return new SarehneMessage
                {
                    Id = Guid.NewGuid().ToString(),
                    Message = sendSarahaMessageDto.Message,
                    ReceiverId = receiver.Id,
                    SenderName = $"{user.FirstName} {user.LastName}",
                    MessagePolicyId = policy.Id,
                    SentAt = DateTime.Now
                };
            }
            return new SarehneMessage
            {
                Id = Guid.NewGuid().ToString(),
                Message = sendSarahaMessageDto.Message,
                ReceiverId = receiver.Id,
                SenderName = "Anonymous",
                MessagePolicyId = policy.Id,
                SentAt = DateTime.Now
            };
        }


        public static SarehneMessage ConvertFromUpdateSarehneMessagePolicyDto(
            UpdateSarehneMessagePolicyDto updateSarehneMessagePolicyDto, SarehneMessage sarehneMessage)
        {
            sarehneMessage.MessagePolicyId = updateSarehneMessagePolicyDto.PolicyIdOrName;
            return sarehneMessage;
        }


    }
}
