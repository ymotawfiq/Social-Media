

using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.BlockRepository
{
    public interface IBlockRepository : ICrud<Block>
    {
        Task<Block> GetBlockByIdAndUserIdAsync(string blockId, string userId);
        Task<Block> GetBlockByUserIdAndBlockedUserIdAsync(string userId, string blockedUserId);
        Task<IEnumerable<Block>> GetUserBlockListAsync(string userId);
    }
}
