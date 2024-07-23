using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.PolicyRepository;
using SocialMedia.Api.Repository.PostRepository;
using SocialMedia.Api.Service.AccountService;
using SocialMedia.Api.Service.GenericReturn;


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
                if(policy is null){
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("Public policy not found"));
                }
                for (int i = 1; i <= 1000; i++)
                {
                    var existUser = await _userManager.FindByNameAsync($"user{i}");
                    if(existUser!=null)continue;
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
        [HttpPost("insertUsers")]
        public async Task<IActionResult> InsertUsersAsync(int from, int to)
        {
            try
            {
                if(from<to){
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._403_Forbidden("From must be greater than to."));
                }
                var policy = await _policyRepository.GetPolicyByNameAsync("PUBLIC");
                if(policy is null){
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("Public policy not found"));
                }
                for (int i = from; i <= to; i++)
                {
                    var existUser = await _userManager.FindByNameAsync($"user{i}");
                    if(existUser!=null)continue;
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
                if((await _policyRepository.GetAllAsync()).ToList().Count!=0){
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._403_Forbidden("Policies already exists"));
                }
                var policies = await _policyRepository.AddRangeAsync();
                if (policies==1)
                {
                    return Ok(StatusCodeReturn<string>
                    ._201_Created("Policies created successfully"));
                }
                else if(policies==0){
                    return Ok(StatusCodeReturn<string>
                        ._403_Forbidden("Policies already exists"));
                }
                return Ok(StatusCodeReturn<string>
                    ._500_ServerError("Policies failed to be saved"));   
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
                if(policy is null){
                    return StatusCode(StatusCodes.Status404NotFound, StatusCodeReturn<string>
                        ._404_NotFound("Public policy not found"));
                }
                for(int i=1; i<=1000; i++)
                {
                    var user = await _userManager.FindByNameAsync($"user{i}");
                    if(user==null){
                        return StatusCode(StatusCodes.Status201Created, StatusCodeReturn<object>
                            ._404_NotFound($"User (user{i}) not found"));
                    }
                    Post post = new Post
                    {
                        Id = Guid.NewGuid().ToString(),
                        CommentPolicyId = policy.Id!,
                        PostPolicyId = policy.Id!,
                        PostedAt = DateTime.Now,
                        Content = $"Post {i}",
                        ReactPolicyId = policy.Id!,
                        UpdatedAt = DateTime.Now,
                        UserId = user.Id
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
