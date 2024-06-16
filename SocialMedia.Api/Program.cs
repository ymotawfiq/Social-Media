using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialMedia.Data;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Data.Models.EmailModel;
using SocialMedia.Repository.BlockRepository;
using SocialMedia.Repository.FollowerRepository;
using SocialMedia.Repository.FriendRequestRepository;
using SocialMedia.Repository.FriendsRepository;
using SocialMedia.Repository.GroupAccessRequestRepository;
using SocialMedia.Repository.GroupMemberRepository;
using SocialMedia.Repository.GroupMemberRoleRepository;
using SocialMedia.Repository.GroupPostsRepository;
using SocialMedia.Repository.GroupRepository;
using SocialMedia.Repository.GroupRoleRepository;
using SocialMedia.Repository.PagePostsRepository;
using SocialMedia.Repository.PageRepository;
using SocialMedia.Repository.PagesFollowersRepository;
using SocialMedia.Repository.PolicyRepository;
using SocialMedia.Repository.PostCommentsRepository;
using SocialMedia.Repository.PostReactsRepository;
using SocialMedia.Repository.PostRepository;
using SocialMedia.Repository.PostViewRepository;
using SocialMedia.Repository.ReactRepository;
using SocialMedia.Repository.SarehneRepository;
using SocialMedia.Repository.SavePostsRepository;
using SocialMedia.Repository.UserSavedPostsFoldersRepository;
using SocialMedia.Service.BlockService;
using SocialMedia.Service.FollowerService;
using SocialMedia.Service.FriendRequestService;
using SocialMedia.Service.FriendsService;
using SocialMedia.Service.GenericReturn;
using SocialMedia.Service.GroupAccessRequestService;
using SocialMedia.Service.GroupManager;
using SocialMedia.Service.GroupPostsService;
using SocialMedia.Service.GroupRolesService;
using SocialMedia.Service.GroupService;
using SocialMedia.Service.PagePostsService;
using SocialMedia.Service.PageService;
using SocialMedia.Service.PagesFollowersService;
using SocialMedia.Service.PolicyService;
using SocialMedia.Service.PostCommentService;
using SocialMedia.Service.PostReactsService;
using SocialMedia.Service.PostService;
using SocialMedia.Service.ReactService;
using SocialMedia.Service.SarehneService;
using SocialMedia.Service.SavedPostsService;
using SocialMedia.Service.SendEmailService;
using SocialMedia.Service.UserAccountService;
using SocialMedia.Service.UserSavedPostsFoldersService;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);


// database configurations
var connection = builder.Configuration.GetConnectionString("Connection");
builder.Services.AddDbContext<ApplicationDbContext>(op =>
{
    op.UseSqlServer(connection);
});


builder.Services.AddIdentity<SiteUser, IdentityRole>(op =>
{
    op.Tokens.ProviderMap["Email"] = new TokenProviderDescriptor(typeof(EmailTokenProvider<SiteUser>));

    op.Password.RequireDigit = false;
    op.Password.RequiredLength = 8;
    op.Password.RequiredUniqueChars = 0;
    op.Password.RequireLowercase = false;
    op.Password.RequireNonAlphanumeric = false;
    op.Password.RequireUppercase = false;

    op.User.RequireUniqueEmail = true;
    op.SignIn.RequireConfirmedEmail = true;
    
}).AddEntityFrameworkStores<ApplicationDbContext>()
  .AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(op =>
{
    op.TokenLifespan = TimeSpan.FromMinutes(5);
});

builder.Services.ConfigureApplicationCookie(op =>
{
    op.AccessDeniedPath = "/error/403";
    op.LoginPath = "/login";
});

builder.Services.AddAuthentication(op =>
{
    op.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    op.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(op =>
{
    op.SaveToken = false;
    op.RequireHttpsMetadata = false;
    op.TokenValidationParameters = new()
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
});

var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// to kill circular in json
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

// services injection
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserManagement, UserManagement>();
builder.Services.AddScoped<IReactService, ReactService>();
builder.Services.AddScoped<IFriendRequestService, FriendRequestService>();
builder.Services.AddScoped<IFriendService, FriendService>();
builder.Services.AddScoped<IFollowerService, FollowerService>();
builder.Services.AddScoped<IBlockService, BlockService>();
builder.Services.AddScoped<IPolicyService, PolicyService>();
builder.Services.AddScoped<IPagePostsService, PagePostsService>();
builder.Services.AddScoped<IGroupPostsService, GroupPostsService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<UserManagerReturn>();
builder.Services.AddScoped<IUserSavedPostsFolderService, UserSavedPostsFolderService>();
builder.Services.AddScoped<ISavedPostsService, SavedPostsService>();
builder.Services.AddScoped<IPostReactsService, PostReactsService>();
builder.Services.AddScoped<IPostCommentService, PostCommentService>();
builder.Services.AddScoped<IPageService, PageService>();
builder.Services.AddScoped<IPagesFollowersService, PagesFollowersService>();
builder.Services.AddScoped<IGroupRolesService, GroupRolesService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IGroupManager, GroupManager>();
builder.Services.AddScoped<IGroupAccessRequestService, GroupAccessRequestService>();
builder.Services.AddScoped<ISarehneService, SarehneService>();

// repositories injection
builder.Services.AddScoped<IReactRepository, ReactRepository>();
builder.Services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
builder.Services.AddScoped<IFriendsRepository, FriendsRepository>();
builder.Services.AddScoped<IFollowerRepository, FollowerRepository>();
builder.Services.AddScoped<IBlockRepository, BlockRepository>();
builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostViewRepository, PostViewRepository>();
builder.Services.AddScoped<IUserSavedPostsFoldersRepository, UserSavedPostsFoldersRepository>();
builder.Services.AddScoped<ISavePostsRepository, SavePostsRepository>();
builder.Services.AddScoped<IPostReactsRepository, PostReactsRepository>();
builder.Services.AddScoped<IPostCommentsRepository, PostCommentsRepository>();
builder.Services.AddScoped<IPageRepository, PageRepository>();
builder.Services.AddScoped<IPagePostsRepository, PagePostsRepository>();
builder.Services.AddScoped<IPagesFollowersRepository, PagesFollowersRepository>();
builder.Services.AddScoped<IGroupRoleRepository, GroupRoleRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IGroupAccessRequestRepository, GroupAccessRequestRepository>();
builder.Services.AddScoped<IGroupMemberRepository, GroupMemberRepository>();
builder.Services.AddScoped<IGroupMemberRoleRepository, GroupMemberRoleRepository>();
builder.Services.AddScoped<IGroupPostsRepository, GroupPostsRepository>();
builder.Services.AddScoped<ISarehneRepository, SarehneRepository>();


builder.Services.AddControllers().AddJsonOptions(op =>
{
    op.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Media Endpoints", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStatusCodePagesWithRedirects("/error/{0}");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
