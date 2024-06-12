

using Microsoft.EntityFrameworkCore;
using SocialMedia.Data;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.CommentPolicyRepository;
using SocialMedia.Repository.PolicyRepository;
using SocialMedia.Repository.PostsPolicyRepository;
using SocialMedia.Repository.ReactPolicyRepository;

namespace SocialMedia.Repository.PostRepository
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPolicyRepository _policyRepository;
        private readonly IReactPolicyRepository _reactPolicyRepository;
        private readonly ICommentPolicyRepository _commentPolicyRepository;
        private readonly IPostsPolicyRepository _postsPolicyRepository;
        public PostRepository(ApplicationDbContext _dbContext, IPolicyRepository _policyRepository,
            IReactPolicyRepository _reactPolicyRepository, ICommentPolicyRepository _commentPolicyRepository,
            IPostsPolicyRepository _postsPolicyRepository)
        {
            this._dbContext = _dbContext;
            this._policyRepository = _policyRepository;
            this._reactPolicyRepository = _reactPolicyRepository;
            this._commentPolicyRepository = _commentPolicyRepository;
            this._postsPolicyRepository = _postsPolicyRepository;
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
            return (await _dbContext.Posts.Where(e => e.Id == postId).FirstOrDefaultAsync())!;
        }
        public async Task<IEnumerable<PostDto>> GetUserPostsForFriendsAsync(SiteUser user)
        {
            List<PostDto> friendsPosts = new();
            var policy = await _policyRepository.GetPolicyByNameAsync("private");
            var postPolicy = await _postsPolicyRepository.GetPostPolicyByPolicyIdAsync(policy.Id);
            var posts = from p in await _dbContext.Posts.ToListAsync()
                        where p.PostPolicyId != postPolicy.Id
                        select p;
            foreach (var p in posts)
            {
                friendsPosts.Add(await GetPostWithImagesByPostIdAsync(p.Id));
            }
            return friendsPosts;
        }

        public async Task<IEnumerable<PostDto>> GetUserPostsForFriendsOfFriendsAsync(SiteUser user)
        {
            List<PostDto> friendsOfFriendsPosts = new();
            var friendsOfFriendsPolicy = await _policyRepository.GetPolicyByNameAsync("friends of friends");
            var publicPolicy = await _policyRepository.GetPolicyByNameAsync("public");
            var publicPostPolicy = await _postsPolicyRepository.GetPostPolicyByPolicyIdAsync(publicPolicy.Id);
            var friendsOfFriendsPostPolicy = await _postsPolicyRepository.GetPostPolicyByPolicyIdAsync(
                friendsOfFriendsPolicy.Id);
            var posts = from p in await _dbContext.Posts.ToListAsync()
                        where p.PostPolicyId == publicPostPolicy.Id 
                        || p.PostPolicyId == friendsOfFriendsPostPolicy.Id
                        select p;
            foreach (var p in posts)
            {
                friendsOfFriendsPosts.Add(await GetPostWithImagesByPostIdAsync(p.Id));
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
                var posts = from p in await _dbContext.Posts.ToListAsync()
                            where p.UserId == user.Id
                            select p;
                var postsDto = new List<PostDto>();
                foreach (var p in posts)
                {
                    var postImages = await _dbContext.PostImages.Where(e => e.PostId == p.Id).ToListAsync();
                    postsDto.Add(CreatePostDtoObject(p, postImages));
                }
                return postsDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<PostDto>> GetUserPostsByPolicyAsync(SiteUser user, PostsPolicy policy)
        {
            return from i in await GetUserPostsAsync(user)
                   where i.Post.PostPolicyId == policy.Id
                   select i;
        }
        public async Task<bool> UpdateUserPostsPolicyToLockedAccountAsync(string userId)
        {
            var policy = await _policyRepository.GetPolicyByNameAsync("friends only");
            var postPolicy = await _postsPolicyRepository.GetPostPolicyByPolicyIdAsync(policy.Id);
            var reactPolicy = await _reactPolicyRepository.GetReactPolicyByPolicyIdAsync(policy.Id);
            var commentPolicy = await _commentPolicyRepository.GetCommentPolicyByPolicyIdAsync(policy.Id);
            await _dbContext.Posts.Where(e => e.UserId == userId)
                .ExecuteUpdateAsync(e => e.SetProperty(p => p.PostPolicyId, postPolicy.Id)
                .SetProperty(p => p.ReactPolicyId, reactPolicy.Id)
                .SetProperty(p => p.CommentPolicyId, commentPolicy.Id));
            await SaveChangesAsync();
            return true;
        }
        public async Task<bool> UpdateUserPostsPolicyToUnLockedAccountAsync(string userId)
        {
            var policy = await _policyRepository.GetPolicyByNameAsync("public");
            var postPolicy = await _postsPolicyRepository.GetPostPolicyByPolicyIdAsync(policy.Id);
            var reactPolicy = await _reactPolicyRepository.GetReactPolicyByPolicyIdAsync(policy.Id);
            var commentPolicy = await _commentPolicyRepository.GetCommentPolicyByPolicyIdAsync(policy.Id);
            await _dbContext.Posts.Where(e => e.UserId == userId)
                .ExecuteUpdateAsync(e => e.SetProperty(p => p.PostPolicyId, postPolicy.Id)
                .SetProperty(p => p.ReactPolicyId, reactPolicy.Id)
                .SetProperty(p => p.CommentPolicyId, commentPolicy.Id));
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
                    Post = post,
                    Images = postImages
                };
            }

            return new PostDto
            {
                Post = post
            };
        }

        
    }
}
