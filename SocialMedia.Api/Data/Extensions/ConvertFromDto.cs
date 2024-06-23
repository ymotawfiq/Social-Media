

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel.ResponseObject;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Data.Extensions
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

        public static Block ConvertFromBlockDto_Update(UnBlockDto updateBlockDto, SiteUser user)
        {
            return new Block
            {
                BlockedUserId = updateBlockDto.UserIdOrUserNameOrEmail,
                UserId = user.Id,
                Id = updateBlockDto.UserIdOrUserNameOrEmail
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



        public static Page ConvertFromPageDto_Add(AddPageDto addPageDto, SiteUser user)
        {
            return new Page
            {
                Description = addPageDto.Description,
                Name = addPageDto.Name,
                Id = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                CreatorId = user.Id
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


        public static Role ConvertFromRoleDto_Add(AddRoleDto addGroupRoleDto)
        {
            return new Role
            {
                Id = Guid.NewGuid().ToString(),
                RoleName = addGroupRoleDto.RoleName
            };
        }

        public static Role ConvertFromRoleDto_Update(UpdateRoleDto updateGroupRoleDto)
        {
            return new Role
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
                SentAt = DateTime.Now.Date
            };
        }


        public static SarehneMessage ConvertFromUpdateSarehneMessagePolicyDto(
            UpdateSarehneMessagePolicyDto updateSarehneMessagePolicyDto, SarehneMessage sarehneMessage)
        {
            sarehneMessage.MessagePolicyId = updateSarehneMessagePolicyDto.PolicyIdOrName;
            return sarehneMessage;
        }


        public static Chat ConvertFromPrivateChatDto_Add(AddChatDto addChatDto, Policy policy)
        {
            return new Chat
            {

                Id = Guid.NewGuid().ToString(),
                Name = addChatDto.Name!,
                CreatorId = null!,
                Description = addChatDto.Description!,
                PolicyId = policy.Id
                
            };
        }

        public static Chat ConvertFromNonPrivateChatDto_Add(
            AddChatDto addChatDto, Policy policy, SiteUser user)
        {
            return new Chat
            {
                Id = Guid.NewGuid().ToString(),
                Name = addChatDto.Name!,
                CreatorId = user.Id,
                Description = addChatDto.Description!,
                PolicyId = policy.Id
            };
        }

        public static ChatMessage ConvertAddChatMessageDto_Add(AddChatMessageDto addChatMessageDto,
            string photoUniqueName, SiteUser user)
        {
            return new ChatMessage
            {
                Id = Guid.NewGuid().ToString(),
                ChatId = addChatMessageDto.ChatId,
                Message = addChatMessageDto.Message,
                Photo = photoUniqueName,
                SenderId = user.Id,
                SentAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }

        public static MessageReact ConvertAddMessageReactDto_Add(AddMessageReactDto addMessageReactDto,
                SiteUser user)
        {
            return new MessageReact
            {
                Id = Guid.NewGuid().ToString(),
                MessageId = addMessageReactDto.MessageId,
                ReactId = addMessageReactDto.ReactId,
                ReactedUserId = user.Id,
                SentAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
        }


    }
}
