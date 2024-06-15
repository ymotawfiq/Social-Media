

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.GroupMemberRepository;
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
        private readonly IGroupMemberRepository _groupMemberRepository;
        public GroupPostsService(IGroupPostsRepository _groupPostsRepository, IPostService _postService,
            IGroupRepository _groupRepository, IPolicyService _policyService,
            IGroupMemberRepository _groupMemberRepository)
        {
            this._groupPostsRepository = _groupPostsRepository;
            this._groupRepository = _groupRepository;
            this._postService = _postService;
            this._policyService = _policyService;
            this._groupMemberRepository = _groupMemberRepository;
        }
        public async Task<object> AddGroupPostAsync(AddGroupPostDto addGroupPostDto, SiteUser user)
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
                        PostId = newPost.ResponseObject!.Post.Id,
                        UserId = user.Id
                    });
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
                var policy = await _policyService.GetPolicyByIdAsync((await _groupRepository
                    .GetGroupByIdAsync(groupPost.GroupId)).GroupPolicyId);
                if (policy != null && policy.ResponseObject != null)
                {
                    if (policy.ResponseObject.PolicyType == "PUBLIC")
                    {
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
                var policy = await _policyService.GetPolicyByIdAsync((await _groupRepository
                    .GetGroupByIdAsync(groupPost.GroupId)).GroupPolicyId);
                if (policy != null && policy.ResponseObject != null)
                {
                    if (policy.ResponseObject.PolicyType == "PUBLIC")
                    {
                        return StatusCodeReturn<GroupPost>
                            ._200_Success("Post found successfully", groupPost);
                    }
                    else if(policy.ResponseObject.PolicyType == "PRIVATE")
                    {
                        var isMember = await _groupMemberRepository.GetGroupMemberAsync(
                        user.Id, groupPost.GroupId);
                        if (isMember != null)
                        {
                            return StatusCodeReturn<GroupPost>
                            ._200_Success("Post found successfully", groupPost);
                        }
                        return StatusCodeReturn<GroupPost>
                            ._403_Forbidden("You must join group to view post");
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
                var policy = await _policyService.GetPolicyByIdAsync(group.GroupPolicyId);
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
                var policy = await _policyService.GetPolicyByIdAsync(group.GroupPolicyId);
                if (policy != null && policy.ResponseObject != null)
                {
                    if(policy.ResponseObject.PolicyType == "PUBLIC")
                    {
                        return StatusCodeReturn<IEnumerable<GroupPost>>
                            ._200_Success("Posts found successfully",
                            await _groupPostsRepository.GetGroupPostsAsync(groupId));
                    }
                    return StatusCodeReturn<IEnumerable<GroupPost>>
                        ._403_Forbidden("You must join group to get posts");
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
                    ._403_Forbidden("You must join group to get posts");
            }
            return StatusCodeReturn<IEnumerable<GroupPost>>
                    ._403_Forbidden();
        }

        public async Task<ApiResponse<GroupPost>> GetGroupPostByPostIdAsync(string postId, SiteUser user)
        {
            var groupPost = await _groupPostsRepository.GetGroupPostByPostIdAsync(postId);
            if (groupPost != null)
            {
                return await GetGroupPostByIdAsync(groupPost.Id, user);
            }
            return StatusCodeReturn<GroupPost>
                    ._404_NotFound("Post not found");
        }
    }
}
