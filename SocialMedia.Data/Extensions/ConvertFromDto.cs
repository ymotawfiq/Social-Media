

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

    }
}
