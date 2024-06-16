

using SocialMedia.Data.Models;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.BlockRepository
{
    public interface IBlockRepository : ICrud<Block>
    {
        Task<Block> GetBlockByIdAndUserIdAsync(string blockId, string userId);
        Task<Block> GetBlockByUserIdAndBlockedUserIdAsync(string userId, string blockedUserId);
        Task<IEnumerable<Block>> GetUserBlockListAsync(string userId);
    }
}
