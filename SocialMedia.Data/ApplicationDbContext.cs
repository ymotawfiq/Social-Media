

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialMedia.Data.Models.Authentication;

namespace SocialMedia.Data
{
    public class ApplicationDbContext : IdentityDbContext<SiteUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            SeedRoles(builder);
            ApplyModelsConfigurations(builder);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { ConcurrencyStamp="1", Name= "Admin", NormalizedName = "Admin"},
                new IdentityRole { ConcurrencyStamp = "2", Name = "User", NormalizedName = "User" },
                new IdentityRole { ConcurrencyStamp = "3", Name = "Owner", NormalizedName = "Owner" },
                new IdentityRole { ConcurrencyStamp = "4", Name = "moderator", NormalizedName = "moderator" }
                );
        }

        private void ApplyModelsConfigurations(ModelBuilder builder)
        {

        }

    }
}
