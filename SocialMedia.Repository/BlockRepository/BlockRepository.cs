

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.BlockRepository
{
    public class BlockRepository : IBlockRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public BlockRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<Block> BlockUserAsync(Block block)
        {
            try
            {
                await _dbContext.Blocks.AddAsync(block);
                await SaveChangesAsync();
                block.User = null;
                return block;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Block> GetBlockByIdAndUserIdAsync(string blockId, string userId)
        {
            try
            {
                var block = await _dbContext.Blocks.Where(e => e.Id == blockId)
                    .Where(e => e.UserId == userId).FirstOrDefaultAsync();
                return block!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Block> GetBlockByIdAsync(string blockId)
        {
            try
            {
                return (await _dbContext.Blocks.Where(e => e.Id == blockId).FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Block> GetBlockByUserIdAndBlockedUserIdAsync(string userId, string blockedUserId)
        {
            var block1 = await _dbContext.Blocks.Where(e => e.UserId == userId)
                .Where(e => e.BlockedUserId == blockedUserId).FirstOrDefaultAsync();
            var block2 = await _dbContext.Blocks.Where(e => e.UserId == blockedUserId)
                .Where(e => e.BlockedUserId == userId).FirstOrDefaultAsync();
            return block1 == null ? block2! : block1;
        }

        public async Task<IEnumerable<Block>> GetBlockListAsync()
        {
            return await _dbContext.Blocks.ToListAsync();
        }

        public async Task<IEnumerable<Block>> GetUserBlockListAsync(string userId)
        {
            try
            {
                return
                    from b in await _dbContext.Blocks.ToListAsync()
                    where b.UserId == userId
                    select (new Block
                    {
                        Id = b.Id,
                        BlockedUserId = b.BlockedUserId,
                        UserId = b.UserId
                    });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Block> UnBlockUserAsync(Block block)
        {
            try
            {
                var block1 = await GetBlockByUserIdAndBlockedUserIdAsync(block.UserId, block.BlockedUserId);
                _dbContext.Remove(block1);
                await SaveChangesAsync();
                block1.User = null;
                return block1;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
