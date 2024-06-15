

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
        public async Task<PagePost> AddPagePostAsync(PagePost pagePost)
        {
            await _dbContext.PagePosts.AddAsync(pagePost);
            await SaveChangesAsync();
            return new PagePost
            {
                Id = pagePost.Id,
                PageId = pagePost.PageId,
                PostId = pagePost.PostId
            };
        }

        public async Task<PagePost> DeletePagePostByIdAsync(string pagePostId)
        {
            var pagePost = await GetPagePostByIdAsync(pagePostId);
            _dbContext.PagePosts.Remove(pagePost);
            await SaveChangesAsync();
            return pagePost;
        }

        public async Task<PagePost> GetPagePostByIdAsync(string pagePostId)
        {
            return (await _dbContext.PagePosts.Select(e=>new PagePost
            {
                PostId = e.PostId,
                PageId = e.PageId,
                Id = e.Id
            }).Where(e => e.Id == pagePostId).FirstOrDefaultAsync())!;
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
    }
}
