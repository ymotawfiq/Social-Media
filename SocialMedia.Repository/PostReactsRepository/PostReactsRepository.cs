

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.PostReactsRepository
{
    public class PostReactsRepository : IPostReactsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PostReactsRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<PostReacts> AddPostReactAsync(PostReacts postReacts)
        {
            try
            {
                await _dbContext.PostReacts.AddAsync(postReacts);
                await SaveChangesAsync();
                return postReacts;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostReacts> DeletePostReactByIdAsync(string Id)
        {
            try
            {
                var postReact = await GetPostReactByIdAsync(Id);
                _dbContext.PostReacts.Remove(postReact);
                await SaveChangesAsync();
                return postReact;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostReacts> DeletePostReactByUserIdAndPostIdAsync(string userId, string postId)
        {
            try
            {
                var postReact = await GetPostReactByUserIdAndPostIdAsync(userId, postId);
                _dbContext.PostReacts.Remove(postReact);
                await SaveChangesAsync();
                return postReact;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostReacts> GetPostReactByIdAsync(string Id)
        {
            try
            {
                return (await _dbContext.PostReacts.Where(e => e.Id == Id)
                    .FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostReacts> GetPostReactByUserIdAndPostIdAsync(string userId, string postId)
        {
            try
            {
                return (await _dbContext.PostReacts.Where(e => e.PostId == postId)
                    .Where(e => e.UserId == userId).FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PostReacts>> GetPostReactsByPostIdAsync(string postId)
        {
            try
            {
                return from p in await _dbContext.PostReacts.ToListAsync()
                       where p.PostId == postId
                       select p;
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

        public async Task<PostReacts> UpdatePostReactAsync(PostReacts postReacts)
        {
            try
            {
                var oldPostReact = await GetPostReactByIdAsync(postReacts.Id);
                oldPostReact.ReactId = postReacts.ReactId;
                await SaveChangesAsync();
                return oldPostReact;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
