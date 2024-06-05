

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
            return page;
        }

        public async Task<Page> DeletePageByIdAsync(string pageId)
        {
            var existPage = await GetPageByIdAsync(pageId);
            var userPage = (await _dbContext.UserPages.Where(e => e.PageId == pageId).FirstOrDefaultAsync())!;
            _dbContext.Pages.Remove(existPage);
            _dbContext.UserPages.Remove(userPage!);
            await SaveChangesAsync();
            return existPage;
        }

        public async Task<Page> GetPageByIdAsync(string pageId)
        {
            return (await _dbContext.Pages.Where(e => e.Id == pageId).FirstOrDefaultAsync())!;
        }

        public async Task<IEnumerable<UserPage>> GetPagesByUserIdAsync(string userId)
        {
            return await _dbContext.UserPages.Where(e => e.UserId == userId).ToListAsync();
        }

        public async Task<UserPage> GetUserPageByPageIdAsync(string pageId)
        {
            return (await _dbContext.UserPages.Where(e => e.PageId == pageId).FirstOrDefaultAsync())!;
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
