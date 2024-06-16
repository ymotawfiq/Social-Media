

using SocialMedia.Data.Models;
using SocialMedia.Repository.GenericCrudInterface;

namespace SocialMedia.Repository.PolicyRepository
{
    public interface IPolicyRepository : ICrud<Policy>
    {
        Task<Policy> GetPolicyByNameAsync(string policyName);
    }
}
