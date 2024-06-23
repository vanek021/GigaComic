using AutoMapper;
using GigaComic.Core.Services.BucketStorage;
using GigaComic.Data;
using GigaComic.Models.Entities.Comic;
using GigaComic.Modules.GigaChat;
using GigaComic.Modules.Kandinsky;
using GigaComic.Shared.Requests.Comic;
using GigaComic.Shared.Responses;
using GigaComic.Shared.Responses.Comic;
using Microsoft.EntityFrameworkCore;
using Sakura.AspNetCore;

namespace GigaComic.Services
{
    public class ComicService : BaseService<Comic>
    {
        private readonly KandinskyApi _kandinskyApi;
        private readonly GigaChatClient _gigaChatClient;
        private readonly IMapper _mapper;
        private readonly IBucket _bucket;

        public ComicService(AppDbContext dbContext, IMapper mapper, IBucketStorageService bucketStorageService/*, KandinskyApi kandinskyApi*/, GigaChatClient gigaChatClient) : base(dbContext)
        {
            //_kandinskyApi = kandinskyApi;
            _gigaChatClient = gigaChatClient;
            _mapper = mapper;
            _bucket = bucketStorageService.GetBucket("comic");
        }

        public async Task CompleteSetup()
        {
            var kdPrompts = "";
        }

        public async Task<PaginatedResult<ComicResponse>> GetPagedComics(long userId, int page, int pageSize)
        {
            var comics = await Get()
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToPagedListAsync(pageSize, page);

            var mappedItems = _mapper.Map<List<ComicResponse>>(comics);

            return PaginatedResult<ComicResponse>.Success(mappedItems, comics.Count, comics.PageIndex, comics.PageIndex);
        }

        public async Task<List<string>> GetLastComicsThemes(long userId)
        {
            var comicThemes = await Get()
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.CreatedAt)
                .Select(c => c.Theme)
                .Distinct()
                .Take(20)
                .ToListAsync();

            return comicThemes;
        }

        public async Task<Comic> GetComicWithIncludes(long comicId)
        {
            var comic = await Get()
                .Where(c => c.Id == comicId)
                .Include(c => c.ComicRawImages)
                .Include(c => c.ComicCompositeImages)
                .SingleAsync();

            return comic;
        }
    }
}
