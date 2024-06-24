

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Extensions;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.BlockRepository;
using SocialMedia.Api.Repository.FollowerRepository;
using SocialMedia.Api.Service.GenericReturn;

namespace SocialMedia.Api.Service.FollowerService
{
    public class FollowerService : IFollowerService
    {
        private readonly IFollowerRepository _followerRepository;
        private readonly UserManagerReturn _userManagerReturn;
        private readonly IBlockRepository _blockRepository;
        public FollowerService(IFollowerRepository _followerRepository,
            IBlockRepository _blockRepository, UserManagerReturn _userManagerReturn)
        {
            this._followerRepository = _followerRepository;
            this._userManagerReturn = _userManagerReturn;
            this._blockRepository = _blockRepository;
        }
        public async Task<ApiResponse<Follower>> FollowAsync(FollowDto followDto, SiteUser user)
        {
            var followedPerson = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                followDto.UserIdOrUserNameOrEmail);
            if (followedPerson != null)
            {
                followDto.UserIdOrUserNameOrEmail = followedPerson.Id;
                if(followedPerson.Id != user.Id)
                {
                    var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                        user.Id, followedPerson.Id);
                    if (isBlocked == null)
                    {
                        var isFollowing = await _followerRepository.GetByUserIdAndFollowerIdAsync(
                                user.Id, followedPerson.Id);
                        if (isFollowing == null)
                        {
                            var follow = await _followerRepository.AddAsync(
                                ConvertFromDto.ConvertFromFollowerDto_Add(followDto, user));
                            follow.User = _userManagerReturn.SetUserToReturn(user);
                            return StatusCodeReturn<Follower>
                                ._200_Success("Followed successfully", follow);
                        }
                        return StatusCodeReturn<Follower>
                            ._403_Forbidden("You already following this person");
                    }
                    return StatusCodeReturn<Follower>
                            ._403_Forbidden();
                }
                return StatusCodeReturn<Follower>
                            ._403_Forbidden("You can't follow yourself");
            }
            return StatusCodeReturn<Follower>
                    ._404_NotFound("User you want to follow not found");

        }

        public async Task<ApiResponse<Follower>> FollowAsync(SiteUser user, SiteUser follower)
        {
            var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                    user.Id, follower.Id);
            if (isBlocked == null)
            {
                var isFollowing = await _followerRepository.GetByUserIdAndFollowerIdAsync(
                            user.Id, follower.Id);
                if (isFollowing == null)
                {
                    var follow = await _followerRepository.AddAsync(new Follower
                    {
                        Id = Guid.NewGuid().ToString(),
                        FollowerId = follower.Id,
                        UserId = user.Id
                    });
                    follow.User = _userManagerReturn.SetUserToReturn(user);
                    return StatusCodeReturn<Follower>
                        ._200_Success("Followed successfully", follow);
                }
                return StatusCodeReturn<Follower>
                        ._403_Forbidden("You already following this person");
            }
            return StatusCodeReturn<Follower>
                        ._403_Forbidden();
        }

        public async Task<ApiResponse<IEnumerable<Follower>>> GetAllFollowers(string userIdOrNameOrEmail)
        {
            var user = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(userIdOrNameOrEmail);
            if (user != null)
            {
                var followers = await _followerRepository.GetAllAsync(user.Id);
                if (followers.ToList().Count == 0)
                {
                    return StatusCodeReturn<IEnumerable<Follower>>
                        ._200_Success("No followers found", followers);
                }
                return StatusCodeReturn<IEnumerable<Follower>>
                        ._200_Success("Followers found successfully", followers);
            }
            return StatusCodeReturn<IEnumerable<Follower>>
                ._404_NotFound("User not found");
        }

        public async Task<ApiResponse<IEnumerable<Follower>>> GetAllFollowers(string userIdOrNameOrEmail,
            SiteUser user)
        {
            var user1 = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(userIdOrNameOrEmail);
            if (user1 != null)
            {
                var followers = await _followerRepository.GetAllAsync(user1.Id);
                if (user1.Id != user.Id)
                {
                    var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                    user.Id, user1.Id);
                    if (isBlocked != null)
                    {
                        return StatusCodeReturn<IEnumerable<Follower>>
                            ._403_Forbidden();
                    }
                }
                if (followers.ToList().Count == 0)
                {
                    return StatusCodeReturn<IEnumerable<Follower>>
                        ._200_Success("No followers found", followers);
                }
                return StatusCodeReturn<IEnumerable<Follower>>
                        ._200_Success("Followers found successfully", followers);
            }
            return StatusCodeReturn<IEnumerable<Follower>>
                ._404_NotFound("User not found");
        }

        public async Task<ApiResponse<Follower>> UnfollowAsync(UnFollowDto unFollowDto, SiteUser follower)
        {
            var followedPerson = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                unFollowDto.UserIdOrUserNameOrEmail);
            if (followedPerson != null)
            {
                var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                    follower.Id, followedPerson.Id);
                if (isBlocked == null)
                {
                    var isFollowed = await _followerRepository.GetByUserIdAndFollowerIdAsync(
                        followedPerson.Id, follower.Id);
                    if (isFollowed != null)
                    {
                        var unfollow = await _followerRepository.UpdateAsync(followedPerson.Id, follower.Id);
                        unfollow.User = _userManagerReturn.SetUserToReturn(follower);
                        return StatusCodeReturn<Follower>
                            ._200_Success("Unfollowed successfully", unfollow);
                    }
                    return StatusCodeReturn<Follower>
                        ._403_Forbidden("You are not following this person");
                }
                return StatusCodeReturn<Follower>
                        ._403_Forbidden();
            }

            return StatusCodeReturn<Follower>
                         ._404_NotFound("User you want to unfollow not found");

        }

        public async Task<ApiResponse<Follower>> UnfollowAsync(string followId, string followerId)
        {
            var follow = await _followerRepository.GetByIdAsync(followId);
            if (follow != null)
            {
                var isBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                    followerId, follow.UserId);
                if (isBlocked == null)
                {
                    if (follow.FollowerId == followerId)
                    {
                        var unfollow = await _followerRepository.UpdateAsync(follow);
                        unfollow.User = _userManagerReturn.SetUserToReturn(await _userManagerReturn
                            .GetUserByUserNameOrEmailOrIdAsync(followerId));
                        return StatusCodeReturn<Follower>
                            ._200_Success("Unfollowed successfully", unfollow);
                    }
                    return StatusCodeReturn<Follower>
                            ._403_Forbidden("You are not following this person");
                }
                return StatusCodeReturn<Follower>
                        ._403_Forbidden();
            }
            return StatusCodeReturn<Follower>
                         ._404_NotFound("Follow not found");
        }
    }
}
