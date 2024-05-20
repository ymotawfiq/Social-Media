

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
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
        public BlockService(IBlockRepository _blockRepository, IFriendsRepository _friendsRepository,
            IFriendRequestRepository _friendRequestRepository)
        {
            this._blockRepository = _blockRepository;
            this._friendsRepository = _friendsRepository;
            this._friendRequestRepository = _friendRequestRepository;
        }
        public async Task<ApiResponse<Block>> BlockUserAsync(BlockDto blockDto)
        {
            var isUserBlocked = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                blockDto.UserId, blockDto.BlockedUserId);
            if (isUserBlocked != null)
            {
                return StatusCodeReturn<Block>
                    ._400_BadRequest("User already blocked");
            }
            var isBlockedUserFriend = await _friendsRepository.GetFriendByUserAndFriendIdAsync(
                blockDto.UserId, blockDto.BlockedUserId);
            if (isBlockedUserFriend != null)
            {
                return await DeleteFromFriendListAndBlockAsync(blockDto);
            }
            var friendRequest = await _friendRequestRepository.GetFriendRequestByUserAndPersonIdAsync(
                blockDto.UserId, blockDto.BlockedUserId);
            if (friendRequest != null)
            {
                return await RemoveFriendRequestAndBlockAsync(blockDto, friendRequest);
            }
            var blockedUser = await _blockRepository.BlockUserAsync(
                ConvertFromDto.ConvertFromBlockDto_BlockUnBlock(blockDto));
            if (blockedUser == null)
            {
                return StatusCodeReturn<Block>
                    ._500_ServerError("Failed to block user");
            }
            return StatusCodeReturn<Block>
                ._200_Success("User blocked successfully", blockedUser);
        }

        public async Task<ApiResponse<Block>> GetBlockByIdAndUserIdAsync(Guid blockId, string userId)
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

        public async Task<ApiResponse<Block>> UnBlockUserAsync(BlockDto blockDto)
        {
            var blockedUser = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                blockDto.UserId, blockDto.BlockedUserId);
            if (blockedUser == null)
            {
                return StatusCodeReturn<Block>
                    ._400_BadRequest("User is not in your block list");
            }
            var unblockedUser = await _blockRepository.UnBlockUserAsync(
                ConvertFromDto.ConvertFromBlockDto_BlockUnBlock(blockDto));
            if (unblockedUser == null)
            {
                return StatusCodeReturn<Block>
                    ._500_ServerError("Unblock failed");
            }
            return StatusCodeReturn<Block>
                ._200_Success("User unblocked successfully", unblockedUser);
        }

        public async Task<ApiResponse<Block>> UnBlockUserByBlockIdAsync(Guid blockId)
        {
            var block = await _blockRepository.GetBlockByIdAsync(blockId);
            if (block == null)
            {
                return StatusCodeReturn<Block>
                    ._404_NotFound("Block not found");
            }
            var unblock = await _blockRepository.UnBlockUserAsync(block);
            if (unblock == null)
            {
                return StatusCodeReturn<Block>
                    ._500_ServerError("Can't unblock user");
            }
            return StatusCodeReturn<Block>
                ._200_Success("Unblocked successfully");
        }

        private async Task<ApiResponse<Block>> DeleteFromFriendListAndBlockAsync(BlockDto blockDto)
        {
            var deletedUser = await _friendsRepository.DeleteFriendAsync(blockDto.UserId, blockDto.BlockedUserId);
            if (deletedUser == null)
            {
                return StatusCodeReturn<Block>
                    ._500_ServerError("Can't block user");
            }
            var blockedUser = await _blockRepository.BlockUserAsync(
                ConvertFromDto.ConvertFromBlockDto_BlockUnBlock(blockDto));
            if (blockedUser == null)
            {
                return StatusCodeReturn<Block>
                    ._400_BadRequest("Failed to block user");
            }
            return StatusCodeReturn<Block>
                ._200_Success("User blocked successfully", blockedUser);
        }
        private async Task<ApiResponse<Block>> RemoveFriendRequestAndBlockAsync(
            BlockDto blockDto, FriendRequest friendRequest)
        {
            var deletedFriendRequest = await _friendRequestRepository.DeleteFriendRequestByAsync(
                friendRequest.Id);
            if (deletedFriendRequest == null)
            {
                return StatusCodeReturn<Block>
                    ._500_ServerError("Can't block user");
            }
            var blockedUser = await _blockRepository.BlockUserAsync(
                ConvertFromDto.ConvertFromBlockDto_BlockUnBlock(blockDto));
            if (blockedUser == null)
            {
                return StatusCodeReturn<Block>
                    ._500_ServerError("Can't block user");
            }
            return StatusCodeReturn<Block>
                ._200_Success("User blocked successfully", blockedUser);
        }


    }
}
