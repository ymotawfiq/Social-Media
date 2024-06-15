

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
                userSavedPostsFolders.FolderName = userSavedPostsFolders.FolderName.ToUpper();
                await _dbContext.UserSavedPostsFolders.AddAsync(userSavedPostsFolders);
                await SaveChangesAsync();
                return new UserSavedPostsFolders
                {
                    UserId = userSavedPostsFolders.UserId,
                    Id = userSavedPostsFolders.Id,
                    FolderName = userSavedPostsFolders.FolderName
                };
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
                var folder = await GetUserSavedPostsFoldersByFolderIdAsync(folderId);
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
                       select (new UserSavedPostsFolders
                       {
                           UserId = f.UserId,
                           Id = f.Id,
                           FolderName = f.FolderName
                       });
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserSavedPostsFolders> GetUserSavedPostsFoldersByFolderNameAndUserIdAsync(
            string userId, string folderName)
        {
            folderName = folderName.ToUpper();
            return (await _dbContext.UserSavedPostsFolders.Select(e => new UserSavedPostsFolders
            {
                FolderName = e.FolderName,
                Id = e.Id,
                UserId = e.UserId
            }).Where(e => e.UserId == userId).Where(e => e.FolderName == folderName).FirstOrDefaultAsync())!;
        }

        public async Task<UserSavedPostsFolders> GetUserSavedPostsFoldersByFolderIdAsync(string folderId)
        {
            return (await _dbContext.UserSavedPostsFolders.Select(e => new UserSavedPostsFolders
            {
                FolderName = e.FolderName,
                Id = e.Id,
                UserId = e.UserId
            }).Where(e => e.Id == folderId).FirstOrDefaultAsync())!;
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
                userSavedPostsFolders.FolderName = userSavedPostsFolders.FolderName.ToUpper();
                var folder = await GetUserSavedPostsFoldersByFolderIdAsync(userSavedPostsFolders.Id);
                folder.FolderName = userSavedPostsFolders.FolderName;
                await SaveChangesAsync();
                return new UserSavedPostsFolders
                {
                    FolderName = folder.FolderName,
                    Id = folder.Id,
                    UserId = folder.UserId
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
