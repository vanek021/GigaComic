using GigaComic.Core.Models.Kandinsky.Responses;
using GigaComic.Core.Services.BucketStorage;
using GigaComic.Data;
using GigaComic.Infrastructure.Helpers;
using GigaComic.Models.Entities.Comic;
using GigaComic.Modules.GigaChat;
using GigaComic.Modules.Kandinsky;
using GigaComic.Services.Generation;
using Hangfire;
using System.ComponentModel;

namespace GigaComic.Services.ImageGeneration.Strategies
{
    /// <summary>
    /// Генерация описания вместе с генерацией запросов к Кандинскому
    /// </summary>
    public class GigachatOneRequestGenerationStrategy(GigaChatClient chatClient, KandinskyApi kandinskyApi, IBucket bucket, AppDbContext appDbContext)
        : AbstractImageGenerationStrategy(chatClient, kandinskyApi, bucket, appDbContext)
    {
        public override async Task<List<ComicRawImage>> GenerateFor(Comic comic)
        {
            var result = new List<ComicRawImage>();

            var fullStory = string.Join(" ", comic.ComicAbstracts.Select(x => x.Content));

            var gigachatPrompt = $"Сгенерируй как можно больше запросов для нейросети генерации картинок с целью создания комикса на основе его содержания. " +
                $"К каждой картинке сгенерируй текстовое описание. " +
                $"Выведи результаты в формате: \"Запрос для нейросети;Описание\".\"\n" +
                $"Содержание: \"{fullStory}\"";

            var unprocessedImages = new List<(string Title, GenerateResponse? GeneratedImage)>();

            var answer = await _chatClient.GenerateAnswer(gigachatPrompt);

            var splitted = answer.Split("\n")
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            foreach (var splittedItem in splitted)
            {
                var splittedQuery = splittedItem.Split(';');

                var kandinskyPrompt = splittedQuery[0];
                var gigaChatDesc = splittedQuery.Length > 1 ? splittedQuery[1] : kandinskyPrompt;

                gigaChatDesc = gigaChatDesc.Replace("Описание:", "").Trim();

                result.Add(new ComicRawImage()
                {
                    GeneratingRequest = kandinskyPrompt,
                    Title = gigaChatDesc,
                    CreatedAt = DateTime.UtcNow,
                    State = Models.Enums.RawImageState.Created,
                    Order = splitted.IndexOf(splittedItem),
                    ComicId = comic.Id
                });
            }

            _appDbCtx.AddRange(result);
            await _appDbCtx.SaveChangesAsync();

            comic.ComicRawImages = result;

            RecurringJob.TriggerJob("GenerateRawImages");

            return result;
        }
    }
}
