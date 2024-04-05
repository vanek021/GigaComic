using GigaComic.Core.Data;
using GigaComic.Core.DataAnnotations;
using GigaComic.Core.Entities;
using GigaComic.Core.Internal;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Core.Extensions
{
    public static class IdentityExtensions
    {
        public static void AddApplicationIdentity<TContext>(this IServiceCollection services, Action<IdentityOptions>? setupAction = null)
            where TContext : DbContext
        {
            // Default Ability configuration
            setupAction ??= options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;

                options.Stores.MaxLengthForKeys = 128;
            };

            ReflectionTools.CallGenericStaticMethodForDbContextType<TContext>(typeof(IdentityExtensions), nameof(RegisterStores), new object[] { services, setupAction });
        }

        [Obfuscation(Exclude = true)]
        private static void RegisterStores<TContext, TUser, TRole>(IServiceCollection services, Action<IdentityOptions> setupAction)
            where TContext : BaseDbContext<TUser, TRole>
            where TUser : BaseUser
            where TRole : BaseRole
        {
            var config = services.AddIdentity<TUser, TRole>(setupAction)
                .AddDefaultTokenProviders()
                .AddUserStore<BaseUserStore<TContext, TUser, TRole>>()
                .AddRoleStore<BaseRoleStore<TContext, TUser, TRole>>()
                .AddUserManager<BaseUserManager<TUser>>()
                .AddSignInManager<BaseSignInManager<TUser>>()
                .AddRoleManager<BaseRoleManager<TUser, TRole>>();

            config
                //.AddDefaultUI()
                .AddDefaultTokenProviders();
        }

        public static void ProcessAnnotationAttributes(this ModelBuilder builder, DbContext context)
        {
            var properties = context.GetType().GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            foreach (var cur in properties)
            {
                var ptype = cur.PropertyType.GetTypeInfo();
                if (ptype.IsGenericType && ptype.GetGenericTypeDefinition().Equals(typeof(DbSet<>)))
                {
                    var entityType = ptype.GetGenericArguments().Single();
                    var columns = entityType.GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    UniqueAttribute.CheckAttribute(builder, entityType, columns);
                }
            }
        }
    }
}
