
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.PostReactsRepository
{
    public interface IPostReactsRepository : ICrud<PostReacts>
    {

        Task<PostReacts> GetPostReactByUserIdAndPostIdAsync(string userId, string postId);
        Task<PostReacts> DeletePostReactByUserIdAndPostIdAsync(string userId, string postId);
        Task<IEnumerable<PostReacts>> GetPostReactsByPostIdAsync(string postId);
        Task<IEnumerable<PostReacts>> GetPostReactsByUserIdAsync(string userId);
    }
}
