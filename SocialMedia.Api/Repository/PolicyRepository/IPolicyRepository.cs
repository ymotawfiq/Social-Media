

using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Repository.GenericCrudInterface;

namespace SocialMedia.Api.Repository.PolicyRepository
{
    public interface IPolicyRepository : ICrud<Policy>
    {
        Task<Policy> GetPolicyByNameAsync(string policyName);
        Task<bool> AddRangeAsync(List<string> policies);
    }
}
