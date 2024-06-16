

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

        public async Task<Block> AddAsync(Block t)
        {
            try
            {
                await _dbContext.Blocks.AddAsync(t);
                await SaveChangesAsync();
                return new Block
                {
                    BlockedUserId = t.BlockedUserId,
                    Id = t.Id,
                    UserId = t.UserId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Block> DeleteByIdAsync(string id)
        {
            var block = await GetByIdAsync(id);
            _dbContext.Blocks.Remove(block);
            await SaveChangesAsync();
            return block;
        }

        public async Task<IEnumerable<Block>> GetAllAsync()
        {
            return await _dbContext.Blocks.Select(e => new Block
            {
                UserId = e.UserId,
                Id = e.Id,
                BlockedUserId = e.BlockedUserId
            }).ToListAsync();
        }

        public async Task<Block> GetBlockByIdAndUserIdAsync(string blockId, string userId)
        {
            try
            {
                var block = (await _dbContext.Blocks.Select(e => new Block
                {
                    UserId = e.UserId,
                    Id = e.Id,
                    BlockedUserId = e.BlockedUserId
                }).Where(e => e.Id == blockId).Where(e => e.UserId == userId).FirstOrDefaultAsync())!;
                return block;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Block> GetBlockByUserIdAndBlockedUserIdAsync(string userId, string blockedUserId)
        {
            var block1 = await _dbContext.Blocks.Select(e => new Block
            {
                UserId = e.UserId,
                Id = e.Id,
                BlockedUserId = e.BlockedUserId
            }).Where(e => e.UserId == userId)
            .Where(e => e.BlockedUserId == blockedUserId).FirstOrDefaultAsync();
            var block2 = await _dbContext.Blocks.Select(e => new Block
            {
                UserId = e.UserId,
                Id = e.Id,
                BlockedUserId = e.BlockedUserId
            }).Where(e => e.UserId == blockedUserId)
            .Where(e => e.BlockedUserId == userId).FirstOrDefaultAsync();
            return block1 == null ? block2! : block1;
        }

        public async Task<Block> GetByIdAsync(string id)
        {
            try
            {
                var block = (await _dbContext.Blocks.Select(e => new Block
                {
                    UserId = e.UserId,
                    Id = e.Id,
                    BlockedUserId = e.BlockedUserId
                }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
                return block;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Block>> GetUserBlockListAsync(string userId)
        {
            try
            {
                return
                    from b in await GetAllAsync()
                    where b.UserId == userId
                    select (new Block
                    {
                        BlockedUserId = b.BlockedUserId,
                        Id = b.Id,
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

        public async Task<Block> UpdateAsync(Block t)
        {
            try
            {
                var block1 = await GetBlockByUserIdAndBlockedUserIdAsync(t.UserId, t.BlockedUserId);
                _dbContext.Remove(block1);
                await SaveChangesAsync();
                return new Block
                {
                    BlockedUserId = block1.BlockedUserId,
                    Id = block1.Id,
                    UserId = block1.UserId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
