using GigaComic.Core.Attributes;
using GigaComic.Core.Data;
using GigaComic.Core.Entities;
using GigaComic.Core.Internal;
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
    public static class ServicesRegistrationExtensions
    {
        public static IServiceCollection AddLowercaseRouting(this IServiceCollection services)
        {
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });
            services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = options.AccessDeniedPath.ToString().ToLowerInvariant();
                options.LoginPath = options.LoginPath.ToString().ToLowerInvariant();
                options.LogoutPath = options.LogoutPath.ToString().ToLowerInvariant();
                //options.ReturnUrlParameter = options.ReturnUrlParameter.ToString().ToLowerInvariant();
            });

            return services;
        }

        public static IServiceCollection AddBasePgsqlContext<TContext>(this IServiceCollection services, string connectionString)
            where TContext : DbContext
        {
            services.AddBaseDbContext<TContext>(options => options.UseNpgsql(connectionString));
            return services;
        }

        public static IServiceCollection AddBaseDbContext<TContext>(this IServiceCollection services, Action<DbContextOptionsBuilder>? optionsAction = null)
            where TContext : DbContext
        {
            services.AddDbContext<TContext>(optionsAction);
            services.AddScoped<DbContext>(x => x.GetService<TContext>()!);

            ReflectionTools.CallGenericStaticMethodForDbContextType<TContext>(typeof(ServicesRegistrationExtensions), nameof(RegisterGenericBaseContext), new object[] { services });

            return services;
        }

        public static IServiceCollection RegisterInjectableTypesFromAssemblies(this IServiceCollection services, params Type[] assemblyMarkerTypes)
        {
            var assemblies = assemblyMarkerTypes.Select(x => x.Assembly);
            foreach (var asm in assemblies)
            {
                var all = asm.GetTypes().Where(x => x.CustomAttributes.Any(it => it.AttributeType.FullName == "GigaComic.Core.Attributes.InjectableAttribute"));
                foreach (var it in all)
                    RegisterInjectableType(services, it);
            }

            return services;
        }

        private static void RegisterInjectableType(IServiceCollection services, Type type)
        {
            services.AddScoped(type);

            var attrs = type.GetCustomAttributes(typeof(InjectableAttribute), true).OfType<InjectableAttribute>();
            foreach (var atr in attrs)
            {
                if (atr.ServiceType != null)
                    services.AddScoped(atr.ServiceType, x => x.GetRequiredService(type));
            }
        }

        [Obfuscation(Exclude = true)]
        private static void RegisterGenericBaseContext<TContext, TUser, TRole>(IServiceCollection services)
            where TContext : BaseDbContext<TUser, TRole>
            where TUser : BaseUser
            where TRole : BaseRole
        {
            services.AddScoped<BaseDbContext<TUser, TRole>>(x => x.GetService<TContext>()!);
        }
    }
}
