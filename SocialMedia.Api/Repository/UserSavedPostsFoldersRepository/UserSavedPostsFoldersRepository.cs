

using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;

namespace SocialMedia.Api.Repository.UserSavedPostsFoldersRepository
{
    public class UserSavedPostsFoldersRepository : IUserSavedPostsFoldersRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public UserSavedPostsFoldersRepository(ApplicationDbContext _dbContext)
        {
            this._dbContext = _dbContext;
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


        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }


        public async Task<UserSavedPostsFolders> AddAsync(UserSavedPostsFolders t)
        {
            try
            {
                t.FolderName = t.FolderName.ToUpper();
                await _dbContext.UserSavedPostsFolders.AddAsync(t);
                await SaveChangesAsync();
                return new UserSavedPostsFolders
                {
                    UserId = t.UserId,
                    Id = t.Id,
                    FolderName = t.FolderName
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserSavedPostsFolders> UpdateAsync(UserSavedPostsFolders t)
        {
            try
            {
                t.FolderName = t.FolderName.ToUpper();
                var folder = await GetByIdAsync(t.Id);
                folder.FolderName = t.FolderName;
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

        public async Task<UserSavedPostsFolders> DeleteByIdAsync(string id)
        {
            try
            {
                var folder = await GetByIdAsync(id);
                _dbContext.UserSavedPostsFolders.Remove(folder);
                await SaveChangesAsync();
                return folder;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<UserSavedPostsFolders> GetByIdAsync(string id)
        {
            return (await _dbContext.UserSavedPostsFolders.Select(e => new UserSavedPostsFolders
            {
                FolderName = e.FolderName,
                Id = e.Id,
                UserId = e.UserId
            }).Where(e => e.Id == id).FirstOrDefaultAsync())!;
        }

        public async Task<IEnumerable<UserSavedPostsFolders>> GetAllAsync()
        {
            return await _dbContext.UserSavedPostsFolders.ToListAsync();
        }
    }
}
