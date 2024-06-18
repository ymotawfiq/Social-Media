

using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.PostRepository;

namespace SocialMedia.Api.Repository.PagePostsRepository
{
    public class PagePostsRepository : IPagePostsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PagePostsRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<PagePost> AddAsync(PagePost t)
        {
            await _dbContext.PagePosts.AddAsync(t);
            await SaveChangesAsync();
            return new PagePost
            {
                Id = t.Id,
                PageId = t.PageId,
                PostId = t.PostId
            };
        }


        public async Task<PagePost> DeleteByIdAsync(string id)
        {
            var pagePost = await GetByIdAsync(id);
            _dbContext.PagePosts.Remove(pagePost);
            await SaveChangesAsync();
            return pagePost;
        }


        public async Task<IEnumerable<PagePost>> GetAllAsync()
        {
            return await _dbContext.PagePosts.Select(e=>new PagePost
            {
                PostId = e.PostId,
                Id = e.Id,
                PageId = e.PageId
            }).ToListAsync();
        }

        public async Task<PagePost> GetByIdAsync(string id)
        {
            return (await _dbContext.PagePosts.Select(e => new PagePost
            {
                PostId = e.PostId,
                PageId = e.PageId,
                Id = e.Id
            }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
        }


        public async Task<PagePost> GetPagePostByPostIdAsync(string postId)
        {
            return (await _dbContext.PagePosts.Select(e => new PagePost
            {
                PostId = e.PostId,
                PageId = e.PageId,
                Id = e.Id
            }).Where(e => e.PostId == postId).FirstOrDefaultAsync())!;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PagePost> UpdateAsync(PagePost t)
        {
            return await DeleteByIdAsync(t.Id);
        }
    }
}
