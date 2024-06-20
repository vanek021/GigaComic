using GigaComic.Models.Entities.Comic;

namespace GigaComic.Services.ImageGeneration.Strategies
{
    public interface IImageGenerationStrategy
    {
        public Task<List<ComicRawImage>> GenerateFor(Comic comic);
    }
}
