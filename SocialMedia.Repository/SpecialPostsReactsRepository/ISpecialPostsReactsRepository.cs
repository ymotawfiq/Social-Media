

using SocialMedia.Data.Models;

namespace SocialMedia.Repository.SpecialPostsReactsRepository
{
    public interface ISpecialPostsReactsRepository
    {
        Task<SpecialPostReacts> AddSpecialPostReactsAsync(SpecialPostReacts specialPostReacts);
        Task<SpecialPostReacts> UpdateSpecialPostReactsAsync(SpecialPostReacts specialPostReacts);
        Task<SpecialPostReacts> GetSpecialPostReactsByIdAsync(string Id);
        Task<SpecialPostReacts> GetSpecialPostReactsByReactIdAsync(string reactId);
        Task<SpecialPostReacts> DeleteSpecialPostReactsByIdAsync(string Id);
        Task<SpecialPostReacts> DeleteSpecialPostReactsByReactIdAsync(string reactId);
        Task<IEnumerable<SpecialPostReacts>> GetSpecialPostReactsAsync();
        Task SaveChangesAsync();
    }
}
