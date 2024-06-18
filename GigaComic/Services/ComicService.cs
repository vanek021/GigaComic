using AutoMapper;
using GigaComic.Core.Services.BucketStorage;
using GigaComic.Data;
using GigaComic.Models.Entities.Comic;
using GigaComic.Modules.GigaChat;
using GigaComic.Shared.Responses;
using GigaComic.Shared.Responses.Comic;
using Sakura.AspNetCore;

namespace GigaComic.Services
{
    public class ComicService : BaseService<Comic>
    {
        private readonly IMapper _mapper;
        private readonly IBucket _bucket;

        public ComicService(AppDbContext dbContext, IMapper mapper, IBucketStorageService bucketStorageService) : base(dbContext)
        {
            _mapper = mapper;
            _bucket = bucketStorageService.GetBucket("comic");
        }

        public async Task<PaginatedResult<ComicResponse>> GetPagedComics(long userId, int page, int pageSize)
        {
            var comics = await Get()
                .Where(c => c.UserId == userId)
                .ToPagedListAsync(pageSize, page);

            var mappedItems = _mapper.Map<List<ComicResponse>>(comics);

            return PaginatedResult<ComicResponse>.Success(mappedItems, comics.Count, comics.PageIndex, comics.PageIndex);
        }
    }
}
