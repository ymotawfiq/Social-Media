

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
                return new PostReacts
                {
                    UserId = postReacts.UserId,
                    Id = postReacts.Id,
                    PostId = postReacts.PostId,
                    PostReactId = postReacts.PostReactId
                };
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
                    .Select(e => new PostReacts
                    {
                        Id = e.Id,
                        UserId = e.UserId,
                        PostId = e.PostId,
                        PostReactId = e.PostReactId
                    }).FirstOrDefaultAsync())!;
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
                    .Where(e => e.UserId == userId).Select(e => new PostReacts
                    {
                        Id = e.Id,
                        UserId = e.UserId,
                        PostId = e.PostId,
                        PostReactId = e.PostReactId
                    }).FirstOrDefaultAsync())!;
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
                       select (new PostReacts
                       {
                           Id = p.Id,
                           PostReactId = p.PostReactId,
                           PostId = p.PostId,
                           UserId = p.UserId
                       });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<PostReacts>> GetPostReactsByUserIdAsync(string userId)
        {
            try
            {
                return from p in await _dbContext.PostReacts.ToListAsync()
                       where p.UserId == userId
                       select (new PostReacts
                       {
                           Id = p.Id,
                           PostReactId = p.PostReactId,
                           PostId = p.PostId,
                           UserId = p.UserId
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

        public async Task<PostReacts> UpdatePostReactAsync(PostReacts postReacts)
        {
            try
            {
                var oldPostReact = await GetPostReactByIdAsync(postReacts.Id);
                oldPostReact.PostReactId = postReacts.PostReactId;
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
