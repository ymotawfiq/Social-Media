
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.PostViewRepository
{
    public interface IPostViewRepository : ICrud<PostView>
    {
        Task<IEnumerable<PostView>> GetPostViews(string postId);
        Task<PostView> GetPostViewByPostIdAsync(string postId);
    }
}
