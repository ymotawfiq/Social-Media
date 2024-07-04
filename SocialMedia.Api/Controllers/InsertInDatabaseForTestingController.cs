using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.PolicyRepository;
using SocialMedia.Api.Repository.PostRepository;
using SocialMedia.Api.Service.AccountService;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.PolicyService;
using SocialMedia.Api.Service.PostService;

namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class InsertInDatabaseForTestingController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IPolicyRepository _policyRepository;
        private readonly UserManager<SiteUser> _userManager;
        private readonly IPostRepository _postRepository;
        public InsertInDatabaseForTestingController(IAccountService accountService,
            IPolicyRepository policyRepository, UserManager<SiteUser> userManager,
            IPostRepository postRepository)
        {
            _accountService = accountService;
            _policyRepository = policyRepository;
            _userManager = userManager;
            _postRepository = postRepository;
        }

        [HttpPost("insert1000User")]
        public async Task<IActionResult> Insert1000UserAsync()
        {
            try
            {
                var policy = await _policyRepository.GetPolicyByNameAsync("PUBLIC");
                for (int i = 1; i <= 1000; i++)
                {
                    var user = new SiteUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        FirstName = $"user{i}",
                        LastName = $"user{i}{i}",
                        DisplayName = $"user{i}{i}{i}",
                        Email = $"user{i}@gmail.com",
                        UserName = $"user{i}",
                        SecurityStamp = Guid.NewGuid().ToString(),
                        AccountPolicyId = policy.Id,
                        AccountPostPolicyId = policy.Id,
                        ReactPolicyId = policy.Id,
                        CommentPolicyId = policy.Id,
                        FriendListPolicyId = policy.Id,
                        EmailConfirmed = true
                    };
                    await _userManager.CreateAsync(user, "12345678");
                    if (i == 1)
                    {
                        await _accountService.AssignRolesToUserAsync(new List<string> { "admin" }, user);
                    }
                    await _accountService.AssignRolesToUserAsync(null!, user);
                }
                return Ok(StatusCodeReturn<string>
                    ._201_Created("Users created successfully"));
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<object>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpPost("insertPolicies")]
        public async Task<IActionResult> InsertPoliciesAsync()
        {
            try
            {
                var policies = await _policyRepository.AddRangeAsync(new List<string> 
                { "public", "private", "friends only", "friends of friends"});
                if (policies)
                {
                    return Ok(StatusCodeReturn<string>
                    ._201_Created("Policies created successfully"));
                }
                return Ok(StatusCodeReturn<string>
                    ._403_Forbidden("Policies already exists"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<object>
                    ._500_ServerError(ex.Message));
            }
        }

        [HttpPost("insert1000Posts")]
        public async Task<IActionResult> Insert1000PostsAsync()
        {
            try
            {
                var policy = await _policyRepository.GetPolicyByNameAsync("PUBLIC");
                for(int i=1; i<=1000; i++)
                {
                    var post = new Post
                    {
                        Id = Guid.NewGuid().ToString(),
                        CommentPolicyId = policy.Id!,
                        PostPolicyId = policy.Id!,
                        PostedAt = DateTime.Now,
                        Content = $"Post {i}",
                        ReactPolicyId = policy.Id!,
                        UpdatedAt = DateTime.Now,
                        UserId = (await _userManager.FindByNameAsync($"user{i}"))!.Id
                    };
                    await _postRepository.AddPostAsync(post, null!);
                }
                return StatusCode(StatusCodes.Status201Created, StatusCodeReturn<object>
                    ._201_Created("Posts created successfully"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<object>
                    ._500_ServerError(ex.Message));
            }
        }




    }
}
