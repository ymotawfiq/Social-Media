
using SocialMedia.Data.Models;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.PostReactsRepository
{
    public interface IPostReactsRepository : ICrud<PostReacts>
    {

        Task<PostReacts> GetPostReactByUserIdAndPostIdAsync(string userId, string postId);
        Task<PostReacts> DeletePostReactByUserIdAndPostIdAsync(string userId, string postId);
        Task<IEnumerable<PostReacts>> GetPostReactsByPostIdAsync(string postId);
        Task<IEnumerable<PostReacts>> GetPostReactsByUserIdAsync(string userId);
    }
}
