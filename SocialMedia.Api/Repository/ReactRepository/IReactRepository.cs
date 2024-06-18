

using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.ReactRepository
{
    public interface IReactRepository : ICrud<React>
    {
        Task<React> GetReactByNameAsync(string reactName);
    }
}
