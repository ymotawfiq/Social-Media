

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialMedia.Api.Data;
using SocialMedia.Api.Data.Models.Authentication;
using SocialMedia.Api.Data.Models.EmailModel;
using SocialMedia.Api.Repository.BlockRepository;
using SocialMedia.Api.Repository.FollowerRepository;
using SocialMedia.Api.Repository.FriendRequestRepository;
using SocialMedia.Api.Repository.FriendsRepository;
using SocialMedia.Api.Repository.GroupAccessRequestRepository;
using SocialMedia.Api.Repository.GroupMemberRoleRepository;
using SocialMedia.Api.Repository.GroupPostsRepository;
using SocialMedia.Api.Repository.GroupRepository;
using SocialMedia.Api.Repository.RoleRepository;
using SocialMedia.Api.Repository.PagePostsRepository;
using SocialMedia.Api.Repository.PageRepository;
using SocialMedia.Api.Repository.PagesFollowersRepository;
using SocialMedia.Api.Repository.PolicyRepository;
using SocialMedia.Api.Repository.PostCommentsRepository;
using SocialMedia.Api.Repository.PostReactsRepository;
using SocialMedia.Api.Repository.PostRepository;
using SocialMedia.Api.Repository.PostViewRepository;
using SocialMedia.Api.Repository.ReactRepository;
using SocialMedia.Api.Repository.SarehneRepository;
using SocialMedia.Api.Repository.SavePostsRepository;
using SocialMedia.Api.Repository.UserSavedPostsFoldersRepository;
using SocialMedia.Api.Service.BlockService;
using SocialMedia.Api.Service.FollowerService;
using SocialMedia.Api.Service.FriendRequestService;
using SocialMedia.Api.Service.FriendsService;
using SocialMedia.Api.Service.GenericReturn;
using SocialMedia.Api.Service.GroupAccessRequestService;
using SocialMedia.Api.Service.GroupManager;
using SocialMedia.Api.Service.GroupPostsService;
using SocialMedia.Api.Service.RolesService;
using SocialMedia.Api.Service.GroupService;
using SocialMedia.Api.Service.PagePostsService;
using SocialMedia.Api.Service.PageService;
using SocialMedia.Api.Service.PagesFollowersService;
using SocialMedia.Api.Service.PolicyService;
using SocialMedia.Api.Service.PostCommentService;
using SocialMedia.Api.Service.PostReactsService;
using SocialMedia.Api.Service.PostService;
using SocialMedia.Api.Service.ReactService;
using SocialMedia.Api.Service.SarehneService;
using SocialMedia.Api.Service.SavedPostsService;
using SocialMedia.Api.Service.SendEmailService;
using SocialMedia.Api.Service.UserSavedPostsFoldersService;
using System.Text;
using System.Text.Json.Serialization;
using SocialMedia.Api.Repository.GroupMemberRepository;
using SocialMedia.Api.Repository.ChatRepository;
using SocialMedia.Api.Service.ChatService;
using SocialMedia.Api.Repository.ChatMemberRepository;
using SocialMedia.Api.Repository.ChatMemberRoleRepository;
using SocialMedia.Api.Service.ChatManagerService;
using SocialMedia.Api.Repository.PrivateChatRepository;
using SocialMedia.Api.Repository.ChatMessageRepository;
using SocialMedia.Api.Repository.MessageReactRepository;
using SocialMedia.Api.Repository.ArchievedChatRepository;
using SocialMedia.Api.Service.ChatMessageService;
using SocialMedia.Api.Service.MessageReactService;
using SocialMedia.Api.Service.ArchievedChatService;
using SocialMedia.Api.Service.AccountService.TokenService;
using SocialMedia.Api.Service.AccountService.SettingsService;
using SocialMedia.Api.Service.AccountService.TwoFactoAuthenticationService;
using SocialMedia.Api.Service.AccountService.UserAccountService;
using SocialMedia.Api.Service.AccountService.EmailService;
using SocialMedia.Api.Service.AccountService.UserRolesService;

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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]!))
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
builder.Services.AddScoped<ISendEmailService, SendEmailService>();
builder.Services.AddScoped<IUserRolesService, UserRolesService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddScoped<ITwoFactoAuthenticationService, TwoFactoAuthenticationService>();
builder.Services.AddScoped<IUserAccountService, UserAccountService>();
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
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IGroupService, GroupService>();
builder.Services.AddScoped<IGroupManager, GroupManager>();
builder.Services.AddScoped<IGroupAccessRequestService, GroupAccessRequestService>();
builder.Services.AddScoped<ISarehneService, SarehneService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IChatManagerService, ChatManagerService>();
builder.Services.AddScoped<IChatMessageService, ChatMessageService>();
builder.Services.AddScoped<IMessageReactService, MessageReactService>();
builder.Services.AddScoped<IArchievedChatService, ArchievedChatService>();

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
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IGroupRepository, GroupRepository>();
builder.Services.AddScoped<IGroupAccessRequestRepository, GroupAccessRequestRepository>();
builder.Services.AddScoped<IGroupMemberRepository, GroupMemberRepository>();
builder.Services.AddScoped<IGroupMemberRoleRepository, GroupMemberRoleRepository>();
builder.Services.AddScoped<IGroupPostsRepository, GroupPostsRepository>();
builder.Services.AddScoped<ISarehneRepository, SarehneRepository>();
builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IChatMemberRepository, ChatMemberRepository>();
builder.Services.AddScoped<IChatMemberRoleRepository, ChatMemberRoleRepository>();
builder.Services.AddScoped<IPrivateChatRepository, PrivateChatRepository>();
builder.Services.AddScoped<IChatMessageRepository, ChatMessageRepository>();
builder.Services.AddScoped<IMessageReactRepository, MessageReactRepository>();
builder.Services.AddScoped<IArchievedChatRepository, ArchievedChatRepository>();



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
