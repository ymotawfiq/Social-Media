

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
                return new Block
                {
                    BlockedUserId = block.BlockedUserId,
                    Id = block.Id,
                    UserId = block.UserId
                };
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
                var block = (await _dbContext.Blocks.Where(e => e.Id == blockId)
                    .Select(e=>new Block
                    {
                        UserId = e.UserId,
                        Id = e.Id,
                        BlockedUserId = e.BlockedUserId
                    }).Where(e => e.UserId == userId).FirstOrDefaultAsync())!;
                return block;
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
                var block = (await _dbContext.Blocks.Select(e => new Block
                {
                    UserId = e.UserId,
                    Id = e.Id,
                    BlockedUserId = e.BlockedUserId
                }).Where(e => e.Id == blockId).FirstOrDefaultAsync())!;
                return block;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Block> GetBlockByUserIdAndBlockedUserIdAsync(string userId, string blockedUserId)
        {
            var block1 = await _dbContext.Blocks.Where(e => e.UserId == userId)
                .Select(e => new Block
                {
                    UserId = e.UserId,
                    Id = e.Id,
                    BlockedUserId = e.BlockedUserId
                }).Where(e => e.BlockedUserId == blockedUserId).FirstOrDefaultAsync();
            var block2 = await _dbContext.Blocks.Where(e => e.UserId == blockedUserId)
                .Select(e => new Block
                {
                    UserId = e.UserId,
                    Id = e.Id,
                    BlockedUserId = e.BlockedUserId
                }).Where(e => e.BlockedUserId == userId).FirstOrDefaultAsync();
            return block1 == null ? block2! : block1;
        }

        public async Task<IEnumerable<Block>> GetBlockListAsync()
        {
            return await _dbContext.Blocks.Select(e => new Block
            {
                UserId = e.UserId,
                Id = e.Id,
                BlockedUserId = e.BlockedUserId
            }).ToListAsync();
        }

        public async Task<IEnumerable<Block>> GetUserBlockListAsync(string userId)
        {
            try
            {
                return
                    from b in await _dbContext.Blocks.ToListAsync()
                    where b.UserId == userId
                    select b;
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
                return block1;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
