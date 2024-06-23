using GigaComic.Core.Data;
using GigaComic.Core.Entities;
using GigaComic.Models.Entities;
using GigaComic.Models.Entities.Comic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace GigaComic.Data
{
    public class AppDbContext : BaseDbContext<User, BaseRole>
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLazyLoadingProxies();
        }

        public class Factory : IDesignTimeDbContextFactory<AppDbContext>
        {
            public AppDbContext CreateDbContext(string[] args)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(System.IO.Directory.GetCurrentDirectory() + @"\..\GigaComic")
                    .AddJsonFile("appsettings.Development.json")
                    .Build();

                var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseNpgsql(configuration.GetConnectionString("DefaultConnection")!).Options;

                return new AppDbContext(options);
            }
        }

        public DbSet<Comic> Comics { get; set; }
        public DbSet<ComicAbstract> ComicAbstracts { get; set; }
        public DbSet<ComicRawImage> ComicRawImages { get; set; }
        public DbSet<ComicCompositeImage> ComicCompositeImages { get; set; }
    }
}
