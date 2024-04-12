using GigaComic.Data;
using GigaComic.Models.Entities.Comic;

namespace GigaComic.Services
{
    public class ComicService : BaseService<Comic>
    {
        public ComicService(AppDbContext dbContext) : base(dbContext)
        {

        }
    }
}
