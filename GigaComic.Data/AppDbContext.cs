using GigaComic.Core.Data;
using GigaComic.Core.Entities;
using GigaComic.Models.Entities;
using GigaComic.Models.Entities.Comic;
using Microsoft.EntityFrameworkCore;

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

        public DbSet<Comic> Comics { get; set; }
        public DbSet<ComicAbstract> ComicAbstracts { get; set; }
    }
}
