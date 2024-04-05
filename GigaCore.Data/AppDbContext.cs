using GigaComic.Core.Data;
using GigaComic.Core.Entities;
using GigaComic.Models.Entities;
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
    }
}
