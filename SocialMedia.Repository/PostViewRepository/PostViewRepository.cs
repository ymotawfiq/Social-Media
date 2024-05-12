

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.PostViewRepository
{
    public class PostViewRepository : IPostViewRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PostViewRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<PostView> AddPostViewAsync(PostView postView)
        {
            try
            {
                await _dbContext.PostViews.AddAsync(postView);
                await SaveChangesAsync();
                return postView;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostView> GetPostViewByPostIdAsync(string postId)
        {
            try
            {
                return (await _dbContext.PostViews.Where(e => e.PostId == postId).FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PostView>> GetPostViews(string postId)
        {
            try
            {
                return from v in await _dbContext.PostViews.ToListAsync()
                       where v.PostId == postId
                       select v;
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

        public async Task<PostView> UpdatePostViewAsync(PostView postView)
        {
            try
            {
                var postView1 = await GetPostViewByPostIdAsync(postView.PostId);
                postView1.ViewNumber = postView.ViewNumber++;
                _dbContext.PostViews.Update(postView);
                await SaveChangesAsync();
                return postView1;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
