using GigaComic.Core.Entities;
using GigaComic.Core.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IDKEY = System.Int64;

namespace GigaComic.Core.Data
{
    public class BaseDbContext<TUser, TRole> : IdentityDbContext<TUser, TRole, IDKEY, BaseUserClaim, BaseUserRole, BaseUserLogin, BaseRoleClaim, BaseUserToken>
        where TUser : BaseUser
        where TRole : BaseRole
    {
        public BaseDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ProcessAnnotationAttributes(this);
            ConfigureIdentityTables(builder);
        }

        private void ConfigureIdentityTables(ModelBuilder builder)
        {
            builder.Entity<TUser>().ToTable("Users");
            builder.Entity<TRole>().ToTable("Roles");
            builder.Entity<BaseUserRole>().ToTable("UserRoles");
            builder.Entity<BaseUserClaim>().ToTable("UserClaims");
            builder.Entity<BaseUserLogin>().ToTable("UserLogins");
            builder.Entity<BaseRoleClaim>().ToTable("RoleClaims");
            builder.Entity<BaseUserToken>().ToTable("UserTokens");

            // ASP .NET Core 1.1 compatibility:

            builder.Entity<TUser>()
                .HasMany(e => e.Claims)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TUser>()
                .HasMany(e => e.Logins)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TUser>()
                .HasMany(e => e.Roles)
                .WithOne()
                .HasForeignKey(e => e.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
