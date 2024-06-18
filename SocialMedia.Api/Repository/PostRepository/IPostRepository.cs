

using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.Authentication;

namespace SocialMedia.Api.Repository.PostRepository
{
    public interface IPostRepository
    {
        Task<PostDto> AddPostAsync(Post post, List<PostImages> postImages);
        Task<PostDto> UpdatePostAsync(Post post, List<PostImages> postImages);
        Task<bool> UpdatePostAsync(Post post);
        Task<bool> UpdateUserPostsPolicyToLockedAccountAsync(string userId);
        Task<bool> UpdateUserPostsPolicyToUnLockedAccountAsync(string userId);
        Task<bool> DeletePostAsync(string postId);
        Task<bool> DeletePostImageAsync(string imageId);
        Task<bool> DeletePostImagesAsync(string postId);
        Task<PostDto> GetPostWithImagesByPostIdAsync(string postId);
        Task<Post> GetPostByIdAsync(string postId);
        Task<IEnumerable<PostDto>> GetUserPostsAsync(SiteUser user);
        Task<IEnumerable<PostDto>> GetUserPostsForFriendsAsync(SiteUser user);
        Task<IEnumerable<PostDto>> GetUserPostsForFriendsOfFriendsAsync(SiteUser user);
        Task<IEnumerable<PostDto>> GetUserPostsByPolicyAsync(SiteUser user, Policy policy);
        Task SaveChangesAsync();



    }
}
