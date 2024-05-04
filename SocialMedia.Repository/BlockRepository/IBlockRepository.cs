

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.BlockRepository
{
    public interface IBlockRepository
    {
        Task<Block> BlockUserAsync(Block block);
        Task<Block> UnBlockUserAsync(Block block);
        Task<Block> GetBlockByIdAsync(Guid blockId);
        Task<Block> GetBlockByIdAndUserIdAsync(Guid blockId, string userId);
        Task<Block> GetBlockByUserIdAndBlockedUserIdAsync(string userId, string blockedUserId);
        Task<IEnumerable<Block>> GetUserBlockListAsync(string userId);
        Task<IEnumerable<Block>> GetBlockListAsync();
        Task SaveChangesAsync();
    }
}
