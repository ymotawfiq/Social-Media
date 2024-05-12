

using Microsoft.AspNetCore.Http;

using SocialMedia.Data.DTOs;
using SocialMedia.Data.Extensions;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.AccountPolicyRepository;
using SocialMedia.Repository.CommentPolicyRepository;
using SocialMedia.Repository.FriendsRepository;
using SocialMedia.Repository.PolicyRepository;
using SocialMedia.Repository.PostRepository;
using SocialMedia.Repository.PostViewRepository;
using SocialMedia.Repository.ReactPolicyRepository;
using SocialMedia.Repository.UserPostsRepository;
using SocialMedia.Service.FriendsService;
using SocialMedia.Service.PolicyService;
using SocialMedia.Service.ReactPolicyService;

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
        private readonly IReactPolicyService _reactPolicyService;
        private readonly IAccountPolicyRepository _accountPolicyRepository;
        private readonly IPostViewRepository _postViewRepository;
        

        public PostService(IPostRepository _postRepository,
             ICommentPolicyRepository _commentPolicyRepository,
            IReactPolicyRepository _reactPolicyRepository, IPolicyRepository _policyRepository,
            IFriendsRepository _friendsRepository, IFriendService _friendService,
            IUserPostsRepository _userPostsRepository, IPolicyService _policyService,
            IReactPolicyService _reactPolicyService, IAccountPolicyRepository _accountPolicyRepository,
            IPostViewRepository _postViewRepository)
        {
            this._postRepository = _postRepository;
            this._commentPolicyRepository = _commentPolicyRepository;
            this._policyRepository = _policyRepository;
            this._reactPolicyRepository = _reactPolicyRepository;
            this._friendsRepository = _friendsRepository;
            this._friendService = _friendService;
            this._userPostsRepository = _userPostsRepository;
            this._policyService = _policyService;
            this._reactPolicyService = _reactPolicyService;
            this._accountPolicyRepository = _accountPolicyRepository;
            this._postViewRepository = _postViewRepository;
        }
        public async Task<ApiResponse<PostDto>> AddPostAsync(SiteUser user, CreatePostDto createPostDto)
        {
            var accountPolicy = await _accountPolicyRepository.GetAccountPolicyByIdAsync(user.AccountPolicyId);
            var policy = await _policyRepository.GetPolicyByIdAsync(accountPolicy.PolicyId);
            if (policy == null)
            {
                return new ApiResponse<PostDto>
                {
                    IsSuccess = false,
                    Message = "Policy not found",
                    StatusCode = 404
                };
            }
            if (policy.PolicyType == "PUBLIC")
            {
                policy = await _policyRepository.GetPolicyByNameAsync("public");
            }
            else if (policy.PolicyType == "PRIVATE")
            {
                policy = await _policyRepository.GetPolicyByNameAsync("friends only");
            }
            var reactPolicy = await _reactPolicyRepository.GetReactPolicyByPolicyIdAsync(policy.Id);
            if (reactPolicy == null)
            {
                return new ApiResponse<PostDto>
                {
                    IsSuccess = false,
                    Message = "React policy not found",
                    StatusCode = 404
                };
            }
            var commentPolicy = await _commentPolicyRepository.GetCommentPolicyByPolicyIdAsync(policy.Id);
            if (commentPolicy == null)
            {
                return new ApiResponse<PostDto>
                {
                    IsSuccess = false,
                    Message = "Comment policy not found",
                    StatusCode = 404
                };
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
            userPosts.User = null;
            return new ApiResponse<PostDto>
            {
                IsSuccess = true,
                Message = "Post created successfully",
                StatusCode = 201,
                ResponseObject = newPostDto
            };
        }

        public async Task<ApiResponse<PostDto>> DeletePostAsync(SiteUser user, string postId)
        {
            var existPost = await _postRepository.IsPostExistsAsync(postId);
            if (existPost == null)
            {
                return new ApiResponse<PostDto>
                {
                    IsSuccess = false,
                    Message = "Post not found",
                    StatusCode = 404
                };
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
            return new ApiResponse<PostDto>
            {
                StatusCode = 200,
                Message = "Post deleted successfully",
                IsSuccess = true
            };
        }

        public async Task<ApiResponse<PostDto>> GetPostByIdAsync(SiteUser user, string postId)
        {
            var existPost = await _postRepository.IsPostExistsAsync(postId);
            if (existPost == null)
            {
                return new ApiResponse<PostDto>
                {
                    IsSuccess = false,
                    Message = "Post not found",
                    StatusCode = 404
                };
            }
            var post = await _postRepository.GetPostByIdAsync(user, postId);
            return new ApiResponse<PostDto>
            {
                StatusCode = 200,
                Message = "Post found successfully",
                IsSuccess = true,
                ResponseObject = post
            };
        }

        public async Task<ApiResponse<IEnumerable<PostDto>>> GetUserPostsAsync(SiteUser user)
        {
            var posts = await _postRepository.GetUserPostsAsync(user);
            if (posts.ToList().Count == 0)
            {
                return new ApiResponse<IEnumerable<PostDto>>
                {
                    IsSuccess = true,
                    Message = "No posts found",
                    ResponseObject = posts,
                    StatusCode = 200
                };
            }
            return new ApiResponse<IEnumerable<PostDto>>
            {
                IsSuccess = true,
                Message = "Posts found successfully",
                ResponseObject = posts,
                StatusCode = 200
            };
        }


        public async Task<ApiResponse<IEnumerable<PostDto>>> GetUserPostsByPolicyAsync
            (SiteUser user, Policy policy)
        {
            var checkPolicy = await _policyRepository.GetPolicyByNameAsync(policy.PolicyType);
            if (checkPolicy == null)
            {
                return new ApiResponse<IEnumerable<PostDto>>
                {
                    IsSuccess = false,
                    Message = "Policy not found",
                    StatusCode = 404
                };
            }
            var userPosts = await _postRepository.GetUserPostsByPolicyAsync(user, policy);
            if (userPosts.ToList().Count == 0)
            {
                return new ApiResponse<IEnumerable<PostDto>>
                {
                    IsSuccess = true,
                    Message = "No posts found",
                    StatusCode = 200,
                    ResponseObject = userPosts
                };
            }
            return new ApiResponse<IEnumerable<PostDto>>
            {
                IsSuccess = true,
                Message = "Posts found successfully",
                StatusCode = 200,
                ResponseObject = userPosts
            };
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
                return new ApiResponse<IEnumerable<List<PostDto>>>
                {
                    IsSuccess = true,
                    ResponseObject = posts,
                    Message = " No posts found",
                    StatusCode = 200
                };
            }
            return new ApiResponse<IEnumerable<List<PostDto>>>
            {
                IsSuccess = true,
                ResponseObject = posts,
                Message = "Posts found successfully",
                StatusCode = 200
            };
        }

        public async Task<ApiResponse<IEnumerable<List<PostDto>>>> 
            GetPostsForFriendsOfFriendsAsync(SiteUser user)
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
                return new ApiResponse<IEnumerable<List<PostDto>>>
                {
                    IsSuccess = true,
                    ResponseObject = posts,
                    Message = " No posts found",
                    StatusCode = 200
                };
            }
            return new ApiResponse<IEnumerable<List<PostDto>>>
            {
                IsSuccess = true,
                ResponseObject = posts,
                Message = "Posts found successfully",
                StatusCode = 200
            };
        }


        public async Task<ApiResponse<IEnumerable<List<PostDto>>>> 
            CheckFriendShipAndGetPostsAsync(SiteUser currentUser, SiteUser routeUser)
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
                return new ApiResponse<IEnumerable<List<PostDto>>>
                {
                    IsSuccess = true,
                    Message = "Posts found successfully",
                    StatusCode = 200,
                    ResponseObject = posts
                };
            }
            return new ApiResponse<IEnumerable<List<PostDto>>>
            {
                IsSuccess = false,
                Message = "No posts found successfully",
                StatusCode = 404
            };
        }


        public async Task<ApiResponse<PostDto>> 
            UpdatePostAsync(SiteUser user, UpdatePostDto updatePostDto)
        {
            var userPost = await _userPostsRepository
                .GetUserPostByUserAndPostIdAsync(user.Id, updatePostDto.PostId);
            if (userPost != null)
            {
                var postDto = await _postRepository.GetPostByIdAsync(user, updatePostDto.PostId);
                if (postDto.Images != null)
                {
                    foreach(var p in postDto.Images)
                    {
                        DeletePostImage(p.ImageUrl);
                    }
                }
                var postImages = new List<PostImages>();
                if (updatePostDto.Images != null)
                {
                    if (updatePostDto.Images != null)
                    {
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
                }
                var oldPost = await _postRepository.IsPostExistsAsync(postDto.PostId);
                var post = ConvertFromDto.ConvertFromPostDto_Update(updatePostDto,
                    postDto, oldPost);
                var updatedPost = await _postRepository.UpdatePostAsync(user, post, postImages);
                var userPosts = await _userPostsRepository.GetUserPostByPostIdAsync(post.Id);
                userPosts.User = null;
                return new ApiResponse<PostDto>
                {
                    IsSuccess = true,
                    Message = "Post updated successfully",
                    StatusCode = 200,
                    ResponseObject = updatedPost
                };
            }
            return new ApiResponse<PostDto>
            {
                IsSuccess = false,
                Message = "Forbidden",
                StatusCode = 403
            };
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
                            return new ApiResponse<PostDto>
                            {
                                IsSuccess = false,
                                Message = "Forbidden",
                                StatusCode = 403
                            };
                        }
                    }
                    else if (postPolicy.ResponseObject.PolicyType == "FRIENDS ONLY")
                    {
                        var isFriend = await _friendsRepository.GetFriendByUserAndFriendIdAsync(
                            currentUser.Id, routeUser.Id);
                        if (isFriend == null)
                        {
                            return new ApiResponse<PostDto>
                            {
                                IsSuccess = false,
                                Message = "Forbidden",
                                StatusCode = 403
                            };
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
                            return new ApiResponse<PostDto>
                            {
                                IsSuccess = false,
                                Message = "Forbidden",
                                StatusCode = 403
                            };
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
                                UserId = currentUser.Id,
                                ViewNumber = 1
                            }
                            );
                        return new ApiResponse<PostDto>
                        {
                            IsSuccess = true,
                            Message = "Post found successfully",
                            StatusCode = 200,
                            ResponseObject = post
                        };
                    }
                    await _postViewRepository.UpdatePostViewAsync(postView);
                    return new ApiResponse<PostDto>
                    {
                        IsSuccess = true,
                        Message = "Post found successfully",
                        StatusCode = 200,
                        ResponseObject = post
                    };
                }
                return new ApiResponse<PostDto>
                {
                    IsSuccess = false,
                    Message = "Post policy not found",
                    StatusCode = 404,
                };
            }
            return new ApiResponse<PostDto>
            {
                IsSuccess = false,
                Message = "Post not found",
                StatusCode = 404,
            };
        }



        public async Task<ApiResponse<bool>> UpdatePostPolicyAsync
            (SiteUser user, UpdatePostPolicyDto updatePostPolicyDto)
        {
            var userPost = await _userPostsRepository.GetUserPostByUserAndPostIdAsync(user.Id,
                updatePostPolicyDto.PostId);
            if (userPost != null)
            {
                var policy = await _policyService.GetPolicyByIdOrNameAsync(updatePostPolicyDto.PolicyIdOrName);
                if (policy.ResponseObject != null)
                {
                    var post = await _postRepository.GetPostByIdAsync(user, updatePostPolicyDto.PostId);
                    post.PolicyId = policy.ResponseObject.Id;
                    await _postRepository.UpdatePostPolicyAsync(user, ConvertFromPostDto(post));
                    return new ApiResponse<bool>
                    {
                        IsSuccess = true,
                        Message = "Post policy updated successfully",
                        StatusCode = 200,
                        ResponseObject = true
                    };
                }
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Message = "Policy not found",
                    StatusCode = 404
                };
            }
            return new ApiResponse<bool>
            {
                IsSuccess = false,
                Message = "Post not found for this user",
                StatusCode = 404
            };
        }

        public async Task<ApiResponse<bool>> UpdatePostReactPolicyAsync
            (SiteUser user, UpdatePostReactPolicyDto updatePostReactPolicy)
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
                        return new ApiResponse<bool>
                        {
                            IsSuccess = true,
                            Message = "Post react policy updated successfully",
                            StatusCode = 200,
                            ResponseObject = true
                        };
                    }
                    return new ApiResponse<bool>
                    {
                        IsSuccess = false,
                        Message = "React policy not found",
                        StatusCode = 404
                    };
                }
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Message = "Policy not found",
                    StatusCode = 404
                };
            }
            return new ApiResponse<bool>
            {
                IsSuccess = false,
                Message = "Post not found for this user",
                StatusCode = 404
            };
        }

        public async Task<ApiResponse<bool>> UpdatePostCommentPolicyAsync
            (SiteUser user, UpdatePostCommentPolicyDto updatePostCommentPolicyDto)
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
                        return new ApiResponse<bool>
                        {
                            IsSuccess = true,
                            Message = "Post comment policy updated successfully",
                            StatusCode = 200,
                            ResponseObject = true
                        };
                    }
                    return new ApiResponse<bool>
                    {
                        IsSuccess = false,
                        Message = "Comment policy not found",
                        StatusCode = 404
                    };
                }
                return new ApiResponse<bool>
                {
                    IsSuccess = false,
                    Message = "Policy not found",
                    StatusCode = 404
                };
            }
            return new ApiResponse<bool>
            {
                IsSuccess = false,
                Message = "Post not found for this user",
                StatusCode = 404
            };
        }



        private string SavePostImages(IFormFile image)
        {
            var path = @"D:\my_source_code\C sharp\SocialMedia.Solution\ImageStorageTest\PostsImages";
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
            var folder = @"D:\my_source_code\C sharp\SocialMedia.Solution\ImageStorageTest\PostsImages\";
            var file = Path.Combine(folder, $"{imageUrl}");
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

    }
}
