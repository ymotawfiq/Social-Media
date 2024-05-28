
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Service.UserSavedPostsFoldersService
{
    public interface IUserSavedPostsFolderService
    {
        Task<ApiResponse<UserSavedPostsFolders>> AddUserSavedPostsFoldersAsync(
                SiteUser user, AddUserSavedPostsFolderDto addUserSavedPostsFolderDto);
        Task<ApiResponse<UserSavedPostsFolders>> UpdateFolderNameAsync(
            SiteUser user, UpdateUserSavedPostsFolderDto updateUserSavedPostsFolderDto);
        Task<ApiResponse<UserSavedPostsFolders>> DeleteUserSavedPostsFoldersByFolderIdAsync(
            SiteUser user, string folderId);
        Task<ApiResponse<UserSavedPostsFolders>> GetUserSavedPostsFoldersByFolderIdAsync(
            SiteUser user, string folderId);
        Task<ApiResponse<IEnumerable<UserSavedPostsFolders>>> GetUserFoldersByUserAsync(SiteUser user);
    }
}
