

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using SocialMedia.Data;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.ApiResponseModel;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.CommentPolicyRepository;
using SocialMedia.Repository.FriendsRepository;
using SocialMedia.Repository.PolicyRepository;
using SocialMedia.Repository.PostRepository;
using SocialMedia.Repository.ReactPolicyRepository;
using SocialMedia.Repository.UserPostsRepository;
using SocialMedia.Service.FriendsService;

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
        private readonly IUserPostsRepository _userPostsRepository;
       
        public PostService(IPostRepository _postRepository,
             ICommentPolicyRepository _commentPolicyRepository,
            IReactPolicyRepository _reactPolicyRepository, IPolicyRepository _policyRepository,
            IFriendsRepository _friendsRepository, IFriendService _friendService,
            IUserPostsRepository _userPostsRepository)
        {
            this._postRepository = _postRepository;
            this._commentPolicyRepository = _commentPolicyRepository;
            this._policyRepository = _policyRepository;
            this._reactPolicyRepository = _reactPolicyRepository;
            this._friendsRepository = _friendsRepository;
            this._friendService = _friendService;
            this._userPostsRepository = _userPostsRepository;
        }
        public async Task<ApiResponse<PostDto>> AddPostAsync(SiteUser user, AddPostDto addPostDto)
        {
            var commentPolicy = await _commentPolicyRepository
                .GetCommentPolicyByIdAsync(addPostDto.CommentPolicyId);
            var reactPolicy = await _reactPolicyRepository.GetReactPolicyByIdAsync(addPostDto.ReactPolicyId);
            var policy = await _policyRepository.GetPolicyByIdAsync(addPostDto.PolicyId);
            if (policy == null)
            {
                return new ApiResponse<PostDto>
                {
                    IsSuccess = false,
                    Message = "Policy not found",
                    StatusCode = 404
                };
            }
            else if (reactPolicy == null)
            {
                return new ApiResponse<PostDto>
                {
                    IsSuccess = false,
                    Message = "React policy not found",
                    StatusCode = 404
                };
            }
            else if (commentPolicy == null)
            {
                return new ApiResponse<PostDto>
                {
                    IsSuccess = false,
                    Message = "Comment policy not found",
                    StatusCode = 404
                };
            }
            var post = CreatePostFromAddPostDto(addPostDto);
            var postImages = new List<PostImages>();
            if (addPostDto.Images != null)
            {
                foreach(var i in addPostDto.Images)
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
                var post = await CreatePostFromUpdatePostDto(updatePostDto);
                var updatedPost = await _postRepository.UpdatePostAsync(user, post, postImages);
                var userPosts = await _userPostsRepository.GetUserPostByPostIdAsync(post.Id);
                userPosts.User = null;
                return new ApiResponse<PostDto>
                {
                    IsSuccess = false,
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
                //await SetUserInfoNull(postId);
                var userPosts = await _userPostsRepository.GetUserPostByPostIdAsync(postId);
                userPosts.User = null;
                if (routeUser.Id==currentUser.Id)
                    {
                        return new ApiResponse<PostDto>
                        {
                            IsSuccess = true,
                            Message = "Post found successfully",
                            StatusCode = 200,
                            ResponseObject = post
                        };
                    }
                var policy = await _policyRepository.GetPolicyByIdAsync(post.PolicyId);
                if (policy.PolicyType == "PUBLIC")
                {
                    return new ApiResponse<PostDto>
                    {
                        IsSuccess = true,
                        Message = "Post found successfully",
                        StatusCode = 200,
                        ResponseObject = post
                    };
                }
                else if(policy.PolicyType=="FRIENDS ONLY")
                {
                    var isFriend = await _friendService.IsUserFriendAsync(routeUser.Id, currentUser.Id);
                    if (isFriend.ResponseObject)
                    {
                        return new ApiResponse<PostDto>
                        {
                            IsSuccess = true,
                            Message = "Post found successfully",
                            StatusCode = 200,
                            ResponseObject = post
                        };
                    }
                }
                else if (policy.PolicyType == "FRIENDS OF FRIENDS")
                {
                    var isFriendOfFriend = await _friendService
                        .IsUserFriendOfFriendAsync(routeUser.Id, currentUser.Id);
                    if (isFriendOfFriend.ResponseObject)
                    {
                        return new ApiResponse<PostDto>
                        {
                            IsSuccess = true,
                            Message = "Post found successfully",
                            StatusCode = 200,
                            ResponseObject = post
                        };
                    }
                }

                return new ApiResponse<PostDto>
                {
                    IsSuccess = false,
                    Message = "Forbidden",
                    StatusCode = 403,
                };
            }
            return new ApiResponse<PostDto>
            {
                IsSuccess = false,
                Message = "Post not found",
                StatusCode = 404
            };
        }

        private Post CreatePostFromAddPostDto(AddPostDto postDto)
        {
            return new Post
            {
                Id = Guid.NewGuid().ToString(),
                CommentPolicyId = postDto.CommentPolicyId,
                Content = postDto.PostContent,
                PolicyId = postDto.PolicyId,
                PostedAt = DateTime.Now,
                ReactPolicyId = postDto.ReactPolicyId,
                UpdatedAt = DateTime.Now
            };
        }

        private async Task<Post> CreatePostFromUpdatePostDto(UpdatePostDto postDto)
        {
            var post = await _postRepository.IsPostExistsAsync(postDto.PostId);
            return new Post
            {
                Id = postDto.PostId,
                CommentPolicyId = postDto.CommentPolicyId,
                Content = postDto.PostContent,
                PolicyId = postDto.PolicyId,
                PostedAt = post.PostedAt,
                ReactPolicyId = postDto.ReactPolicyId,
                UpdatedAt = DateTime.Now
            };
        }


        private async Task SetUserInfoNull(string postId)
        {
            var userPosts = await _userPostsRepository.GetUserPostByPostIdAsync(postId);
            userPosts.User.PasswordHash = null;
            userPosts.User.UserName = null;
            userPosts.User.Email = null;
            userPosts.User.SecurityStamp = null;
            userPosts.User.ConcurrencyStamp = null;
            userPosts.User.NormalizedEmail = null;
            userPosts.User.NormalizedUserName = null;
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

        
    }
}
