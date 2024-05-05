

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;

namespace SocialMedia.Data.Extensions
{
    public static class ConvertFromDto
    {
        public static React ConvertFromReactDto_Add(ReactDto reactDto)
        {
            return new React
            {
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
                Id = new Guid(reactDto.Id),
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



    }
}
