

using SocialMedia.Data.Models;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.UserSavedPostsFoldersRepository
{
    public interface IUserSavedPostsFoldersRepository : ICrud<UserSavedPostsFolders>
    {
        Task<UserSavedPostsFolders> GetUserSavedPostsFoldersByFolderNameAndUserIdAsync(
            string userId, string folderName);
        Task<IEnumerable<UserSavedPostsFolders>> GetUserFoldersByUserId(string userId);
    }
}
