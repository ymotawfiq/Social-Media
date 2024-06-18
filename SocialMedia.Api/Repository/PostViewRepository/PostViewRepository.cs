

using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Repository.PostViewRepository
{
    public class PostViewRepository : IPostViewRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PostViewRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<PostView> AddAsync(PostView t)
        {
            try
            {
                await _dbContext.PostViews.AddAsync(t);
                await SaveChangesAsync();
                return new PostView
                {
                    Id = t.Id,
                    PostId = t.PostId,
                    ViewNumber = t.ViewNumber
                };
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<PostView> DeleteByIdAsync(string id)
        {
            var postView = await GetByIdAsync(id);
            _dbContext.PostViews.Remove(postView);
            await SaveChangesAsync();
            return postView;
        }

        public async Task<IEnumerable<PostView>> GetAllAsync()
        {
            return await _dbContext.PostViews.Select(e=>new PostView
            {
                Id = e.Id,
                PostId = e.PostId,
                ViewNumber = e.ViewNumber
            }).ToListAsync();
        }

        public async Task<PostView> GetByIdAsync(string id)
        {
            return (await _dbContext.PostViews.Select(e => new PostView
            {
                Id = e.Id,
                PostId = e.PostId,
                ViewNumber = e.ViewNumber
            }).FirstOrDefaultAsync())!;
        }

        public async Task<PostView> GetPostViewByPostIdAsync(string postId)
        {
            try
            {
                return (await _dbContext.PostViews.Select(e=>new PostView
                {
                    ViewNumber = e.ViewNumber,
                    PostId = e.PostId,
                    Id = e.Id
                }).Where(e => e.PostId == postId).FirstOrDefaultAsync())!;
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
                return from v in await GetAllAsync()
                       where v.PostId == postId
                       select (new PostView
                       {
                           Id = v.Id,
                           PostId = v.PostId,
                           ViewNumber = v.ViewNumber
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

        public async Task<PostView> UpdateAsync(PostView t)
        {
            try
            {
                var postView1 = await GetPostViewByPostIdAsync(t.PostId);
                postView1.ViewNumber = t.ViewNumber++;
                _dbContext.PostViews.Update(t);
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
