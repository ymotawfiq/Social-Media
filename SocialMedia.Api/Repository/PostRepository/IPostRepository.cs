using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel.ResponseObject;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Repository.PostRepository
{
    public interface IPostRepository
    {
        Task<PostResponseObject> AddPostAsync(Post post, List<PostImages> postImages);
        Task<PostResponseObject> UpdatePostAsync(Post post, List<PostImages> postImages);
        Task<bool> UpdatePostAsync(Post post);
        Task<bool> UpdateUserPostsPolicyToLockedAccountAsync(string userId);
        Task<bool> UpdateUserPostsPolicyToUnLockedAccountAsync(string userId);
        Task<bool> DeletePostAsync(string postId);
        Task<bool> DeletePostImageAsync(string imageId);
        Task<bool> DeletePostImagesAsync(string postId);
        Task<PostResponseObject> GetPostWithImagesByPostIdAsync(string postId);
        Task<Post> GetPostByIdAsync(string postId);
        Task<IEnumerable<PostResponseObject>> GetUserPostsAsync(SiteUser user);
        Task<IEnumerable<PostResponseObject>> GetUserPostsForFriendsAsync(SiteUser user);
        Task<IEnumerable<PostResponseObject>> GetUserPostsForFriendsOfFriendsAsync(SiteUser user);
        Task<IEnumerable<PostResponseObject>> GetUserPostsByPolicyAsync(SiteUser user, Policy policy);
        Task SaveChangesAsync();



    }
}
