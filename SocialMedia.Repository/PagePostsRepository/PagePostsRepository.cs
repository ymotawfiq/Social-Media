

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.PostRepository;

namespace SocialMedia.Repository.PagePostsRepository
{
    public class PagePostsRepository : IPagePostsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PagePostsRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<PagePosts> AddPagePostAsync(PagePosts pagePost)
        {
            await _dbContext.PagePosts.AddAsync(pagePost);
            await SaveChangesAsync();
            return pagePost;
        }

        public async Task<PagePosts> DeletePagePostByIdAsync(string pagePostId)
        {
            var pagePost = await GetPagePostByIdAsync(pagePostId);
            _dbContext.PagePosts.Remove(pagePost);
            await SaveChangesAsync();
            return pagePost;
        }

        public async Task<PagePosts> GetPagePostByIdAsync(string pagePostId)
        {
            return (await _dbContext.PagePosts.Where(e => e.Id == pagePostId).FirstOrDefaultAsync())!;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
