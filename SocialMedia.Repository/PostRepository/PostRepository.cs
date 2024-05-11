

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SocialMedia.Data;
using SocialMedia.Data.DTOs;
using SocialMedia.Data.Models;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Repository.CommentPolicyRepository;
using SocialMedia.Repository.PolicyRepository;
using SocialMedia.Repository.ReactPolicyRepository;

namespace SocialMedia.Repository.PostRepository
{
    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPolicyRepository _policyRepository;
        private readonly IReactPolicyRepository _reactPolicyRepository;
        private readonly ICommentPolicyRepository _commentPolicyRepository;
        public PostRepository(ApplicationDbContext _dbContext, IPolicyRepository _policyRepository,
            IReactPolicyRepository _reactPolicyRepository, ICommentPolicyRepository _commentPolicyRepository)
        {
            this._dbContext = _dbContext;
            this._policyRepository = _policyRepository;
            this._reactPolicyRepository = _reactPolicyRepository;
            this._commentPolicyRepository = _commentPolicyRepository;
        }
        public async Task<PostDto> AddPostAsync(SiteUser user, Post post, List<PostImages> postImages)
        {
            try
            {
                var userPosts = new UserPosts
                {
                    Id = Guid.NewGuid().ToString(),
                    PostId = post.Id,
                    UserId = user.Id
                };
                await _dbContext.Posts.AddAsync(post);
                await _dbContext.UserPosts.AddAsync(userPosts);
                if (postImages != null && postImages.Count != 0)
                {
                    await _dbContext.PostImages.AddRangeAsync(postImages);
                }
                await SaveChangesAsync();
                return CreatePostDtoObject(post, postImages!);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostDto> DeletePostAsync(SiteUser user, string postId)
        {
            try
            {
                // delete from posts table
                var post = await _dbContext.Posts.Where(e => e.Id == postId).FirstOrDefaultAsync();
                _dbContext.Posts.Remove(post!);
                // delete from user posts table
                var userPost = await _dbContext.UserPosts.Where(e => e.PostId == postId)
                    .Where(e => e.UserId == user.Id).FirstOrDefaultAsync();
                _dbContext.UserPosts.Remove(userPost!);
                // delete post images
                var images = from i in await _dbContext.PostImages.ToListAsync()
                             where i.PostId == post!.Id
                             select i;
                if (images != null)
                {
                    _dbContext.PostImages.RemoveRange(images);
                }
                await SaveChangesAsync();
                return CreatePostDtoObject(post!, images!.ToList());
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PostDto> GetPostByIdAsync(SiteUser user, string postId)
        {
            var post = await _dbContext.Posts.Where(e => e.Id == postId).FirstOrDefaultAsync();
            var images = from i in await _dbContext.PostImages.ToListAsync()
                         where i.PostId == postId
                         select i;

            return CreatePostDtoObject(post!, images!.ToList());
        }


        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<PostDto> UpdatePostAsync(SiteUser user, Post post, List<PostImages> postImages)
        {
            try
            {
                _dbContext.Posts.Update(post);
                await UpdatePostImagesAsync(post, postImages);
                await SaveChangesAsync();
                return CreatePostDtoObject(post, postImages);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<bool> UpdateUserPostsPolicyToFriendsOnlyAsync
            (SiteUser user)
        {
            try
            {
                var userPosts = await GetUserPostsAsync(user);
                userPosts.ToList().ForEach(async a =>
                {
                    var friendsOnlyPolicy = await _policyRepository.GetPolicyByNameAsync("friends only");
                    var reactPolicy = await _reactPolicyRepository.GetReactPolicyByPolicyIdAsync(
                        friendsOnlyPolicy.Id);
                    var commentPolicy = await _commentPolicyRepository.GetCommentPolicyByPolicyIdAsync(
                        friendsOnlyPolicy.Id);
                    var publicPolicy = await _policyRepository.GetPolicyByNameAsync("public");
                    if (a.PolicyId == publicPolicy.Id)
                    {
                        a.PolicyId = friendsOnlyPolicy.Id;
                        a.ReactPolicyId = reactPolicy.Id;
                        a.CommentPolicyId = commentPolicy.Id;
                    }
                    
                });
                await SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<IEnumerable<PostDto>> GetUserPostsAsync(SiteUser user)
        {
            try
            {
                var userPosts = await _dbContext.UserPosts.Where(e => e.UserId == user.Id).ToListAsync();
                var postsDto = new List<PostDto>();
                foreach (var p in userPosts)
                {
                    var post = await _dbContext.Posts.Where(e => e.Id == p.PostId).FirstOrDefaultAsync();
                    var postImages = await _dbContext.PostImages.Where(e => e.PostId == p.PostId).ToListAsync();
                    postsDto.Add(CreatePostDtoObject(post!, postImages));
                }
                return postsDto;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<Post> IsPostExistsAsync(string postId)
        {
            return (await _dbContext.Posts.Where(e => e.Id == postId).FirstOrDefaultAsync())!;
        }


        public async Task<IEnumerable<PostDto>> GetUserPostsByPolicyAsync(SiteUser user, Policy policy)
        {
            return from i in await GetUserPostsAsync(user)
                   where i.PolicyId == policy.Id
                   select i;
        }

        public async Task<Post> UpdatePostPolicyAsync(SiteUser user, Post post)
        {
            try
            {
                var oldPost = await _dbContext.Posts.Where(e => e.Id == post.Id).FirstOrDefaultAsync()!;
                oldPost!.PolicyId = post.PolicyId;
                _dbContext.Posts.Update(oldPost);
                await SaveChangesAsync();
                return oldPost;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Post> UpdatePostReactPolicyAsync(SiteUser user, Post post)
        {
            try
            {
                var oldPost = await _dbContext.Posts.Where(e => e.Id == post.Id).FirstOrDefaultAsync()!;
                oldPost!.ReactPolicyId = post.ReactPolicyId;
                _dbContext.Posts.Update(oldPost);
                await SaveChangesAsync();
                return oldPost;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Post> UpdatePostCommentPolicyAsync(SiteUser user, Post post)
        {
            try
            {
                var oldPost = await _dbContext.Posts.Where(e => e.Id == post.Id).FirstOrDefaultAsync()!;
                oldPost!.CommentPolicyId = post.CommentPolicyId;
                _dbContext.Posts.Update(oldPost);
                await SaveChangesAsync();
                return oldPost;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task UpdatePostImagesAsync(Post post, List<PostImages> postImages)
        {
            var oldPostImages = from i in await _dbContext.PostImages.ToListAsync()
                                where i.PostId == post.Id
                                select i;
            if (oldPostImages != null)
            {
                _dbContext.PostImages.RemoveRange(oldPostImages);
            }
            if (postImages == null || postImages.Count == 0)
            {
                await SaveChangesAsync();
                return;
            }
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
                    CommentPolicyId = post.CommentPolicyId,
                    Images = postImages,
                    PolicyId = post.PolicyId,
                    PostContent = post.Content,
                    PostId = post.Id,
                    ReactPolicyId = post.ReactPolicyId
                };
            }

            return new PostDto
            {
                CommentPolicyId = post.CommentPolicyId,
                PolicyId = post.PolicyId,
                PostContent = post.Content,
                PostId = post.Id,
                ReactPolicyId = post.ReactPolicyId
            };
        }


    }
}
