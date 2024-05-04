
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;

namespace SocialMedia.Service.BlockService
{
    public interface IBlockService
    {
        Task<ApiResponse<Block>> BlockUserAsync(BlockDto blockDto);
        Task<ApiResponse<Block>> UnBlockUserAsync(BlockDto blockDto);
        Task<ApiResponse<Block>> UnBlockUserByBlockIdAsync(Guid blockId);
        Task<ApiResponse<Block>> GetBlockByIdAndUserIdAsync(Guid blockId, string userId);
        Task<ApiResponse<Block>> GetBlockByUserIdAndBlockedUserIdAsync(string userId, string blockedUserId);
        Task<ApiResponse<IEnumerable<Block>>> GetUserBlockListAsync(string userId);
        Task<ApiResponse<IEnumerable<Block>>> GetBlockListAsync();
    }
}
