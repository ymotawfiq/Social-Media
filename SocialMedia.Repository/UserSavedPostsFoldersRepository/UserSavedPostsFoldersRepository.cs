

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.Models;

namespace SocialMedia.Repository.UserSavedPostsFoldersRepository
{
    public class UserSavedPostsFoldersRepository : IUserSavedPostsFoldersRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserSavedPostsFoldersRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
        }
        public async Task<UserSavedPostsFolders> AddUserSavedPostsFoldersAsync(
            UserSavedPostsFolders userSavedPostsFolders)
        {
            try
            {
                await _dbContext.UserSavedPostsFolders.AddAsync(userSavedPostsFolders);
                await SaveChangesAsync();
                return userSavedPostsFolders;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserSavedPostsFolders> DeleteUserSavedPostsFoldersByFolderIdAsync(string folderId)
        {
            try
            {
                var folder = (await _dbContext.UserSavedPostsFolders
                    .Where(e => e.Id == folderId).FirstOrDefaultAsync())!;
                _dbContext.UserSavedPostsFolders.Remove(folder);
                await SaveChangesAsync();
                return folder;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<IEnumerable<UserSavedPostsFolders>> GetUserFoldersByUserId(string userId)
        {
            try
            {
                return from f in await _dbContext.UserSavedPostsFolders.ToListAsync()
                       where f.UserId == userId
                       select f;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserSavedPostsFolders> GetUserSavedPostsFoldersByFolderNameAndUserIdAsync(
            string userId, string folderName)
        {
            return (await _dbContext.UserSavedPostsFolders.Where(e => e.UserId == userId)
                .Where(e => e.FolderName == folderName).FirstOrDefaultAsync())!;
        }

        public async Task<UserSavedPostsFolders> GetUserSavedPostsFoldersByFolderIdAsync(string folderId)
        {
            return (await _dbContext.UserSavedPostsFolders
                    .Where(e => e.Id == folderId).FirstOrDefaultAsync())!;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserSavedPostsFolders> UpdateFolderNameAsync(
            UserSavedPostsFolders userSavedPostsFolders)
        {
            try
            {
                var folder = (await _dbContext.UserSavedPostsFolders
                    .Where(e => e.Id == userSavedPostsFolders.Id).FirstOrDefaultAsync())!;
                folder.FolderName = userSavedPostsFolders.FolderName;
                await SaveChangesAsync();
                return folder;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
