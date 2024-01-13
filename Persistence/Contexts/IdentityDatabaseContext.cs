using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts
{
    public class IdentityDatabaseContext : IdentityDbContext<User>
    {
        public IdentityDatabaseContext(DbContextOptions<IdentityDatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            string identitySchema = "identity";

            builder.Entity<IdentityUser<string>>().ToTable("Users", identitySchema);
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", identitySchema);
            builder.Entity<IdentityRole<string>>().ToTable("Roles", identitySchema);
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", identitySchema);
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", identitySchema);
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", identitySchema);
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", identitySchema);

            builder.Entity<IdentityUserLogin<string>>()
                .HasKey(x => new { x.LoginProvider, x.ProviderKey });

            builder.Entity<IdentityUserRole<string>>()
                .HasKey(x => new { x.UserId, x.RoleId });

            builder.Entity<IdentityUserToken<string>>()
                .HasKey(x => new { x.UserId, x.LoginProvider, x.Name });
        }
    }
}