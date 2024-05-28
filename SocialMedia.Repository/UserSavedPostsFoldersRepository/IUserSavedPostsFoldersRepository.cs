

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.UserSavedPostsFoldersRepository
{
    public interface IUserSavedPostsFoldersRepository
    {
        Task<UserSavedPostsFolders> AddUserSavedPostsFoldersAsync(
            UserSavedPostsFolders userSavedPostsFolders);
        Task<UserSavedPostsFolders> UpdateFolderNameAsync(UserSavedPostsFolders userSavedPostsFolders);
        Task<UserSavedPostsFolders> DeleteUserSavedPostsFoldersByFolderIdAsync(
            string folderId);
        Task<UserSavedPostsFolders> GetUserSavedPostsFoldersByFolderIdAsync(
            string folderId);
        Task<UserSavedPostsFolders> GetUserSavedPostsFoldersByFolderNameAndUserIdAsync(
            string userId, string folderName);
        Task<IEnumerable<UserSavedPostsFolders>> GetUserFoldersByUserId(string userId);
        Task SaveChangesAsync();
    }
}
