

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.DTOs;
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

        public async Task<SavedPosts> GetSavedPostAsync(SiteUser user, string postId, string folderId)
        {
            try
            {
                return (await _dbContext.SavedPosts.Where(e => e.UserId == user.Id)
                    .Where(e => e.FolderId == folderId).Where(e => e.PostId == postId)
                    .FirstOrDefaultAsync())!;
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
                return (await _dbContext.SavedPosts.Where(e => e.UserId == user.Id)
                    .Where(e => e.PostId == postId).FirstOrDefaultAsync())!;
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

        public async Task<SavedPosts> SavePostAsync(SavedPosts savedPosts)
        {
            try
            {
                await _dbContext.SavedPosts.AddAsync(savedPosts);
                await SaveChangesAsync();
                return savedPosts;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<SavedPosts> UnSavePostAsync(SiteUser user, string postId)
        {
            try
            {
                var savedPost = (await _dbContext.SavedPosts.Where(e => e.PostId == postId)
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
    }
}
