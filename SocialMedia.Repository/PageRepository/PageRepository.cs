

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.PageRepository
{
    public class PageRepository : IPageRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PageRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<Page> AddAsync(Page t)
        {
            await _dbContext.Pages.AddAsync(t);
            await SaveChangesAsync();
            return new Page
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                CreatedAt = t.CreatedAt,
                CreatorId = t.CreatorId
            };
        }


        public async Task<Page> DeleteByIdAsync(string id)
        {
            var existPage = await GetByIdAsync(id);
            _dbContext.Pages.Remove(existPage);
            await SaveChangesAsync();
            return existPage;
        }


        public async Task<IEnumerable<Page>> GetAllAsync()
        {
            return await _dbContext.Pages.Select(e=>new Page
            {
                Id = e.Id,
                CreatedAt = e.CreatedAt,
                CreatorId = e.CreatorId,
                Description = e.Description,
                Name = e.Name
            }).ToListAsync();
        }

        public async Task<Page> GetByIdAsync(string id)
        {
            return (await _dbContext.Pages.Select(e => new Page
            {
                CreatedAt = e.CreatedAt,
                Description = e.Description,
                Name = e.Name,
                Id = e.Id
            }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
        }


        public async Task<IEnumerable<Page>> GetPagesByUserIdAsync(string userId)
        {
            return await _dbContext.Pages.Select(e => new Page
            {
                CreatorId = e.CreatorId,
                CreatedAt = e.CreatedAt,
                Description = e.Description,
                Name = e.Name,
                Id = e.Id
            }).Where(e => e.CreatorId == userId).ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Page> UpdateAsync(Page t)
        {
            var existPage = await GetByIdAsync(t.Id);
            existPage.Description = t.Description;
            existPage.Name = t.Name;
            await SaveChangesAsync();
            return new Page
            {
                CreatorId = existPage.CreatorId,
                CreatedAt = existPage.CreatedAt,
                Description = existPage.Description,
                Id = existPage.Id,
                Name = existPage.Name
            };
        }


    }
}
