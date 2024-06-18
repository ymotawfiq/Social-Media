
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Service.BlockService
{
    public interface IBlockService
    {
        Task<ApiResponse<Block>> BlockUserAsync(AddBlockDto addBlockDto, SiteUser user);
        Task<ApiResponse<Block>> UnBlockUserAsync(UnBlockDto updateBlockDto, SiteUser user);
        Task<ApiResponse<Block>> UnBlockUserByBlockIdAsync(string blockId, SiteUser user);
        Task<ApiResponse<Block>> GetBlockByIdAndUserIdAsync(string blockId, string userId);
        Task<ApiResponse<Block>> GetBlockByUserIdAndBlockedUserIdAsync(string userId, string blockedUserId);
        Task<ApiResponse<IEnumerable<Block>>> GetUserBlockListAsync(string userId);
        Task<ApiResponse<IEnumerable<Block>>> GetBlockListAsync();
    }
}
