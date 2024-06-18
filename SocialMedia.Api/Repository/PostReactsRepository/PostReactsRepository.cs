

using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Repository.PostReactsRepository
{
    public class PostReactsRepository : IPostReactsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public PostReactsRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<PostReacts> AddAsync(PostReacts t)
        {
            try
            {
                await _dbContext.PostReacts.AddAsync(t);
                await SaveChangesAsync();
                return new PostReacts
                {
                    UserId = t.UserId,
                    Id = t.Id,
                    PostId = t.PostId,
                    PostReactId = t.PostReactId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostReacts> DeleteByIdAsync(string id)
        {
            try
            {
                var postReact = await GetByIdAsync(id);
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

        public async Task<IEnumerable<PostReacts>> GetAllAsync()
        {
            return await _dbContext.PostReacts.Select(e=>new PostReacts
            {
                UserId = e.UserId,
                PostId = e.PostId,
                Id = e.Id,
                PostReactId = e.PostReactId
            }).ToListAsync();
        }

        public async Task<PostReacts> GetByIdAsync(string id)
        {
            try
            {
                return (await _dbContext.PostReacts.Where(e => e.Id == id)
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

        public async Task<PostReacts> UpdateAsync(PostReacts t)
        {
            try
            {
                var oldPostReact = await GetByIdAsync(t.Id);
                oldPostReact.PostReactId = t.PostReactId;
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
