using GigaComic.Models.Entities.Comic;
using GigaComic.Modules.GigaChat;
using GigaComic.Modules.Kandinsky;

namespace GigaComic.Services.ImageGeneration.Strategies
{
    public abstract class AbstractImageGenerationStrategy : IImageGenerationStrategy
    {
        protected readonly GigaChatClient _chatClient;
        protected readonly KandinskyApi _kandinskyApi;

        public AbstractImageGenerationStrategy(GigaChatClient chatClient, KandinskyApi kandinskyApi)
        {
            _chatClient = chatClient;
            _kandinskyApi = kandinskyApi;
        }

        public abstract Task<List<ComicRawImage>> GenerateFor(Comic comic);
    }
}
