

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.BlockRepository;
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
        private readonly UserManagerReturn _userManagerReturn;
        public BlockService(IBlockRepository _blockRepository, IFriendsRepository _friendsRepository,
            IFriendRequestRepository _friendRequestRepository, UserManagerReturn _userManagerReturn)
        {
            this._blockRepository = _blockRepository;
            this._friendsRepository = _friendsRepository;
            this._friendRequestRepository = _friendRequestRepository;
            this._userManagerReturn = _userManagerReturn;
        }
        public async Task<ApiResponse<Block>> BlockUserAsync(AddBlockDto addBlockDto, SiteUser user)
        {
            var blockedUser = await _userManagerReturn.GetUserByUserNameOrEmailOrIdAsync(
                    addBlockDto.UserIdOrUserNameOrEmail);
            if (blockedUser != null)
            {
                var isUserBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                        user.Id, addBlockDto.UserIdOrUserNameOrEmail);
                if (isUserBlocked == null)
                {
                    addBlockDto.UserIdOrUserNameOrEmail = blockedUser.Id;
                    var isUserFriend = await _friendsRepository.GetFriendByUserAndFriendIdAsync(
                            user.Id, addBlockDto.UserIdOrUserNameOrEmail);
                    if (isUserFriend != null)
                    {
                        return await DeleteFromFriendListAndBlockAsync(addBlockDto, user);
                    }
                    var friendRequest = await _friendRequestRepository.GetFriendRequestByUserAndPersonIdAsync(
                    user.Id, addBlockDto.UserIdOrUserNameOrEmail);
                    if (friendRequest != null)
                    {
                        return await RemoveFriendRequestAndBlockAsync(addBlockDto, friendRequest, user);
                    }
                    var newBlockedUser = await _blockRepository.BlockUserAsync(
                            ConvertFromDto.ConvertFromBlockDto_Add(addBlockDto, user));
                    return StatusCodeReturn<Block>
                    ._200_Success("User blocked successfully", newBlockedUser);
                }
                return StatusCodeReturn<Block>
                    ._403_Forbidden();
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
                    ._404_NotFound("This block");
            }
            return StatusCodeReturn<Block>
                ._200_Success("Block found successfully", block);
        }

        public async Task<ApiResponse<Block>> GetBlockByUserIdAndBlockedUserIdAsync
            (string userId, string blockedUserId)
        {
            var block = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(userId, blockedUserId);
            if (block == null)
            {
                return StatusCodeReturn<Block>
                    ._400_BadRequest("User is not in your friend list");
            }
            return StatusCodeReturn<Block>
                ._200_Success("Blocked user found successfully", block);
        }

        public async Task<ApiResponse<IEnumerable<Block>>> GetBlockListAsync()
        {
            var blockList = await _blockRepository.GetBlockListAsync();
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

        public async Task<ApiResponse<Block>> UnBlockUserAsync(UpdateBlockDto updateBlockDto, SiteUser user)
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
                    var unblockedUser = await _blockRepository.UnBlockUserAsync(
                            ConvertFromDto.ConvertFromBlockDto_Update(updateBlockDto, user));
                    return StatusCodeReturn<Block>
                            ._200_Success("User unblocked successfully", unblockedUser);
                }
                return StatusCodeReturn<Block>
                        ._403_Forbidden();
            }
            return StatusCodeReturn<Block>
                        ._404_NotFound("User you want to unblock not found");
        }

        public async Task<ApiResponse<Block>> UnBlockUserByBlockIdAsync(string blockId)
        {
            var block = await _blockRepository.GetBlockByIdAsync(blockId);
            if (block != null)
            {
                var unblock = await _blockRepository.UnBlockUserAsync(block);
                return StatusCodeReturn<Block>
                ._200_Success("Unblocked successfully");
            }
            return StatusCodeReturn<Block>
                    ._404_NotFound("Block not found");
        }

        private async Task<ApiResponse<Block>> DeleteFromFriendListAndBlockAsync(
            AddBlockDto addBlockDto, SiteUser user)
        {
            await _friendsRepository.DeleteFriendAsync(user.Id, addBlockDto.UserIdOrUserNameOrEmail);
            var blockedUser = await _blockRepository.BlockUserAsync(
            ConvertFromDto.ConvertFromBlockDto_Add(addBlockDto, user));
            return StatusCodeReturn<Block>
                ._200_Success("User blocked successfully", blockedUser);
        }
        private async Task<ApiResponse<Block>> RemoveFriendRequestAndBlockAsync(
            AddBlockDto addBlockDto, FriendRequest friendRequest, SiteUser user)
        {
            await _friendRequestRepository.DeleteFriendRequestByAsync(friendRequest.Id);
            var blockedUser = await _blockRepository.BlockUserAsync(
            ConvertFromDto.ConvertFromBlockDto_Add(addBlockDto, user));
            return StatusCodeReturn<Block>
            ._200_Success("User blocked successfully", blockedUser);
        }


    }
}
