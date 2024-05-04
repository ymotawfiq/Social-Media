

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Repository.BlockRepository;
using SocialMedia.Repository.FriendRequestRepository;
using SocialMedia.Repository.FriendsRepository;

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
                return new ApiResponse<Block>
                {
                    IsSuccess = false,
                    Message = "User already blocked",
                    StatusCode = 400
                };
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
                return new ApiResponse<Block>
                {
                    IsSuccess = false,
                    Message = "Failed to block user",
                    StatusCode = 500
                };
            }
            return new ApiResponse<Block>
            {
                IsSuccess = true,
                Message = "User blocked successfully",
                StatusCode = 200,
                ResponseObject = blockedUser
            };
        }

        public async Task<ApiResponse<Block>> GetBlockByIdAndUserIdAsync(Guid blockId, string userId)
        {
            var block = await _blockRepository.GetBlockByIdAndUserIdAsync(blockId, userId);
            if (block == null)
            {
                return new ApiResponse<Block>
                {
                    IsSuccess = false,
                    Message = "This block is not in your block list",
                    StatusCode = 404
                };
            }
            return new ApiResponse<Block>
            {
                StatusCode = 200,
                Message = "Block found successfully",
                IsSuccess = true,
                ResponseObject = block
            };
        }

        public async Task<ApiResponse<Block>> GetBlockByUserIdAndBlockedUserIdAsync
            (string userId, string blockedUserId)
        {
            var block = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(userId, blockedUserId);
            if (block == null)
            {
                return new ApiResponse<Block>
                {
                    IsSuccess = false,
                    Message = "User is not in your friend list",
                    StatusCode = 404
                };
            }
            return new ApiResponse<Block>
            {
                StatusCode = 200,
                Message = "Blocked user found successfully",
                IsSuccess = true,
                ResponseObject = block
            };
        }

        public async Task<ApiResponse<IEnumerable<Block>>> GetBlockListAsync()
        {
            var blockList = await _blockRepository.GetBlockListAsync();
            if (blockList.ToList().Count == 0)
            {
                return new ApiResponse<IEnumerable<Block>>
                {
                    IsSuccess = true,
                    Message = "Block list empty",
                    StatusCode = 200,
                    ResponseObject = blockList
                };
            }
            return new ApiResponse<IEnumerable<Block>>
            {
                IsSuccess = true,
                Message = "Block list found successfully",
                StatusCode = 200,
                ResponseObject = blockList
            };
        }

        public async Task<ApiResponse<IEnumerable<Block>>> GetUserBlockListAsync(string userId)
        {
            var userBlockList = await _blockRepository.GetUserBlockListAsync(userId);
            if (userBlockList.ToList().Count == 0)
            {
                return new ApiResponse<IEnumerable<Block>>
                {
                    IsSuccess = true,
                    Message = "No blocked users found",
                    StatusCode = 200,
                    ResponseObject = userBlockList
                };
            }
            return new ApiResponse<IEnumerable<Block>>
            {
                IsSuccess = true,
                Message = "Blocked users found successfully",
                StatusCode = 200,
                ResponseObject = userBlockList
            };
        }

        public async Task<ApiResponse<Block>> UnBlockUserAsync(BlockDto blockDto)
        {
            var blockedUser = await _blockRepository.GetBlockByUserIdAndBlockedUserIdAsync(
                blockDto.UserId, blockDto.BlockedUserId);
            if (blockedUser == null)
            {
                return new ApiResponse<Block>
                {
                    IsSuccess = false,
                    Message = "User is not in your block list",
                    StatusCode = 404
                };
            }
            var unblockedUser = await _blockRepository.UnBlockUserAsync(
                ConvertFromDto.ConvertFromBlockDto_BlockUnBlock(blockDto));
            if (unblockedUser == null)
            {
                return new ApiResponse<Block>
                {
                    IsSuccess = false,
                    Message = "Unblock failed",
                    StatusCode = 500
                };
            }
            return new ApiResponse<Block>
            {
                IsSuccess = true,
                Message = "User unblocked successfully",
                StatusCode = 200,
                ResponseObject = unblockedUser
            };
        }

        public async Task<ApiResponse<Block>> UnBlockUserByBlockIdAsync(Guid blockId)
        {
            var block = await _blockRepository.GetBlockByIdAsync(blockId);
            if (block == null)
            {
                return new ApiResponse<Block>
                {
                    IsSuccess = false,
                    Message = "Block not found",
                    StatusCode = 404
                };
            }
            var unblock = await _blockRepository.UnBlockUserAsync(block);
            if (unblock == null)
            {
                return new ApiResponse<Block>
                {
                    IsSuccess = false,
                    Message = "Can't unblock user",
                    StatusCode = 500
                };
            }
            return new ApiResponse<Block>
            {
                IsSuccess = true,
                Message = "Unblocked successfully",
                StatusCode = 200
            };
        }

        private async Task<ApiResponse<Block>> DeleteFromFriendListAndBlockAsync(BlockDto blockDto)
        {
            var deletedUser = await _friendsRepository.DeleteFriendAsync(blockDto.UserId, blockDto.BlockedUserId);
            if (deletedUser == null)
            {
                return new ApiResponse<Block>
                {
                    IsSuccess = false,
                    Message = "Can't block user",
                    StatusCode = 500
                };
            }
            var blockedUser = await _blockRepository.BlockUserAsync(
                ConvertFromDto.ConvertFromBlockDto_BlockUnBlock(blockDto));
            if (blockedUser == null)
            {
                return new ApiResponse<Block>
                {
                    IsSuccess = false,
                    Message = "Failed to block user",
                    StatusCode = 400
                };
            }
            return new ApiResponse<Block>
            {
                IsSuccess = true,
                Message = "User blocked successfully",
                StatusCode = 200,
                ResponseObject = blockedUser
            };
        }
        private async Task<ApiResponse<Block>> RemoveFriendRequestAndBlockAsync(
            BlockDto blockDto, FriendRequest friendRequest)
        {
            var deletedFriendRequest = await _friendRequestRepository.DeleteFriendRequestByAsync(
                friendRequest.Id);
            if (deletedFriendRequest == null)
            {
                return new ApiResponse<Block>
                {
                    IsSuccess = false,
                    Message = "Can't block user",
                    StatusCode = 500
                };
            }
            var blockedUser = await _blockRepository.BlockUserAsync(
                ConvertFromDto.ConvertFromBlockDto_BlockUnBlock(blockDto));
            if (blockedUser == null)
            {
                return new ApiResponse<Block>
                {
                    IsSuccess = false,
                    Message = "Can't block user",
                    StatusCode = 500
                };
            }
            return new ApiResponse<Block>
            {
                IsSuccess = true,
                Message = "User blocked successfully",
                StatusCode = 200,
                ResponseObject = blockedUser
            };
        }


    }
}
