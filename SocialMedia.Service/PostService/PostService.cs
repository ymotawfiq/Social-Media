

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.AccountPolicyRepository;
using SocialMedia.Repository.AccountPostsPolicyRepository;
using SocialMedia.Repository.CommentPolicyRepository;
using SocialMedia.Repository.FriendsRepository;
using SocialMedia.Repository.GroupPolicyRepository;
using SocialMedia.Repository.GroupPostsRepository;
using SocialMedia.Repository.GroupRepository;
using SocialMedia.Repository.PolicyRepository;
using SocialMedia.Repository.PostRepository;
using SocialMedia.Repository.PostViewRepository;
using SocialMedia.Repository.ReactPolicyRepository;
using SocialMedia.Repository.UserPostsRepository;
using SocialMedia.Service.FriendsService;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.PolicyService;

namespace SocialMedia.Service.PostService
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ICommentPolicyRepository _commentPolicyRepository;
        private readonly IReactPolicyRepository _reactPolicyRepository;
        private readonly IPolicyRepository _policyRepository;
        private readonly IFriendsRepository _friendsRepository;
        private readonly IFriendService _friendService;
        private readonly IPolicyService _policyService;
        private readonly IUserPostsRepository _userPostsRepository;
        private readonly IAccountPostsPolicyRepository _accountPostsPolicyRepository;
        private readonly IPostViewRepository _postViewRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IGroupPolicyRepository _groupPolicyRepository;
        private readonly IGroupPostsRepository _groupPostsRepository;
        private readonly IGroupRepository _groupRepository;
        

        public PostService(IPostRepository _postRepository,
             ICommentPolicyRepository _commentPolicyRepository,
            IReactPolicyRepository _reactPolicyRepository, IPolicyRepository _policyRepository,
            IFriendsRepository _friendsRepository, IFriendService _friendService,
            IUserPostsRepository _userPostsRepository, IPolicyService _policyService,
            IAccountPostsPolicyRepository _accountPostsPolicyRepository, IGroupRepository _groupRepository,
            IPostViewRepository _postViewRepository, IWebHostEnvironment _webHostEnvironment,
            IGroupPolicyRepository _groupPolicyRepository, IGroupPostsRepository _groupPostsRepository)
        {
            this._postRepository = _postRepository;
            this._commentPolicyRepository = _commentPolicyRepository;
            this._policyRepository = _policyRepository;
            this._reactPolicyRepository = _reactPolicyRepository;
            this._friendsRepository = _friendsRepository;
            this._friendService = _friendService;
            this._userPostsRepository = _userPostsRepository;
            this._policyService = _policyService;
            this._accountPostsPolicyRepository = _accountPostsPolicyRepository;
            this._postViewRepository = _postViewRepository;
            this._webHostEnvironment = _webHostEnvironment;
            this._groupPostsRepository = _groupPostsRepository;
            this._groupPolicyRepository = _groupPolicyRepository;
            this._groupRepository = _groupRepository;
        }
        public async Task<ApiResponse<PostDto>> AddPostAsync(SiteUser user, AddPostDto createPostDto)
        {
            var accountPostsPolicy = await _accountPostsPolicyRepository
                .GetAccountPostPolicyByIdAsync(user.AccountPostPolicyId!);
            var reactPolicy = await _reactPolicyRepository.GetReactPolicyByIdAsync(user.ReactPolicyId!);
            var commentPolicy = await _commentPolicyRepository.GetCommentPolicyByIdAsync
                (user.CommentPolicyId!);
            var policy = await _policyRepository.GetPolicyByIdAsync(accountPostsPolicy.PolicyId);

            if (policy == null)
            {
                policy = await _policyRepository.GetPolicyByNameAsync("public");
            }
            if (reactPolicy == null)
            {
                reactPolicy = await _reactPolicyRepository.GetReactPolicyByPolicyIdAsync(policy.Id);
            }
            if (commentPolicy == null)
            {
                commentPolicy = await _commentPolicyRepository.GetCommentPolicyByPolicyIdAsync(policy.Id);
            }
            var post = ConvertFromDto.ConvertFromCreatePostDto_Add(createPostDto, policy, reactPolicy,
                commentPolicy);
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
            var newPostDto = await _postRepository.AddPostAsync(user, post, postImages);
            var userPosts = await _userPostsRepository.GetUserPostByPostIdAsync(post.Id);
            //userPosts.User = null;
            return StatusCodeReturn<PostDto>
                    ._201_Created("Post created successfully", newPostDto);
        }

        public async Task<ApiResponse<PostDto>> DeletePostAsync(SiteUser user, string postId)
        {
            var existPost = await _postRepository.IsPostExistsAsync(postId);
            if (existPost == null)
            {
                return StatusCodeReturn<PostDto>
                    ._404_NotFound("Post not found");
            }
            var post = await _postRepository.GetPostByIdAsync(user, postId);
            if(post.Images!=null && post.Images.Count != 0)
            {
                foreach(var i in post.Images)
                {
                    DeletePostImage(i.ImageUrl);
                }
            }
            await _postRepository.DeletePostAsync(user, postId);
            return StatusCodeReturn<PostDto>
                    ._200_Success("Post deleted successfully");
        }

        public async Task<ApiResponse<PostDto>> GetPostByIdAsync(SiteUser user, string postId)
        {
            var existPost = await _postRepository.IsPostExistsAsync(postId);
            if (existPost == null)
            {
                return StatusCodeReturn<PostDto>
                    ._404_NotFound("Post not found");
            }
            var post = await _postRepository.GetPostByIdAsync(user, postId);
            return StatusCodeReturn<PostDto>
                    ._200_Success("Post found successfully", post);
        }

        public async Task<ApiResponse<IEnumerable<PostDto>>> GetUserPostsAsync(SiteUser user)
        {
            var posts = await _postRepository.GetUserPostsAsync(user);
            if (posts.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<PostDto>>
                    ._200_Success("No posts found", posts);
            }
            return StatusCodeReturn<IEnumerable<PostDto>>
                    ._200_Success("Posts found successfully", posts);
        }


        public async Task<ApiResponse<IEnumerable<PostDto>>> GetUserPostsByPolicyAsync(
            SiteUser user, Policy policy)
        {
            var checkPolicy = await _policyRepository.GetPolicyByNameAsync(policy.PolicyType);
            if (checkPolicy == null)
            {
                return StatusCodeReturn<IEnumerable<PostDto>>
                    ._404_NotFound("Policy not found");
            }
            var userPosts = await _postRepository.GetUserPostsByPolicyAsync(user, policy);
            if (userPosts.ToList().Count == 0)
            {
                return StatusCodeReturn<IEnumerable<PostDto>>
                    ._200_Success("No posts found");
            }
            return StatusCodeReturn<IEnumerable<PostDto>>
                    ._200_Success("Posts found successfully", userPosts);
        }


        public async Task<ApiResponse<IEnumerable<List<PostDto>>>> GetPostsForFriendsAsync(SiteUser user)
        {
            var publicPolicy = await _policyRepository.GetPolicyByNameAsync("public");
            var publicPosts = await GetUserPostsByPolicyAsync(user, publicPolicy);
            var friendsPolicy = await _policyRepository.GetPolicyByNameAsync("friends only");
            var friendsPosts = await GetUserPostsByPolicyAsync(user, friendsPolicy);
            var friendsOfFriendsPolicy = await _policyRepository
                .GetPolicyByNameAsync("friends of friends");
            var friendsOfFriendsPosts = await GetUserPostsByPolicyAsync(user, friendsOfFriendsPolicy);
            var posts = new List<List<PostDto>>();
            if (publicPosts.ResponseObject != null)
            {
                posts.Add(publicPosts.ResponseObject.ToList());
            }
            if (friendsPosts.ResponseObject != null)
            {
                posts.Add(friendsPosts.ResponseObject.ToList());
            }
            if (posts.Count == 0)
            {
                return StatusCodeReturn<IEnumerable<List<PostDto>>>
                    ._200_Success("No posts found");
            }
            return StatusCodeReturn<IEnumerable<List<PostDto>>>
                    ._200_Success("Posts found successfully", posts);
        }

        public async Task<ApiResponse<IEnumerable<List<PostDto>>>> GetPostsForFriendsOfFriendsAsync(
            SiteUser user)
        {
            var publicPolicy = await _policyRepository.GetPolicyByNameAsync("public");
            var publicPosts = await GetUserPostsByPolicyAsync(user, publicPolicy);
            
            var friendsOfFriendsPolicy = await _policyRepository
                .GetPolicyByNameAsync("friends of friends");
            var friendsOfFriendsPosts = await GetUserPostsByPolicyAsync(user, friendsOfFriendsPolicy);
            var posts = new List<List<PostDto>>();
            if (publicPosts.ResponseObject != null)
            {
                posts.Add(publicPosts.ResponseObject.ToList());
            }
            if (friendsOfFriendsPosts.ResponseObject != null)
            {
                posts.Add(friendsOfFriendsPosts.ResponseObject.ToList());
            }
            if (posts.Count == 0)
            {
                return StatusCodeReturn<IEnumerable<List<PostDto>>>
                    ._200_Success("No posts found");
            }
            return StatusCodeReturn<IEnumerable<List<PostDto>>>
                    ._200_Success("Posts found successfully", posts);
        }


        public async Task<ApiResponse<IEnumerable<List<PostDto>>>> CheckFriendShipAndGetPostsAsync(
            SiteUser currentUser, SiteUser routeUser)
        {
            var isFriend = await _friendService.IsUserFriendAsync(routeUser.Id, currentUser.Id);
            if (isFriend.ResponseObject)
            {
                return await GetPostsForFriendsAsync(routeUser);
            }
            var isFriendOfFriend = await _friendService.IsUserFriendOfFriendAsync(routeUser.Id, currentUser.Id);
            if (isFriendOfFriend.ResponseObject)
            {
                return await GetPostsForFriendsOfFriendsAsync(routeUser);
            }
            var policy = await _policyRepository.GetPolicyByNameAsync("public");
            var publicPosts = (await GetUserPostsByPolicyAsync(routeUser, policy)).ResponseObject;
            if (publicPosts != null)
            {
                var posts = new List<List<PostDto>>();
                posts.Add(publicPosts.ToList());
                return StatusCodeReturn<IEnumerable<List<PostDto>>>
                    ._200_Success("Posts found successfully", posts);
            }
            return StatusCodeReturn<IEnumerable<List<PostDto>>>
                    ._404_NotFound("No posts found");
        }


        public async Task<ApiResponse<PostDto>> UpdatePostAsync(
            SiteUser user, UpdatePostDto updatePostDto)
        {
            var userPost = await _userPostsRepository
                .GetUserPostByUserAndPostIdAsync(user.Id, updatePostDto.PostId);
            if (userPost != null)
            {
                var postDto = await _postRepository.GetPostByIdAsync(user, updatePostDto.PostId);
                var postImages = new List<PostImages>();
                if (postDto != null)
                {
                    if (updatePostDto.Images != null)
                    {
                        if (postDto.Images != null)
                        {
                            foreach (var p in postDto.Images)
                            {
                                DeletePostImage(p.ImageUrl);
                            }
                        }
                        foreach (var i in updatePostDto.Images)
                        {
                            postImages.Add(new PostImages
                            {
                                ImageUrl = SavePostImages(i),
                                PostId = updatePostDto.PostId,
                                Id = Guid.NewGuid().ToString()
                            });
                        }
                    }
                    var oldPost = await _postRepository.IsPostExistsAsync(postDto.PostId);
                    var post = ConvertFromDto.ConvertFromPostDto_Update(updatePostDto,
                        postDto, oldPost);
                    var updatedPost = await _postRepository.UpdatePostAsync(user, post, postImages);
                    var userPosts = await _userPostsRepository.GetUserPostByPostIdAsync(post.Id);
                    userPosts.User = null;
                    return StatusCodeReturn<PostDto>
                        ._200_Success("Post updated successfully", updatedPost);
                }
                return StatusCodeReturn<PostDto>
                ._404_NotFound("Post not found");
            }
            return StatusCodeReturn<PostDto>
                ._403_Forbidden();
        }


        public async Task<ApiResponse<PostDto>> GetPostByIdAsync(SiteUser currentUser
            , SiteUser routeUser, string postId)
        {
            var post = await _postRepository.GetPostByIdAsync(routeUser, postId);
            if (post != null)
            {
                var userPosts = await _userPostsRepository.GetUserPostByPostIdAsync(postId);
                userPosts.User = null;
                var postPolicy = await _policyService.GetPolicyByIdAsync(post.PolicyId);
                if (postPolicy.ResponseObject != null)
                {
                    if (postPolicy.ResponseObject.PolicyType == "PRIVATE")
                    {
                        var userPost = await _userPostsRepository.GetUserPostByUserAndPostIdAsync(
                            currentUser.Id, postId);
                        if (userPost == null)
                        {
                            return StatusCodeReturn<PostDto>._403_Forbidden();
                        }
                    }
                    else if (postPolicy.ResponseObject.PolicyType == "FRIENDS ONLY")
                    {
                        var isFriend = await _friendsRepository.GetFriendByUserAndFriendIdAsync(
                            currentUser.Id, routeUser.Id);
                        if (isFriend == null)
                        {
                            return StatusCodeReturn<PostDto>._403_Forbidden();
                        }
                    }
                    else if (postPolicy.ResponseObject.PolicyType == "FRIENDS OF FRIENDS")
                    {
                        var isFriend = await _friendsRepository.GetFriendByUserAndFriendIdAsync(
                            currentUser.Id, routeUser.Id);
                        var isFriendOfFriend = await _friendService.IsUserFriendOfFriendAsync(routeUser.Id,
                            currentUser.Id);
                        if (isFriend == null || !isFriendOfFriend.ResponseObject)
                        {
                            return StatusCodeReturn<PostDto>._403_Forbidden();
                        }
                    }
                    var postView = await _postViewRepository.GetPostViewByPostIdAsync(postId);
                    if (postView == null)
                    {
                        await _postViewRepository.AddPostViewAsync(
                            new PostView
                            {
                                Id = Guid.NewGuid().ToString(),
                                PostId = postId,
                                ViewNumber = 1
                            }
                            );
                        return StatusCodeReturn<PostDto>
                        ._200_Success("Post found successfully", post);
                    }
                    await _postViewRepository.UpdatePostViewAsync(postView);
                    return StatusCodeReturn<PostDto>
                        ._200_Success("Post found successfully", post);
                }
                return StatusCodeReturn<PostDto>
                ._404_NotFound("Post policy not found");
            }
            return StatusCodeReturn<PostDto>
                ._404_NotFound("Post not found");
        }



        public async Task<ApiResponse<bool>> UpdatePostPolicyAsync(
            SiteUser user, UpdatePostPolicyDto updatePostPolicyDto)
        {
            var userPost = await _userPostsRepository.GetUserPostByUserAndPostIdAsync(user.Id,
                updatePostPolicyDto.PostId);
            if (userPost != null)
            {
                var policy = await _policyService.GetPolicyByIdOrNameAsync(updatePostPolicyDto.PolicyIdOrName);
                if (policy.ResponseObject != null)
                {
                    var canModify = await CanModifyPostPolicyAsync(userPost, policy.ResponseObject.Id);
                    if (canModify.ResponseObject)
                    {
                        var post = await _postRepository.GetPostByIdAsync(user, updatePostPolicyDto.PostId);
                        post.PolicyId = policy.ResponseObject.Id;
                        await _postRepository.UpdatePostPolicyAsync(user, ConvertFromPostDto(post));
                        return StatusCodeReturn<bool>
                            ._200_Success("Post policy updated successfully", true);
                    }
                    return canModify;
                }
                return StatusCodeReturn<bool>
                ._404_NotFound("Policy not found");
            }
            return StatusCodeReturn<bool>
                ._404_NotFound("Post not found for this user");
        }

        public async Task<ApiResponse<bool>> UpdatePostReactPolicyAsync(
            SiteUser user, UpdatePostReactPolicyDto updatePostReactPolicy)
        {
            var userPost = await _userPostsRepository.GetUserPostByUserAndPostIdAsync(user.Id,
                updatePostReactPolicy.PostId);
            if (userPost != null)
            {
                var policy = await _policyService.GetPolicyByIdOrNameAsync(updatePostReactPolicy.PolicyIdOrName);
                if (policy.ResponseObject != null)
                {
                    var reactPolicy = await _reactPolicyRepository.GetReactPolicyByPolicyIdAsync(
                        policy.ResponseObject.Id);
                    if (reactPolicy != null)
                    {
                        var post = await _postRepository.GetPostByIdAsync(user, updatePostReactPolicy.PostId);
                        post.ReactPolicyId = reactPolicy.Id;
                        await _postRepository.UpdatePostReactPolicyAsync(user, ConvertFromPostDto(post));
                        return StatusCodeReturn<bool>
                        ._200_Success("Post react policy updated successfully", true);
                    }
                    return StatusCodeReturn<bool>
                ._404_NotFound("React policy not found");
                }
                return StatusCodeReturn<bool>
                ._404_NotFound("Policy not found");
            }
            return StatusCodeReturn<bool>
                ._404_NotFound("Post not found for this user");
        }

        public async Task<ApiResponse<bool>> UpdatePostCommentPolicyAsync(
            SiteUser user, UpdatePostCommentPolicyDto updatePostCommentPolicyDto)
        {
            var userPost = await _userPostsRepository.GetUserPostByUserAndPostIdAsync(user.Id,
                updatePostCommentPolicyDto.PostId);
            if (userPost != null)
            {
                var policy = await _policyService.GetPolicyByIdOrNameAsync(
                    updatePostCommentPolicyDto.PolicyIdOrName);
                if (policy.ResponseObject != null)
                {
                    var commentPolicy = await _commentPolicyRepository.GetCommentPolicyByPolicyIdAsync(
                        policy.ResponseObject.Id);
                    if (commentPolicy != null)
                    {
                        var post = await _postRepository.GetPostByIdAsync(user, updatePostCommentPolicyDto.PostId);
                        post.CommentPolicyId = commentPolicy.Id;
                        await _postRepository.UpdatePostCommentPolicyAsync(user, ConvertFromPostDto(post));
                        return StatusCodeReturn<bool>
                        ._200_Success("Post comment policy updated successfully", true);
                    }
                    return StatusCodeReturn<bool>
                ._404_NotFound("Comment policy not found");
                }
                return StatusCodeReturn<bool>
                ._404_NotFound("Policy not found");
            }
            return StatusCodeReturn<bool>
                ._404_NotFound("Post not found for this user");
        }

        public async Task<ApiResponse<bool>> MakePostsFriendsOnlyAsync(SiteUser user)
        {
            var userPosts = await _postRepository.GetUserPostsAsync(user);
            if (userPosts != null)
            {
                await UpdatePostsToFriendsOnlyAsync(user, userPosts);
                return StatusCodeReturn<bool>
                    ._200_Success("Posts policy updated successfully", true);
            }
            return StatusCodeReturn<bool>
                ._404_NotFound("No posts found for this user");
        }

        private async Task<ApiResponse<bool>> UpdatePostsToFriendsOnlyAsync(SiteUser user
            , IEnumerable<PostDto> posts)
        {
            var policy = await _policyRepository.GetPolicyByNameAsync("friends only");
            var commentPolicy = await _commentPolicyRepository.GetCommentPolicyByPolicyIdAsync(policy.Id);
            var reactPolicy = await _reactPolicyRepository.GetReactPolicyByPolicyIdAsync(policy.Id);
            foreach(var postDto in posts)
            {
                postDto.PolicyId = policy.Id;
                postDto.ReactPolicyId = reactPolicy.Id;
                postDto.CommentPolicyId = commentPolicy.Id;
                var post = ConvertFromPostDto(postDto);
                await _postRepository.UpdatePostPoliciesAsync(post, policy.Id, reactPolicy.Id,
                    commentPolicy.Id);
            }
            return StatusCodeReturn<bool>
                ._200_Success("Posts policy updated successfully", true);
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

        
        private Post ConvertFromPostDto(PostDto post)
        {
            return new Post
            {
                Id = post.PostId,
                CommentPolicyId = post.CommentPolicyId,
                PolicyId = post.PolicyId,
                ReactPolicyId = post.ReactPolicyId,
                UpdatedAt = DateTime.Now,
            };
        }

        private async Task<ApiResponse<GroupPost>> IsGroupPostAsync(UserPosts userPost)
        {
            var isGroupPost = await _groupPostsRepository.GetGroupPostByPostIdAsync(userPost.PostId);
            if (isGroupPost != null)
            {
                return StatusCodeReturn<GroupPost>
                    ._200_Success("Group post", isGroupPost);
            }
            return StatusCodeReturn<GroupPost>
                    ._404_NotFound("Not group post");
        }

        private async Task<ApiResponse<bool>> CanModifyPostPolicyAsync(UserPosts userPost, string policyId)
        {
            var isGroupPost = await IsGroupPostAsync(userPost);
            if (isGroupPost != null && isGroupPost.ResponseObject != null)
            {
                var newPolicy = await _policyRepository.GetPolicyByIdAsync(policyId);
                if (newPolicy.PolicyType != "PUBLIC")
                {
                    return StatusCodeReturn<bool>
                        ._403_Forbidden();
                }
                return StatusCodeReturn<bool>
                    ._200_Success("Ok", true);
            }
            return StatusCodeReturn<bool>
                    ._200_Success("Ok", true);
        }



    }
}
