using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Repository.PolicyRepository;
using SocialMedia.Api.Repository.PostRepository;
using SocialMedia.Api.Service.AccountService.UserRolesService;
using SocialMedia.Api.Service.GenericReturn;


namespace SocialMedia.Api.Controllers
{
    [ApiController]
    public class InsertInDatabaseForTestingController : ControllerBase
    {
        private readonly IPolicyRepository _policyRepository;
        private readonly IUserRolesService _rolesService;
        private readonly UserManager<SiteUser> _userManager;
        private readonly ApplicationDbContext _dbContext;
        private readonly IPostRepository _postRepository;
        public InsertInDatabaseForTestingController(IUserRolesService _rolesService,
            IPolicyRepository policyRepository, UserManager<SiteUser> userManager,
            IPostRepository postRepository, ApplicationDbContext _dbContext)
        {
            _policyRepository = policyRepository;
            _userManager = userManager;
            _postRepository = postRepository;
            this._rolesService = _rolesService;
            this._dbContext = _dbContext;
        }

        [HttpPost("insert-1000-user")]
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
                    if (i % 2 == 0)
                    {
                        await _rolesService.AssignRolesToUserAsync(new List<string> { "admin".ToUpper() }, user);
                    }
                    await _rolesService.AssignRolesToUserAsync(null!, user);
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
        [HttpPost("insert-users")]
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
                        await _rolesService.AssignRolesToUserAsync(new List<string> { "admin" }, user);
                    }
                    await _rolesService.AssignRolesToUserAsync(null!, user);
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

        [HttpPost("insert-policies")]
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

        [HttpPost("insert-1000-posts")]
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

        [HttpPost("insert-roles")]
        public async Task<IActionResult> InsertRolesAsync(){
            try{
                if(_dbContext.Roles.ToList().Count==0 || _dbContext.Roles.ToList() == null){
                    await _dbContext.Roles.AddRangeAsync(
                    [
                            new IdentityRole{
                                Id = Guid.NewGuid().ToString(),
                                ConcurrencyStamp = Guid.NewGuid().ToString(),
                                Name = "USER",
                                NormalizedName = "USER"
                            },
                            new IdentityRole{
                                Id = Guid.NewGuid().ToString(),
                                ConcurrencyStamp = Guid.NewGuid().ToString(),
                                Name = "ADMIN",
                                NormalizedName = "ADMIN"
                            },
                    ]);
                    await _dbContext.SaveChangesAsync();
                    return StatusCode(StatusCodes.Status201Created, StatusCodeReturn<string>
                        ._201_Created("Roles inserted successfully"));
                }
                return StatusCode(StatusCodes.Status403Forbidden, StatusCodeReturn<string>
                        ._403_Forbidden("Roles already exist"));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, StatusCodeReturn<object>
                    ._500_ServerError(ex.Message));
            }
        }




    }
}
