
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.BlockService
{
    public interface IBlockService
    {
        Task<ApiResponse<Block>> BlockUserAsync(AddBlockDto addBlockDto, SiteUser user);
        Task<ApiResponse<Block>> UnBlockUserAsync(UpdateBlockDto updateBlockDto, SiteUser user);
        Task<ApiResponse<Block>> UnBlockUserByBlockIdAsync(string blockId);
        Task<ApiResponse<Block>> GetBlockByIdAndUserIdAsync(string blockId, string userId);
        Task<ApiResponse<Block>> GetBlockByUserIdAndBlockedUserIdAsync(string userId, string blockedUserId);
        Task<ApiResponse<IEnumerable<Block>>> GetUserBlockListAsync(string userId);
        Task<ApiResponse<IEnumerable<Block>>> GetBlockListAsync();
    }
}
