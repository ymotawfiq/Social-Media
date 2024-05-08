

using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Repository.UserPostsRepository
{
    public interface IUserPostsRepository
    {
        Task<UserPosts> AddUserPostAsync(UserPosts userPosts);
        Task<UserPosts> GetUserPostByIdAsync(string userPostId);
        Task<UserPosts> GetUserPostByPostIdAsync(string postId);
        Task<UserPosts> GetUserPostByUserAndPostIdAsync(string userId, string postId);
        Task<UserPosts> DeleteUserPostByIdAsync(string userPostId);
        Task<UserPosts> DeleteUserPostByUserAndPostIdAsync(string userId, string postId);
        Task<IEnumerable<UserPosts>> GetUserPostsByUserIdAsync(string userId);
        Task SaveChangesAsync();
    }
}
