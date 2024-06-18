

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.GroupAccessRequestRepository;
using SocialMedia.Repository.GroupMemberRepository;
using SocialMedia.Repository.GroupPolicyRepository;
using SocialMedia.Repository.GroupRepository;
using SocialMedia.Repository.GroupRoleRepository;
using SocialMedia.Repository.PolicyRepository;

namespace SocialMedia.Repository.GroupAccessRequestService
{
    public class GroupAccessRequestService : IGroupAccessRequestService
    {
        private readonly IGroupAccessRequestRepository _groupAccessRequestRepository;
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IPolicyRepository _policyRepository;
        private readonly IGroupPolicyRepository _groupPolicyRepository;
        private readonly IGroupRoleRepository _groupRoleRepository;
        public GroupAccessRequestService(IGroupAccessRequestRepository _groupAccessRequestRepository,
            IGroupMemberRepository _groupMemberRepository, IGroupRepository _groupRepository,
            IPolicyRepository _policyRepository, IGroupPolicyRepository _groupPolicyRepository,
            IGroupRoleRepository _groupRoleRepository)
        {
            this._groupAccessRequestRepository = _groupAccessRequestRepository;
            this._groupMemberRepository = _groupMemberRepository;
            this._groupRepository = _groupRepository;
            this._policyRepository = _policyRepository;
            this._groupPolicyRepository = _groupPolicyRepository;
            this._groupRoleRepository = _groupRoleRepository;
            
        }

        public async Task<ApiResponse<GroupAccessRequest>> AddGroupAccessRequestAsync(
            AddGroupAccessRequestDto addGroupAccessRequestDto, SiteUser user)
        {

        }

        public Task<ApiResponse<GroupAccessRequest>> DeleteGroupAccessRequestAsync(
            string groupAccessRequestId, SiteUser user)
        {
            throw new NotImplementedException();
        }
    }
}
