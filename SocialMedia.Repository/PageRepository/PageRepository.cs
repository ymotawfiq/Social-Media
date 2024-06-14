

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Repository.PageRepository
{
    public class PageRepository : IPageRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PageRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<Page> AddPageAsync(Page page, SiteUser user)
        {
            await _dbContext.Pages.AddAsync(page);
            await _dbContext.UserPages.AddAsync(new UserPage
            {
                UserId = user.Id,
                PageId = page.Id,
                Id = Guid.NewGuid().ToString()
            });
            await SaveChangesAsync();
            return new Page
            {
                Id = page.Id,
                Name = page.Name,
                Description = page.Description,
                CreatedAt = page.CreatedAt
            };
        }

        public async Task<Page> DeletePageByIdAsync(string pageId)
        {
            var existPage = await GetPageByIdAsync(pageId);
            _dbContext.Pages.Remove(existPage);
            await SaveChangesAsync();
            return existPage;
        }

        public async Task<Page> GetPageByIdAsync(string pageId)
        {
            return (await _dbContext.Pages.Select(e=>new Page
            {
                CreatedAt = e.CreatedAt,
                Description = e.Description,
                Name = e.Name,
                Id = e.Id
            }).Where(e => e.Id == pageId).FirstOrDefaultAsync())!;
        }

        public async Task<IEnumerable<UserPage>> GetPagesByUserIdAsync(string userId)
        {
            return await _dbContext.UserPages.Select(e => new UserPage
            {
                PageId = e.PageId,
                UserId = e.UserId,
                Id = e.Id
            }).Where(e => e.UserId == userId).ToListAsync();
        }

        public async Task<UserPage> GetUserPageByPageIdAsync(string pageId)
        {
            return (await _dbContext.UserPages.Select(e => new UserPage
            {
                PageId = e.PageId,
                UserId = e.UserId,
                Id = e.Id
            }).Where(e => e.PageId == pageId).FirstOrDefaultAsync())!;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Page> UpdatePageAsync(Page page)
        {
            var existPage = await GetPageByIdAsync(page.Id);
            existPage.Description = page.Description;
            existPage.Name = page.Name;
            await SaveChangesAsync();
            return existPage;
        }
    }
}
