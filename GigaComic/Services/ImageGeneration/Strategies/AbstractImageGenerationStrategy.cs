using GigaComic.Core.Services.BucketStorage;
using GigaComic.Data;
using GigaComic.Models.Entities.Comic;
using GigaComic.Modules.GigaChat;
using GigaComic.Modules.Kandinsky;

namespace GigaComic.Services.ImageGeneration.Strategies
{
    public abstract class AbstractImageGenerationStrategy : IImageGenerationStrategy
    {
        protected readonly GigaChatClient _chatClient;
        protected readonly KandinskyApi _kandinskyApi;
        protected readonly AppDbContext _appDbCtx;
        protected readonly IBucket _bucket;

        public AbstractImageGenerationStrategy(GigaChatClient chatClient, KandinskyApi kandinskyApi, IBucket bucket, AppDbContext appDbContext)
        {
            _chatClient = chatClient;
            _kandinskyApi = kandinskyApi;
            _bucket = bucket;
            _appDbCtx = appDbContext;
        }

        public abstract Task<List<ComicRawImage>> GenerateFor(Comic comic);
    }
}
