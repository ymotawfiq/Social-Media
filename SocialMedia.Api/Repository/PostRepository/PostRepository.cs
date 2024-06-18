

using Microsoft.EntityFrameworkCore;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.DTOs;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.PolicyRepository;

namespace SocialMedia.Api.Repository.PostRepository
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPolicyRepository _policyRepository;
        public PostRepository(ApplicationDbContext _dbContext, IPolicyRepository _policyRepository)
        {
            this._dbContext = _dbContext;
            this._policyRepository = _policyRepository;
        }

        public async Task<PostDto> AddPostAsync(Post post, List<PostImages> postImages)
        {
            await _dbContext.Posts.AddAsync(post);
            if (postImages != null && postImages.Count != 0)
            {
                await _dbContext.PostImages.AddRangeAsync(postImages);
            }
            await SaveChangesAsync();
            return CreatePostDtoObject(post, postImages!);
        }
        public async Task<bool> DeletePostAsync(string postId)
        {
            try
            {
                var post = (await _dbContext.Posts.Where(e => e.Id == postId).FirstOrDefaultAsync())!;
                _dbContext.Posts.Remove(post);
                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> DeletePostImageAsync(string imageId)
        {
            var image = (await _dbContext.PostImages.Where(e=>e.Id==imageId).FirstOrDefaultAsync())!;
            _dbContext.PostImages.Remove(image);
            await SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeletePostImagesAsync(string postId)
        {
            await _dbContext.PostImages.Where(e => e.PostId == postId).ExecuteDeleteAsync();
            return true;
        }
        public async Task<PostDto> GetPostWithImagesByPostIdAsync(string postId)
        {
            var post = await GetPostByIdAsync(postId);
            var images = from i in await _dbContext.PostImages.ToListAsync()
                         where i.PostId == postId
                         select i;

            return CreatePostDtoObject(post!, images!.ToList());
        }
        public async Task<Post> GetPostByIdAsync(string postId)
        {
            return (await _dbContext.Posts.Where(e => e.Id == postId).
                Select(e=>new Post
                {
                    Id = e.Id,
                    UserId = e.UserId,
                    CommentPolicyId = e.CommentPolicyId,
                    Content = e.Content,
                    PostedAt = e.PostedAt,
                    PostPolicyId = e.PostPolicyId,
                    ReactPolicyId = e.ReactPolicyId
                }).FirstOrDefaultAsync())!;
        }
        public async Task<IEnumerable<PostDto>> GetUserPostsForFriendsAsync(SiteUser user)
        {
            List<PostDto> friendsPosts = new();
            var policy = await _policyRepository.GetPolicyByNameAsync("private");
            var posts = from p in await GetUserPostsAsync(user)
                        where p.Post.PostPolicyId != policy.Id
                        select p;
            foreach (var p in posts)
            {
                friendsPosts.Add(await GetPostWithImagesByPostIdAsync(p.Post.Id));
            }
            return friendsPosts;
        }

        public async Task<IEnumerable<PostDto>> GetUserPostsForFriendsOfFriendsAsync(SiteUser user)
        {
            List<PostDto> friendsOfFriendsPosts = new();
            var friendsOfFriendsPolicy = await _policyRepository.GetPolicyByNameAsync("friends of friends");
            var publicPolicy = await _policyRepository.GetPolicyByNameAsync("public");
            var posts = from p in await GetUserPostsAsync(user)
                        where p.Post.PostPolicyId == publicPolicy.Id 
                        || p.Post.PostPolicyId == friendsOfFriendsPolicy.Id
                        select p;
            foreach (var p in posts)
            {
                friendsOfFriendsPosts.Add(await GetPostWithImagesByPostIdAsync(p.Post.Id));
            }
            return friendsOfFriendsPosts;
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
        public async Task<PostDto> UpdatePostAsync(Post post, List<PostImages> postImages)
        {
            try
            {
                await UpdatePostAsync(post);
                await UpdatePostImagesAsync(post, postImages);
                return CreatePostDtoObject(post, postImages);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> UpdatePostAsync(Post post)
        {
            var existPost = await GetPostByIdAsync(post.Id);
            existPost.PostedAt = post.PostedAt;
            existPost.Content = post.Content;
            existPost.ReactPolicyId = post.ReactPolicyId;
            existPost.CommentPolicyId = post.CommentPolicyId;
            existPost.PostPolicyId = post.PostPolicyId;
            existPost.UpdatedAt = DateTime.Now;
            await SaveChangesAsync();
            return true;
        }
        public async Task<IEnumerable<PostDto>> GetUserPostsAsync(SiteUser user)
        {
            try
            {
                var posts = await _dbContext.Posts.Where(e => e.UserId == user.Id)
                    .Select(e => new Post
                    {
                        Id = e.Id,
                        UserId = e.UserId,
                        CommentPolicyId = e.CommentPolicyId,
                        Content = e.Content,
                        PostedAt = e.PostedAt,
                        PostPolicyId = e.PostPolicyId,
                        ReactPolicyId = e.ReactPolicyId,
                    }).ToListAsync();
                            
                var postsDto = new List<PostDto>();
                foreach (var p in posts)
                {
                    var postImages = await _dbContext.PostImages.Where(e => e.PostId == p.Id)
                        .Select(e=>new PostImages
                        {
                            Id = e.Id,
                            PostId = e.PostId,
                            ImageUrl = e.ImageUrl
                        }).ToListAsync();
                    postsDto.Add(CreatePostDtoObject(p, postImages));
                }
                return postsDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<PostDto>> GetUserPostsByPolicyAsync(SiteUser user, Policy policy)
        {
            return from i in await GetUserPostsAsync(user)
                   where i.Post.PostPolicyId == policy.Id
                   select i;
        }
        public async Task<bool> UpdateUserPostsPolicyToLockedAccountAsync(string userId)
        {
            var policy = await _policyRepository.GetPolicyByNameAsync("friends only");
            await _dbContext.Posts.Where(e => e.UserId == userId)
                .ExecuteUpdateAsync(e => e.SetProperty(p => p.PostPolicyId, policy.Id)
                .SetProperty(p => p.ReactPolicyId, policy.Id)
                .SetProperty(p => p.CommentPolicyId, policy.Id));
            await SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateUserPostsPolicyToUnLockedAccountAsync(string userId)
        {
            var policy = await _policyRepository.GetPolicyByNameAsync("public");
            await _dbContext.Posts.Where(e => e.UserId == userId)
                .ExecuteUpdateAsync(e => e.SetProperty(p => p.PostPolicyId, policy.Id)
                .SetProperty(p => p.ReactPolicyId, policy.Id)
                .SetProperty(p => p.CommentPolicyId, policy.Id));
            await SaveChangesAsync();
            return true;
        }
        private async Task UpdatePostImagesAsync(Post post, List<PostImages> postImages)
        {
            await _dbContext.PostImages.Where(e => e.PostId == post.Id).ExecuteDeleteAsync();
            await SaveChangesAsync();
            await _dbContext.PostImages.AddRangeAsync(postImages);
            await SaveChangesAsync();
            return;
        }
        private PostDto CreatePostDtoObject(Post post, List<PostImages> postImages)
        {
            if (postImages != null)
            {
                return new PostDto
                {
                    Post = new Post
                    {
                        Id = post.Id,
                        UserId = post.UserId,
                        CommentPolicyId = post.CommentPolicyId,
                        Content = post.Content,
                        PostedAt = post.PostedAt,
                        PostPolicyId = post.PostPolicyId,
                        ReactPolicyId = post.ReactPolicyId
                    },
                    Images = postImages.Select(e => new PostImages
                    {
                        PostId = e.PostId,
                        Id = e.Id,
                        ImageUrl = e.ImageUrl
                    }).ToList()
                };
            }

            return new PostDto
            {
                Post = new Post
                {
                    Id = post.Id,
                    UserId = post.UserId,
                    CommentPolicyId = post.CommentPolicyId,
                    Content = post.Content,
                    PostedAt = post.PostedAt,
                    PostPolicyId = post.PostPolicyId,
                    ReactPolicyId = post.ReactPolicyId
                }
            };
        }

        
    }
}
