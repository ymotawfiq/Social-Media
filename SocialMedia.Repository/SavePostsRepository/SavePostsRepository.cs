

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;
using static NuGet.Packaging.PackagingConstants;

namespace SocialMedia.Repository.SavePostsRepository
{
    public class SavePostsRepository : ISavePostsRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public SavePostsRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }

        public async Task<SavedPosts> AddAsync(SavedPosts t)
        {
            try
            {
                await _dbContext.SavedPosts.AddAsync(t);
                await SaveChangesAsync();
                return new SavedPosts
                {
                    FolderId = t.FolderId,
                    Id = t.Id,
                    PostId = t.PostId,
                    UserId = t.UserId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SavedPosts> DeleteByIdAsync(string id)
        {
            var savedPost = await GetByIdAsync(id);
            _dbContext.SavedPosts.Remove(savedPost);
            await SaveChangesAsync();
            return savedPost;
        }

        public async Task<IEnumerable<SavedPosts>> GetAllAsync()
        {
            return await _dbContext.SavedPosts.Select(e=>new SavedPosts
            {
                Id = e.Id,
                FolderId = e.FolderId,
                PostId = e.PostId,
                UserId = e.UserId
            }).ToListAsync();
        }

        public async Task<SavedPosts> GetByIdAsync(string id)
        {
            try
            {
                return (await _dbContext.SavedPosts.Select(e => new SavedPosts
                {
                    FolderId = e.FolderId,
                    Id = e.Id,
                    PostId = e.PostId,
                    UserId = e.UserId
                }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SavedPosts> GetSavedPostAsync(SiteUser user, string postId, string folderId)
        {
            try
            {
                return (await _dbContext.SavedPosts.Select(e=>new SavedPosts
                {
                    FolderId = e.FolderId,
                    Id = e.Id,
                    PostId = e.PostId,
                    UserId = e.UserId
                }).Where(e => e.UserId == user.Id).Where(e => e.FolderId == folderId)
                .Where(e => e.PostId == postId).FirstOrDefaultAsync())!;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SavedPosts> GetSavedPostAsync(SiteUser user, string postId)
        {
            try
            {
                return (await _dbContext.SavedPosts.Select(e => new SavedPosts
                {
                    FolderId = e.FolderId,
                    Id = e.Id,
                    PostId = e.PostId,
                    UserId = e.UserId
                }).Where(e => e.UserId == user.Id).Where(e => e.PostId == postId).FirstOrDefaultAsync())!;
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


        public async Task<SavedPosts> UnSavePostAsync(SiteUser user, string postId)
        {
            try
            {
                var savedPost = (await _dbContext.SavedPosts.Select(e => new SavedPosts
                {
                    FolderId = e.FolderId,
                    Id = e.Id,
                    PostId = e.PostId,
                    UserId = e.UserId
                }).Where(e => e.PostId == postId)
                    .Where(e=>e.UserId==user.Id).FirstOrDefaultAsync())!;
                _dbContext.SavedPosts.Remove(savedPost);
                await SaveChangesAsync();
                return savedPost;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SavedPosts> UpdateAsync(SavedPosts t)
        {
            try
            {
                var savedPost = (await _dbContext.SavedPosts.Select(e => new SavedPosts
                {
                    FolderId = e.FolderId,
                    Id = e.Id,
                    PostId = e.PostId,
                    UserId = e.UserId
                }).Where(e => e.PostId == t.PostId)
                    .Where(e => e.UserId == t.Id).FirstOrDefaultAsync())!;
                _dbContext.SavedPosts.Remove(savedPost);
                await SaveChangesAsync();
                return savedPost;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
