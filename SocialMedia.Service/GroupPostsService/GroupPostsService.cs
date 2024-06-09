

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.GroupMemberRepository;
using SocialMedia.Repository.GroupPolicyRepository;
using SocialMedia.Repository.GroupPostsRepository;
using SocialMedia.Repository.GroupRepository;
using SocialMedia.Repository.PostRepository;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.PolicyService;
using SocialMedia.Service.PostService;
using System.Text.RegularExpressions;

namespace SocialMedia.Service.GroupPostsService
{
    public class GroupPostsService : IGroupPostsService
    {
        private readonly IGroupPostsRepository _groupPostsRepository;
        private readonly IPostService _postService;
        private readonly IGroupRepository _groupRepository;
        private readonly IPolicyService _policyService;
        private readonly IGroupPolicyRepository _groupPolicyRepository;
        private readonly IGroupMemberRepository _groupMemberRepository;
        public GroupPostsService(IGroupPostsRepository _groupPostsRepository, IPostService _postService,
            IGroupRepository _groupRepository, IPolicyService _policyService,
            IGroupPolicyRepository _groupPolicyRepository, IGroupMemberRepository _groupMemberRepository)
        {
            this._groupPostsRepository = _groupPostsRepository;
            this._groupRepository = _groupRepository;
            this._postService = _postService;
            this._groupPolicyRepository = _groupPolicyRepository;
            this._policyService = _policyService;
            this._groupMemberRepository = _groupMemberRepository;
        }
        public async Task<object> AddGroupPostAsync(
            AddGroupPostDto addGroupPostDto, SiteUser user)
        {
            var group = await _groupRepository.GetGroupByIdAsync(addGroupPostDto.GroupId);
            if (group != null)
            {
                var newPost = await _postService.AddPostAsync(user, new AddPostDto
                {
                    Images = addGroupPostDto.Images,
                    PostContent = addGroupPostDto.PostContent
                });
                if (newPost.ResponseObject != null)
                {
                    var groupPost = await _groupPostsRepository.AddGroupPostAsync(new GroupPost
                    {
                        GroupId = addGroupPostDto.GroupId,
                        Id = Guid.NewGuid().ToString(),
                        PostId = newPost.ResponseObject!.PostId,
                        UserId = user.Id
                    });
                    SetNull(groupPost);
                    return StatusCodeReturn<object>
                        ._201_Created("Post saved successfully", groupPost);
                }
                return newPost;
            }
            return StatusCodeReturn<object>
                ._404_NotFound("Group not found");
        }

        public async Task<ApiResponse<GroupPost>> DeleteGroupPostByIdAsync(string groupPostId, SiteUser user)
        {
            var groupPost = await _groupPostsRepository.GetGroupPostByIdAsync(groupPostId);
            if (groupPost != null)
            {
                if(groupPost.UserId == user.Id)
                {
                    await _postService.DeletePostAsync(user, groupPost.PostId);
                    SetNull(groupPost);
                    return StatusCodeReturn<GroupPost>
                        ._200_Success("Post deleted successfully", groupPost);
                }
                return StatusCodeReturn<GroupPost>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<GroupPost>
                    ._404_NotFound("Post not found");
        }

        public async Task<ApiResponse<GroupPost>> GetGroupPostByIdAsync(string groupPostId)
        {
            var groupPost = await _groupPostsRepository.GetGroupPostByIdAsync(groupPostId);
            if (groupPost != null)
            {
                var policy = await _policyService.GetPolicyByIdAsync((await _groupPolicyRepository
                    .GetGroupPolicyByIdAsync((await _groupRepository
                    .GetGroupByIdAsync(groupPost.GroupId)).GroupPolicyId)).PolicyId);
                if (policy != null && policy.ResponseObject != null)
                {
                    if (policy.ResponseObject.PolicyType == "PUBLIC")
                    {
                        SetNull(groupPost);
                        return StatusCodeReturn<GroupPost>
                            ._200_Success("Post found successfully", groupPost);
                    }
                    return StatusCodeReturn<GroupPost>
                    ._403_Forbidden();
                }
                return StatusCodeReturn<GroupPost>
                    ._404_NotFound("Policy not found");
            }
            return StatusCodeReturn<GroupPost>
                    ._404_NotFound("Post not found");
        }


        public async Task<ApiResponse<GroupPost>> GetGroupPostByIdAsync(string groupPostId, SiteUser user)
        {
            var groupPost = await _groupPostsRepository.GetGroupPostByIdAsync(groupPostId);
            if (groupPost != null)
            {
                var policy = await _policyService.GetPolicyByIdAsync((await _groupPolicyRepository
                    .GetGroupPolicyByIdAsync((await _groupRepository
                    .GetGroupByIdAsync(groupPost.GroupId)).GroupPolicyId)).PolicyId);
                if (policy != null && policy.ResponseObject != null)
                {
                    if (policy.ResponseObject.PolicyType == "PUBLIC")
                    {
                        SetNull(groupPost);
                        return StatusCodeReturn<GroupPost>
                            ._200_Success("Post found successfully", groupPost);
                    }
                    else if(policy.ResponseObject.PolicyType == "PRIVATE")
                    {
                        var isMember = await _groupMemberRepository.GetGroupMemberAsync(
                        user.Id, groupPost.GroupId);
                        if (isMember != null)
                        {
                            SetNull(groupPost);
                            return StatusCodeReturn<GroupPost>
                            ._200_Success("Post found successfully", groupPost);
                        }
                        return StatusCodeReturn<GroupPost>
                            ._403_Forbidden();
                    }
                }
                return StatusCodeReturn<GroupPost>
                            ._404_NotFound("Policy not found");
            }
            return StatusCodeReturn<GroupPost>
                    ._404_NotFound("Post not found");
        }

        public async Task<ApiResponse<IEnumerable<GroupPost>>> GetGroupPostsAsync(
            string groupId, SiteUser user)
        {
            var group = await _groupRepository.GetGroupByIdAsync(groupId);
            if (group != null)
            {
                var policy = await _policyService.GetPolicyByIdAsync((await _groupPolicyRepository
                    .GetGroupPolicyByIdAsync(group.GroupPolicyId)).PolicyId);
                if (policy != null && policy.ResponseObject != null)
                {
                    return await CheckPolicyAndGetPostsAsync(policy, groupId, user);
                }
                return StatusCodeReturn<IEnumerable<GroupPost>>
                            ._404_NotFound("Policy not found");
            }
            return StatusCodeReturn<IEnumerable<GroupPost>>
                            ._404_NotFound("Group not found");
        }

        public async Task<ApiResponse<IEnumerable<GroupPost>>> GetGroupPostsAsync(string groupId)
        {
            var group = await _groupRepository.GetGroupByIdAsync(groupId);
            if (group != null)
            {
                var policy = await _policyService.GetPolicyByIdAsync((await _groupPolicyRepository
                    .GetGroupPolicyByIdAsync(group.GroupPolicyId)).PolicyId);
                if (policy != null && policy.ResponseObject != null)
                {
                    if(policy.ResponseObject.PolicyType == "PUBLIC")
                    {
                        return StatusCodeReturn<IEnumerable<GroupPost>>
                            ._200_Success("Posts found successfully",
                            await _groupPostsRepository.GetGroupPostsAsync(groupId));
                    }
                    return StatusCodeReturn<IEnumerable<GroupPost>>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<IEnumerable<GroupPost>>
                            ._404_NotFound("Policy not found");
            }
            return StatusCodeReturn<IEnumerable<GroupPost>>
                            ._404_NotFound("Group not found");
        }
        private async Task<ApiResponse<IEnumerable<GroupPost>>> GetPostsAsync(string groupId)
        {
            var posts = await _groupPostsRepository.GetGroupPostsAsync(groupId);
            foreach(var p in posts)
            {
                SetNull(p);
            }
            if (posts.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<GroupPost>>
                    ._200_Success("No posts found", posts);
            }
            return StatusCodeReturn<IEnumerable<GroupPost>>
                    ._200_Success("Posts found successfully", posts);
        }

        private async Task<ApiResponse<IEnumerable<GroupPost>>> CheckPolicyAndGetPostsAsync(
            ApiResponse<Policy> policy, string groupId, SiteUser user)
        {
            if (policy.ResponseObject!.PolicyType == "PUBLIC")
            {
                return await GetPostsAsync(groupId);
            }
            else if (policy.ResponseObject.PolicyType == "PRIVATE")
            {
                var isMember = await _groupMemberRepository.GetGroupMemberAsync(user.Id, groupId);
                if (isMember != null)
                {
                    return await GetPostsAsync(groupId);
                }
                return StatusCodeReturn<IEnumerable<GroupPost>>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<IEnumerable<GroupPost>>
                    ._403_Forbidden();
        }

        private void SetNull(GroupPost groupPost)
        {
            groupPost.Group = null;
            groupPost.Post = null;
            groupPost.User = null;
        }

        
    }
}
