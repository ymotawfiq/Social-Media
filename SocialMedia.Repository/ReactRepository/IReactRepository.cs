

using SocialMedia.Data.Models;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.ReactRepository
{
    public interface IReactRepository : ICrud<React>
    {
        Task<React> GetReactByNameAsync(string reactName);
    }
}
