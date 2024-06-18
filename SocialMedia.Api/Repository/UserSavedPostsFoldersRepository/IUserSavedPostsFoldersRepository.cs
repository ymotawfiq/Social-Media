

using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.UserSavedPostsFoldersRepository
{
    public interface IUserSavedPostsFoldersRepository : ICrud<UserSavedPostsFolders>
    {
        Task<UserSavedPostsFolders> GetUserSavedPostsFoldersByFolderNameAndUserIdAsync(
            string userId, string folderName);
        Task<IEnumerable<UserSavedPostsFolders>> GetUserFoldersByUserId(string userId);
    }
}
