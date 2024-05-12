using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SocialMedia.Data;
using SocialMedia.Data.Models.Authentication;
using SocialMedia.Data.Models.EmailModel;
using SocialMedia.Repository.AccountPolicyRepository;
using SocialMedia.Repository.BlockRepository;
using SocialMedia.Repository.CommentPolicyRepository;
using SocialMedia.Repository.FollowerRepository;
using SocialMedia.Repository.FriendListPolicyRepository;
using SocialMedia.Repository.FriendRequestRepository;
using SocialMedia.Repository.FriendsRepository;
using SocialMedia.Repository.PolicyRepository;
using SocialMedia.Repository.PostRepository;
using SocialMedia.Repository.PostViewRepository;
using SocialMedia.Repository.ReactPolicyRepository;
using SocialMedia.Repository.ReactRepository;
using SocialMedia.Repository.UserPostsRepository;
using SocialMedia.Service.AccountPolicyService;
using SocialMedia.Service.BlockService;
using SocialMedia.Service.CommentPolicyService;
using SocialMedia.Service.FollowerService;
using SocialMedia.Service.FriendListPolicyService;
using SocialMedia.Service.FriendRequestService;
using SocialMedia.Service.FriendsService;
using SocialMedia.Service.PolicyService;
using SocialMedia.Service.PostService;
using SocialMedia.Service.ReactPolicyService;
using SocialMedia.Service.ReactService;
using SocialMedia.Service.SendEmailService;
using SocialMedia.Service.UserAccountService;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// database configurations
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(op =>
{
    op.UseSqlServer(connection);
    op.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
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

// services injection
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IUserManagement, UserManagement>();
builder.Services.AddScoped<IReactService, ReactService>();
builder.Services.AddScoped<IFriendRequestService, FriendRequestService>();
builder.Services.AddScoped<IFriendService, FriendService>();
builder.Services.AddScoped<IFollowerService, FollowerService>();
builder.Services.AddScoped<IBlockService, BlockService>();
builder.Services.AddScoped<IPolicyService, PolicyService>();
builder.Services.AddScoped<IReactPolicyService, ReactPolicyService>();
builder.Services.AddScoped<ICommentPolicyService, CommentPolicyService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IUserPostsRepository, UserPostsRepository>();
builder.Services.AddScoped<IAccountPolicyService, AccountPolicyService>();
builder.Services.AddScoped<IFriendListPolicyService, FriendListPolicyService>();
// repositories injection
builder.Services.AddScoped<IReactRepository, ReactRepository>();
builder.Services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
builder.Services.AddScoped<IFriendsRepository, FriendsRepository>();
builder.Services.AddScoped<IFollowerRepository, FollowerRepository>();
builder.Services.AddScoped<IBlockRepository, BlockRepository>();
builder.Services.AddScoped<IPolicyRepository, PolicyRepository>();
builder.Services.AddScoped<IReactPolicyRepository, ReactPolicyRepository>();
builder.Services.AddScoped<ICommentPolicyRepository, CommentPolicyRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IAccountPolicyRepository, AccountPolicyRepository>();
builder.Services.AddScoped<IPostViewRepository, PostViewRepository>();
builder.Services.AddScoped<IFriendListPolicyRepository, FriendListPolicyRepository>();

builder.Services.AddControllers().AddJsonOptions(op =>
{
    op.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Social Media Website Endpoints", Version = "v1" });
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
