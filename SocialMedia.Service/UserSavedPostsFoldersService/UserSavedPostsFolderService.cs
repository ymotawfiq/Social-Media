
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.UserSavedPostsFoldersRepository;
using SocialMedia.Service.GenericReturn;

namespace SocialMedia.Service.UserSavedPostsFoldersService
{
    public class UserSavedPostsFolderService : IUserSavedPostsFolderService
    {
        private readonly IUserSavedPostsFoldersRepository _userSavedPostsFoldersRepository;
        public UserSavedPostsFolderService(IUserSavedPostsFoldersRepository _userSavedPostsFoldersRepository)
        {
            this._userSavedPostsFoldersRepository = _userSavedPostsFoldersRepository;
        }
        public async Task<ApiResponse<UserSavedPostsFolders>> AddUserSavedPostsFoldersAsync(
            SiteUser user, AddUserSavedPostsFolderDto addUserSavedPostsFolderDto)
        {
            var existFolder = await _userSavedPostsFoldersRepository
                .GetUserSavedPostsFoldersByFolderNameAndUserIdAsync(user.Id, 
                addUserSavedPostsFolderDto.FolderName);
            if (existFolder!=null)
            {
                return StatusCodeReturn<UserSavedPostsFolders>
                    ._403_Forbidden("Folder with same name already exists");
            }
            var newFolder = await _userSavedPostsFoldersRepository.AddUserSavedPostsFoldersAsync(
                new UserSavedPostsFolders
                {
                    FolderName = addUserSavedPostsFolderDto.FolderName,
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id
                }
                );
            return StatusCodeReturn<UserSavedPostsFolders>
                ._201_Created("Folder created successfully", newFolder); 
        }

        public async Task<ApiResponse<UserSavedPostsFolders>> DeleteUserSavedPostsFoldersByFolderIdAsync(
            SiteUser user, string folderId)
        {
            var folder = await _userSavedPostsFoldersRepository.GetUserSavedPostsFoldersByFolderIdAsync(
                folderId);
            if (folder != null)
            {
                if (folder.UserId == user.Id)
                {
                    await _userSavedPostsFoldersRepository.DeleteUserSavedPostsFoldersByFolderIdAsync(folderId);
                    return StatusCodeReturn<UserSavedPostsFolders>
                        ._200_Success("Folder deleted successfully", folder);
                }
                return StatusCodeReturn<UserSavedPostsFolders>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<UserSavedPostsFolders>
                ._404_NotFound("Folder not found");
        }

        public async Task<ApiResponse<IEnumerable<UserSavedPostsFolders>>> GetUserFoldersByUserAsync(
            SiteUser user)
        {
            var folders = await _userSavedPostsFoldersRepository.GetUserFoldersByUserId(user.Id);
            if (folders.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<UserSavedPostsFolders>>
                    ._200_Success("No folders found", folders);
            }
            return StatusCodeReturn<IEnumerable<UserSavedPostsFolders>>
                    ._200_Success("Folders found successfully", folders);
        }

        public async Task<ApiResponse<UserSavedPostsFolders>> GetUserSavedPostsFoldersByFolderIdAsync(
            SiteUser user, string folderId)
        {
            var folder = await _userSavedPostsFoldersRepository.GetUserSavedPostsFoldersByFolderIdAsync(
                folderId);
            if (folder != null)
            {
                if(folder.UserId == user.Id)
                {
                    folder.User = null;
                    return StatusCodeReturn<UserSavedPostsFolders>
                    ._200_Success("Folder found successfully", folder);
                }
                return StatusCodeReturn<UserSavedPostsFolders>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<UserSavedPostsFolders>
                ._404_NotFound("Folder not found");
        }

        public async Task<ApiResponse<UserSavedPostsFolders>> UpdateFolderNameAsync(
            SiteUser user, UpdateUserSavedPostsFolderDto updateUserSavedPostsFolderDto)
        {
            var folder = await _userSavedPostsFoldersRepository.GetUserSavedPostsFoldersByFolderIdAsync(
                updateUserSavedPostsFolderDto.Id);
            if (folder != null)
            {
                var existFolder = await _userSavedPostsFoldersRepository
                .GetUserSavedPostsFoldersByFolderNameAndUserIdAsync(user.Id,
                    updateUserSavedPostsFolderDto.FolderName);
                if (existFolder != null)
                {
                    return StatusCodeReturn<UserSavedPostsFolders>
                        ._403_Forbidden("Folder with same name already exists");
                }
                var updatedFolder = await _userSavedPostsFoldersRepository.UpdateFolderNameAsync(
                    new UserSavedPostsFolders
                    {
                        Id = updateUserSavedPostsFolderDto.Id,
                        FolderName = updateUserSavedPostsFolderDto.FolderName
                    }
                    );
                return StatusCodeReturn<UserSavedPostsFolders>
                    ._200_Success("Folder updated successfully", updatedFolder);
            }
            return StatusCodeReturn<UserSavedPostsFolders>
                ._404_NotFound("Folder not found");
        }
    }
}
