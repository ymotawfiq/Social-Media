

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Extensions;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.ApiResponseModel;
using SocialMedia.Api.Data.Models.ApiResponseModel.ResponseObject;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.GroupMemberRepository;
using SocialMedia.Api.Repository.GroupPostsRepository;
using SocialMedia.Api.Repository.GroupRepository;
using SocialMedia.Api.Repository.PolicyRepository;
using SocialMedia.Api.Repository.PostRepository;
using SocialMedia.Api.Repository.PostViewRepository;
using SocialMedia.Api.Service.BlockService;
using SocialMedia.Api.Service.FriendsService;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.PolicyService;

namespace SocialMedia.Api.Service.PostService
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IPolicyRepository _policyRepository;
        private readonly IFriendService _friendService;
        private readonly IPolicyService _policyService;
        private readonly IGroupPostsRepository _groupPostsRepository;
        private readonly IPostViewRepository _postViewRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IGroupRepository _groupRepository;
        private readonly IBlockService _blockService;
        private readonly IGroupMemberRepository _groupMemberRepository;
        private readonly UserManagerReturn _userManagerReturn;

        public PostService(
            IPostRepository _postRepository, IGroupPostsRepository _groupPostsRepository,
            IPolicyRepository _policyRepository, IFriendService _friendService, IPolicyService _policyService
            , IGroupRepository _groupRepository, IGroupMemberRepository _groupMemberRepository,
            IPostViewRepository _postViewRepository, IWebHostEnvironment _webHostEnvironment,
            IBlockService _blockService, UserManagerReturn _userManagerReturn)
        {
            this._postRepository = _postRepository;
            this._policyRepository = _policyRepository;
            this._friendService = _friendService;
            this._policyService = _policyService;
            this._postViewRepository = _postViewRepository;
            this._webHostEnvironment = _webHostEnvironment;
            this._groupRepository = _groupRepository;
            this._blockService = _blockService;
            this._groupPostsRepository = _groupPostsRepository;
            this._groupMemberRepository = _groupMemberRepository;
            this._userManagerReturn = _userManagerReturn;
        }
        public async Task<ApiResponse<PostResponseObject>> AddPostAsync(SiteUser user, AddPostDto createPostDto)
        {
            var postsPolicy = await _policyRepository.GetByIdAsync(user.AccountPostPolicyId!);
            var reactPolicy = await _policyRepository.GetByIdAsync(user.ReactPolicyId!);
            var commentPolicy = await _policyRepository.GetByIdAsync(user.CommentPolicyId!);
            var post = ConvertFromDto.ConvertFromCreatePostDto_Add(createPostDto, postsPolicy, reactPolicy,
                commentPolicy, user);
            await _postViewRepository.AddAsync(new PostView
            {
                Id = Guid.NewGuid().ToString(),
                PostId = post.Id,
                ViewNumber = 0
            });
            var postImages = new List<PostImages>();
            if (createPostDto.Images != null)
            {
                foreach(var i in createPostDto.Images)
                {
                    postImages.Add(new PostImages
                    {
                        ImageUrl = SavePostImages(i),
                        PostId = post.Id,
                        Id = Guid.NewGuid().ToString()
                    });
                }
            }
            var newPostDto = await _postRepository.AddPostAsync(post, postImages);
            newPostDto.Post.User = _userManagerReturn.SetUserToReturn(user);
            return StatusCodeReturn<PostResponseObject>
                    ._201_Created("Post created successfully", newPostDto);
        }

        public async Task<ApiResponse<bool>> DeletePostAsync(SiteUser user, string postId)
        {
            var post = await _postRepository.GetPostWithImagesByPostIdAsync(postId);
            if (post != null)
            {
                if(post.Post.UserId == user.Id)
                {
                    if(post.Images != null && post.Images.Count > 0)
                    {
                        foreach(var i in post.Images)
                        {
                            DeletePostImage(i.ImageUrl);
                        }
                    }
                    await _postRepository.DeletePostAsync(postId);
                    return StatusCodeReturn<bool>
                    ._200_Success("Post deleted successfully");
                }
                return StatusCodeReturn<bool>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<bool>
                    ._404_NotFound("Post not found");
        }

        public async Task<ApiResponse<PostResponseObject>> GetPostByIdAsync(SiteUser user, string postId)
        {
            var existPost = await _postRepository.GetPostByIdAsync(postId);
            if (existPost != null)
            {
                if(!(await IsBlockedAsync(user.Id, existPost.UserId)).ResponseObject)
                {
                    return await CheckGroupPostAndGetPostAsync(postId, user);
                }
                return StatusCodeReturn<PostResponseObject>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<PostResponseObject>
                    ._404_NotFound("Post not found");
        }

        public async Task<ApiResponse<IEnumerable<PostResponseObject>>> GetUserPostsAsync(SiteUser user)
        {
            var posts = await _postRepository.GetUserPostsAsync(user);
            if (posts.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<PostResponseObject>>
                    ._200_Success("No posts found", posts);
            }
            return StatusCodeReturn<IEnumerable<PostResponseObject>>
                    ._200_Success("Posts found successfully", posts);
        }

        public async Task<ApiResponse<IEnumerable<PostResponseObject>>> GetUserPostsAsync(SiteUser user,
            SiteUser routeUser)
        {
            if (user.Id == routeUser.Id)
            {
                return await GetUserPostsAsync(user);
            }
            else if (!(await IsBlockedAsync(user.Id, routeUser.Id)).ResponseObject)
            {
                return await CheckFriendShipAndGetPostsAsync(user, routeUser); 
            }
            return StatusCodeReturn<IEnumerable<PostResponseObject>>
                                ._403_Forbidden();
        }

        public async Task<ApiResponse<IEnumerable<PostResponseObject>>> GetUserPostsByPolicyAsync(
            SiteUser user, Policy policy)
        {
            var checkPolicy = await _policyRepository.GetByIdAsync(policy.Id);
            if (checkPolicy == null)
            {
                return StatusCodeReturn<IEnumerable<PostResponseObject>>
                    ._404_NotFound("Policy not found");
            }
            var userPosts = await _postRepository.GetUserPostsByPolicyAsync(user, policy);
            if (userPosts.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<PostResponseObject>>
                    ._200_Success("No posts found");
            }
            return StatusCodeReturn<IEnumerable<PostResponseObject>>
                    ._200_Success("Posts found successfully", userPosts);
        }
        public async Task<ApiResponse<IEnumerable<PostResponseObject>>> GetPostsForFriendsAsync(SiteUser user)
        {
            var posts = await _postRepository.GetUserPostsForFriendsAsync(user);
            if (posts.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<PostResponseObject>>
                    ._200_Success("No posts found");
            }
            return StatusCodeReturn<IEnumerable<PostResponseObject>>
                    ._200_Success("Posts found successfully", posts);
        }

        public async Task<ApiResponse<IEnumerable<PostResponseObject>>> GetPostsForFriendsOfFriendsAsync(
            SiteUser user)
        {
            var posts = await _postRepository.GetUserPostsForFriendsOfFriendsAsync(user);
            if (posts.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<PostResponseObject>>
                    ._200_Success("No posts found");
            }
            return StatusCodeReturn<IEnumerable<PostResponseObject>>
                    ._200_Success("Posts found successfully", posts);
        }

        public async Task<ApiResponse<IEnumerable<PostResponseObject>>> CheckFriendShipAndGetPostsAsync(
            SiteUser currentUser, SiteUser routeUser)
        {
            if(!(await IsBlockedAsync(currentUser.Id, routeUser.Id)).ResponseObject)
            {
                var isFriend = await _friendService.IsUserFriendAsync(routeUser.Id, currentUser.Id);
                if (isFriend.ResponseObject)
                {
                    return await GetPostsForFriendsAsync(routeUser);
                }
                var isFriendOfFriend = await _friendService.IsUserFriendOfFriendAsync(routeUser.Id,
                    currentUser.Id);
                if (isFriendOfFriend.ResponseObject)
                {
                    return await GetPostsForFriendsOfFriendsAsync(routeUser);
                }
                var postPolicy = await _policyRepository.GetPolicyByNameAsync("public");
                var publicPosts = (await GetUserPostsByPolicyAsync(routeUser, postPolicy)).ResponseObject;
                if (publicPosts != null)
                {
                    return StatusCodeReturn<IEnumerable<PostResponseObject>>
                        ._200_Success("Posts found successfully", publicPosts.ToList());
                }
                return StatusCodeReturn<IEnumerable<PostResponseObject>>
                        ._404_NotFound("No posts found");
            }
            return StatusCodeReturn<IEnumerable<PostResponseObject>>
                    ._403_Forbidden();
        }


        public async Task<ApiResponse<PostResponseObject>> UpdatePostAsync(SiteUser user, UpdatePostDto updatePostDto)
        {
            var post = await _postRepository.GetPostWithImagesByPostIdAsync(updatePostDto.PostId);
            var postImages = new List<PostImages>();
            if (post != null)
            {
                if (post.Post.UserId == user.Id)
                {
                    if (updatePostDto.Images != null)
                    {
                        if (post.Images != null || post.Images!.Count > 0)
                        {
                            foreach (var p in post.Images)
                            {
                                DeletePostImage(p.ImageUrl);
                            }
                            await _postRepository.DeletePostImagesAsync(post.Post.Id);
                        }
                        foreach (var i in updatePostDto.Images)
                        {
                            var postImage = new PostImages
                            {
                                ImageUrl = SavePostImages(i),
                                PostId = updatePostDto.PostId,
                                Id = Guid.NewGuid().ToString()
                            };
                            postImages.Add(postImage);
                        }
                    }
                    post.Post.Content = updatePostDto.PostContent;
                    var updatedPost = await _postRepository.UpdatePostAsync(
                            ConvertFromPostDto(post), postImages);
                    //SetNull(updatedPost);
                    updatedPost.Post.User = _userManagerReturn.SetUserToReturn(user);
                    return StatusCodeReturn<PostResponseObject>
                            ._200_Success("Post updated successfully", updatedPost);
                }
                return StatusCodeReturn<PostResponseObject>
                    ._403_Forbidden();
            }
            return StatusCodeReturn<PostResponseObject>
            ._404_NotFound("Post not found");
        }

        public async Task<ApiResponse<bool>> UpdatePostPolicyAsync(SiteUser user,
            UpdatePostPolicyDto updatePostPolicyDto)
        {
            var post = await _postRepository.GetPostByIdAsync(updatePostPolicyDto.PostId);
            if (post != null)
            {
                if (post.UserId == user.Id)
                {
                    var policy = await _policyService.GetPolicyByIdOrNameAsync(
                        updatePostPolicyDto.PolicyIdOrName);
                    if (policy != null && policy.ResponseObject != null)
                    {
                        if(!(await IsGroupPostAsync(updatePostPolicyDto.PostId)).IsSuccess)
                        {
                            post.PostPolicyId = policy.ResponseObject.Id;
                            await _postRepository.UpdatePostAsync(post);
                            return StatusCodeReturn<bool>
                                ._200_Success("Post policy updated successfully", true);
                        }
                        return StatusCodeReturn<bool>
                        ._403_Forbidden("Group post policy must be public");
                    }
                    return StatusCodeReturn<bool>
                            ._404_NotFound("Policy not found", false);
                }
                return StatusCodeReturn<bool>
                            ._403_Forbidden();
            }
            return StatusCodeReturn<bool>
                            ._404_NotFound("Post not found", false);
        }

        public async Task<ApiResponse<bool>> UpdatePostReactPolicyAsync(SiteUser user,
            UpdatePostReactPolicyDto updatePostReactPolicy)
        {
            var post = await _postRepository.GetPostByIdAsync(updatePostReactPolicy.PostId);
            if (post != null)
            {
                if (post.UserId == user.Id)
                {
                    var policy = await _policyService.GetPolicyByIdOrNameAsync(
                        updatePostReactPolicy.PolicyIdOrName);
                    if (policy != null && policy.ResponseObject != null)
                    {
                        post.ReactPolicyId = policy.ResponseObject.Id;
                        await _postRepository.UpdatePostAsync(post);
                        return StatusCodeReturn<bool>
                            ._200_Success("Post react policy updated successfully", true);
                    }
                    return StatusCodeReturn<bool>
                            ._404_NotFound("Policy not found", false);
                }
                return StatusCodeReturn<bool>
                            ._403_Forbidden();
            }
            return StatusCodeReturn<bool>
                            ._404_NotFound("Post not found", false);
        }

        public async Task<ApiResponse<bool>> UpdatePostCommentPolicyAsync(
            SiteUser user, UpdatePostCommentPolicyDto updatePostCommentPolicyDto)
        {
            var post = await _postRepository.GetPostByIdAsync(updatePostCommentPolicyDto.PostId);
            if (post != null)
            {
                if (post.UserId == user.Id)
                {
                    var policy = await _policyService.GetPolicyByIdOrNameAsync(
                        updatePostCommentPolicyDto.PolicyIdOrName);
                    if (policy != null && policy.ResponseObject != null)
                    {
                        post.CommentPolicyId = policy.ResponseObject.Id;
                        await _postRepository.UpdatePostAsync(post);
                        return StatusCodeReturn<bool>
                            ._200_Success("Post comment policy updated successfully", true);
                    }
                    return StatusCodeReturn<bool>
                            ._404_NotFound("Policy not found", false);
                }
                return StatusCodeReturn<bool>
                            ._403_Forbidden();
            }
            return StatusCodeReturn<bool>
                            ._404_NotFound("Post not found", false);
        }

        public async Task<ApiResponse<bool>> UpdateUserPostsPolicyToLockedProfileAsync(SiteUser user)
        {
            await _postRepository.UpdateUserPostsPolicyToLockedAccountAsync(user.Id);
            return StatusCodeReturn<bool>
                ._200_Success("Posts policy updated successfully to locked account", true);
        }

        public async Task<ApiResponse<bool>> UpdateUserPostsPolicyToUnLockedProfileAsync(SiteUser user)
        {
            await _postRepository.UpdateUserPostsPolicyToUnLockedAccountAsync(user.Id);
            return StatusCodeReturn<bool>
                ._200_Success("Posts policy updated successfully to unlocked account", true);
        }

        private string SavePostImages(IFormFile image)
        {
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, @"wwwroot\Images\Post_Images");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + image.FileName;
            if (!System.IO.Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }
            string filePath = Path.Combine(path, uniqueFileName);
            using(var fileStream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(fileStream);
                fileStream.Flush();
            }
            return uniqueFileName;
        }

        private bool DeletePostImage(string imageUrl)
        {
            var path = Path.Combine(_webHostEnvironment.ContentRootPath, @"wwwroot\Images\Post_Images\");
            var file = Path.Combine(path, $"{imageUrl}");
            if (System.IO.File.Exists(file))
            {
                System.IO.File.Delete(file);
                return true;
            }
            return false;
        }

        private async Task<ApiResponse<bool>> CheckPostPolicyAsync(SiteUser user, Post post)
        {
            var postPolicy = await _policyRepository.GetByIdAsync(post.PostPolicyId);
            if (post.UserId != user.Id)
            {
                if (postPolicy.PolicyType == "PRIVATE")
                {
                    return StatusCodeReturn<bool>
                        ._403_Forbidden();
                }
                else if (postPolicy.PolicyType == "FRIENDS ONLY")
                {
                    var isFriend = await _friendService.IsUserFriendAsync(user.Id, post.UserId);
                    if (isFriend != null && !isFriend.ResponseObject)
                    {
                        return StatusCodeReturn<bool>
                        ._403_Forbidden();
                    }
                }
                else if (postPolicy.PolicyType == "FRIEND OF FRIEND")
                {
                    var isFriendOfFriend = await _friendService.IsUserFriendOfFriendAsync(user.Id,
                        post.UserId);
                    if (isFriendOfFriend != null && !isFriendOfFriend.ResponseObject)
                    {
                        return StatusCodeReturn<bool>
                        ._403_Forbidden();
                    }
                }
            }
            return StatusCodeReturn<bool>
                ._200_Success("Success", true);
        }

        private async Task<ApiResponse<bool>> IsBlockedAsync(string userId1, string userId2)
        {
            var isBlocked = await _blockService.GetBlockByUserIdAndBlockedUserIdAsync(userId1, userId2);
            if (isBlocked != null && isBlocked.ResponseObject != null && isBlocked.IsSuccess)
            {
                return StatusCodeReturn<bool>
                    ._200_Success("Blocked", true);
            }
            return StatusCodeReturn<bool>
                    ._200_Success("Not blocked", false);
        }

        private Post ConvertFromPostDto(PostResponseObject post)
        {
            return new Post
            {
                Id = post.Post.Id,
                UserId = post.Post.UserId,
                CommentPolicyId = post.Post.CommentPolicyId,
                Content = post.Post.Content,
                PostedAt = post.Post.PostedAt,
                PostPolicyId = post.Post.PostPolicyId,
                ReactPolicyId = post.Post.ReactPolicyId
            };
        }

        private async Task<ApiResponse<PostResponseObject>> CheckGroupPostAndGetPostAsync(string postId,
            SiteUser user)
        {
            var post = await _postRepository.GetPostWithImagesByPostIdAsync(postId);
            var groupPost = await IsGroupPostAsync(postId);
            if (groupPost != null && groupPost.ResponseObject != null)
            {
                var policy = await _policyService.GetPolicyByIdAsync((await _groupRepository
                    .GetByIdAsync(groupPost.ResponseObject.GroupId)).GroupPolicyId);
                if (policy != null && policy.ResponseObject != null)
                {
                    if (policy.ResponseObject.PolicyType == "PUBLIC")
                    {
                        return await IncreasePostViewsAndGetPostAsync(postId);
                    }
                    else if (policy.ResponseObject.PolicyType == "PRIVATE")
                    {
                        if ((await IsMemberGroupAsync(groupPost.ResponseObject.GroupId, user.Id)).IsSuccess)
                        {
                            return await IncreasePostViewsAndGetPostAsync(postId);
                        }
                        return StatusCodeReturn<PostResponseObject>
                            ._403_Forbidden("You must join group to view post");
                    }
                }
                return StatusCodeReturn<PostResponseObject>
                            ._404_NotFound("Policy not found");
            }
            if((await CheckPostPolicyAsync(user, post.Post)).IsSuccess)
            {
                return await IncreasePostViewsAndGetPostAsync(postId);
            }
            return StatusCodeReturn<PostResponseObject>
                ._403_Forbidden((await CheckPostPolicyAsync(user, post.Post)).Message);
        }

        private async Task<ApiResponse<PostResponseObject>> IncreasePostViewsAndGetPostAsync(string postId)
        {
            var post = await _postRepository.GetPostWithImagesByPostIdAsync(postId);
            if (post != null)
            {
                var postView = await _postViewRepository.GetPostViewByPostIdAsync(post.Post.Id);
                postView.ViewNumber = ++postView.ViewNumber;
                await _postViewRepository.UpdateAsync(postView);
                post.Post.User = _userManagerReturn.SetUserToReturn(await _userManagerReturn
                    .GetUserByUserNameOrEmailOrIdAsync(post.Post.UserId));
                return StatusCodeReturn<PostResponseObject>
                    ._200_Success("Post found successfully", post);
            }
            return StatusCodeReturn<PostResponseObject>
                ._404_NotFound("Post not found");
        }

        private async Task<ApiResponse<bool>> IsMemberGroupAsync(string groupId, string userId)
        {
            var isMember = await _groupMemberRepository.GetGroupMemberAsync(
                        userId, groupId);
            if (isMember != null)
            {
                return StatusCodeReturn<bool>
                    ._200_Success("Group member", true);
            }
            return StatusCodeReturn<bool>
                ._404_NotFound("Not group member", false);
        }

        private async Task<ApiResponse<GroupPost>> IsGroupPostAsync(string postId)
        {
            var groupPost = await _groupPostsRepository.GetGroupPostByPostIdAsync(postId);
            if (groupPost != null)
            {
                return StatusCodeReturn<GroupPost>
                    ._200_Success("Group post", groupPost);
            }
            return StatusCodeReturn<GroupPost>
                ._404_NotFound("Not group post");
        }

        
    }
}
