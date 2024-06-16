

namespace SocialMedia.Repository.GenericCrudInterface
{
    public interface ICrud<T>
    {
        Task<T> AddAsync(T t);
        Task<T> UpdateAsync(T t);
        Task<T> DeleteByIdAsync(string id);
        Task<T> GetByIdAsync(string id);
        Task SaveChangesAsync();
        Task<IEnumerable<T>> GetAllAsync();
    }
}
