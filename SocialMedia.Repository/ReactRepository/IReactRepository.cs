

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.ReactRepository
{
    public interface IReactRepository
    {
        Task<React> AddReactAsync(React react);
        Task<React> UpdateReactAsync(React react);
        Task<React> DeleteReactByIdAsync(Guid reactId);
        Task<React> GetReactByIdAsync(Guid reactId);
        Task<IEnumerable<React>> GetAllReactsAsync();
        Task SaveChangesAsync();
    }
}
