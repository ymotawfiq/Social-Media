

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.BlockRepository;
using SocialMedia.Repository.FollowerRepository;
using SocialMedia.Repository.FriendRequestRepository;
using SocialMedia.Repository.FriendsRepository;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.BlockService
{
    public class BlockService : IBlockService
    {
        private readonly IBlockRepository _blockRepository;
        private readonly IFriendsRepository _friendsRepository;
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IFollowerRepository _followerRepository;
        private readonly UserManagerReturn _userManagerReturn;
        public BlockService(IBlockRepository _blockRepository, IFriendsRepository _friendsRepository,
            IFriendRequestRepository _friendRequestRepository, UserManagerReturn _userManagerReturn,
            IFollowerRepository _followerRepository)
        {
            this._blockRepository = _blockRepository;
            this._friendsRepository = _friendsRepository;
            this._friendRequestRepository = _friendRequestRepository;
            this._userManagerReturn = _userManagerReturn;
            this._followerRepository = _followerRepository;
        }
        public async Task<ApiResponse<Block>> BlockUserAsync(AddBlockDto addBlockDto, SiteUser user)
        {
            var blockedUser = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                    addBlockDto.UserIdOrUserNameOrEmail);
            if (blockedUser != null)
            {
                addBlockDto.UserIdOrUserNameOrEmail = blockedUser.Id;
                var isUserBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                        user.Id, addBlockDto.UserIdOrUserNameOrEmail);
                if (isUserBlocked == null)
                {
                    await UnfollowAsync(addBlockDto, user);
                    await DeleteFriendRequestAsync(addBlockDto, user);
                    await DeleteFromFriendListAsync(addBlockDto, user);
                    var newBlockedUser = await _blockRepository.AddAsync(
                            ConvertFromDto.ConvertFromBlockDto_Add(addBlockDto, user));
                    return StatusCodeReturn<Block>
                    ._200_Success("User blocked successfully", newBlockedUser);
                }
                return StatusCodeReturn<Block>
                    ._403_Forbidden("User already blocked");
            }
            return StatusCodeReturn<Block>
                        ._404_NotFound("User you want to block not found");
        }

        public async Task<ApiResponse<Block>> GetBlockByIdAndUserIdAsync(string blockId, string userId)
        {
            var block = await _blockRepository.GetBlockByIdAndUserIdAsync(blockId, userId);
            if (block == null)
            {
                return StatusCodeReturn<Block>
                    ._404_NotFound("Block not found");
            }
            return StatusCodeReturn<Block>
                ._200_Success("Block found successfully", block);
        }

        public async Task<ApiResponse<Block>> GetBlockByUserIdAndBlockedUserIdAsync(string userId,
            string blockedUserId)
        {
            var block = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(userId, blockedUserId);
            if (block == null)
            {
                return StatusCodeReturn<Block>
                    ._404_NotFound("User is not in your block list");
            }
            return StatusCodeReturn<Block>
                ._200_Success("Blocked user found successfully", block);
        }

        public async Task<ApiResponse<IEnumerable<Block>>> GetBlockListAsync()
        {
            var blockList = await _blockRepository.GetAllAsync();
            if (blockList.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<Block>>
                    ._200_Success("Block list empty", blockList);
            }
            return StatusCodeReturn<IEnumerable<Block>>
                    ._200_Success("Block list found successfully", blockList);
        }

        public async Task<ApiResponse<IEnumerable<Block>>> GetUserBlockListAsync(string userId)
        {
            var userBlockList = await _blockRepository.GetUserBlockListAsync(userId);
            if (userBlockList.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<Block>>
                    ._200_Success("No blocked users found", userBlockList);
            }
            return StatusCodeReturn<IEnumerable<Block>>
                    ._200_Success("Blocked users found successfully", userBlockList);
        }

        public async Task<ApiResponse<Block>> UnBlockUserAsync(UnBlockDto updateBlockDto, SiteUser user)
        {
            var blockedUser = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                updateBlockDto.UserIdOrUserNameOrEmail);
            if (blockedUser != null)
            {
                updateBlockDto.UserIdOrUserNameOrEmail = blockedUser.Id;
                var existBlockedUser = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                user.Id, updateBlockDto.UserIdOrUserNameOrEmail);
                if (existBlockedUser != null)
                {
                    var unblockedUser = await _blockRepository.UpdateAsync(
                            ConvertFromDto.ConvertFromBlockDto_Update(updateBlockDto, user));
                    return StatusCodeReturn<Block>
                            ._200_Success("Block removed successfully", unblockedUser);
                }
                return StatusCodeReturn<Block>
                        ._404_NotFound("User not in your block list");
            }
            return StatusCodeReturn<Block>
                        ._404_NotFound("User you want to unblock not found");
        }

        public async Task<ApiResponse<Block>> UnBlockUserByBlockIdAsync(string blockId, SiteUser user)
        {
            var block = await _blockRepository.GetByIdAsync(blockId);
            if (block != null)
            {
                if(block.UserId == user.Id)
                {
                    var unblock = await _blockRepository.UpdateAsync(block);
                    return StatusCodeReturn<Block>
                    ._200_Success("Unblocked successfully", unblock);
                }
                return StatusCodeReturn<Block>._403_Forbidden();
            }
            return StatusCodeReturn<Block>
                    ._404_NotFound("Block not found");
        }

        private async Task<ApiResponse<Block>> DeleteFromFriendListAsync(
            AddBlockDto addBlockDto, SiteUser user)
        {
            var isUserFriend = await _friendsRepository.GetByUserAndFriendIdAsync(
                            user.Id, addBlockDto.UserIdOrUserNameOrEmail);
            if (isUserFriend != null)
            {
                await _friendsRepository.DeleteFriendAsync(user.Id, addBlockDto.UserIdOrUserNameOrEmail);
                return StatusCodeReturn<Block>._200_Success("Success");
            }
            return StatusCodeReturn<Block>._404_NotFound("You are not friends");
        }
        private async Task<ApiResponse<Block>> DeleteFriendRequestAsync(
            AddBlockDto addBlockDto, SiteUser user)
        {
            var friendRequest = await _friendRequestRepository.GetByUserAndPersonIdAsync(
                    user.Id, addBlockDto.UserIdOrUserNameOrEmail);
            if (friendRequest != null)
            {
                await _friendRequestRepository.DeleteByIdAsync(friendRequest.Id);
                return StatusCodeReturn<Block>._200_Success("Success");
            }
            return StatusCodeReturn<Block>._404_NotFound("Friend request not found");
        }

        private async Task<ApiResponse<Block>> UnfollowAsync(AddBlockDto addBlockDto, SiteUser user)
        {
            var isFollowingYou = await _followerRepository.GetByUserIdAndFollowerIdAsync(
                    user.Id, addBlockDto.UserIdOrUserNameOrEmail);
            var areYouFollowingHim = await _followerRepository.GetByUserIdAndFollowerIdAsync(
                addBlockDto.UserIdOrUserNameOrEmail, user.Id);
            if (isFollowingYou != null)
            {
                await _followerRepository.UpdateAsync(user.Id, addBlockDto.UserIdOrUserNameOrEmail);
            }
            if (areYouFollowingHim != null)
            {
                await _followerRepository.UpdateAsync(addBlockDto.UserIdOrUserNameOrEmail, user.Id);
            }
            return StatusCodeReturn<Block>._200_Success("Success");
        }


    }
}
