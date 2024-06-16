
using SocialMedia.Data.Models;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.PostViewRepository
{
    public interface IPostViewRepository : ICrud<PostView>
    {
        Task<IEnumerable<PostView>> GetPostViews(string postId);
        Task<PostView> GetPostViewByPostIdAsync(string postId);
    }
}
