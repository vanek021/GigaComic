using GigaComic.Infrastructure.Helpers;
using GigaComic.Models.Entities.Comic;
using GigaComic.Modules.GigaChat;
using GigaComic.Modules.Kandinsky;
using System.ComponentModel;

namespace GigaComic.Services.ImageGeneration.Strategies
{
    /// <summary>
    /// Генерация описания вместе с генерацией запросов к Кандинскому
    /// </summary>
    public class GigachatOneRequestGenerationStrategy(GigaChatClient chatClient, KandinskyApi kandinskyApi) 
        : AbstractImageGenerationStrategy(chatClient, kandinskyApi)
    {
        public override async Task<List<ComicRawImage>> GenerateFor(Comic comic)
        {
            var result = new List<ComicRawImage>();

            var fullStory = string.Join(" ", comic.ComicAbstracts.Select(x => x.Content));

            var gigachatPrompt = $"Сгенерируй как можно больше запросов для нейросети генерации картинок с целью создания комикса на основе его содержания. " +
                $"К каждой картинке сгенерируй текстовое описание. " +
                $"Выведи результаты в формате: \"Запрос для нейросети;Описание\".\"\n" +
                $"Содержание: \"{fullStory}\"";


            CallbackRetrier.ExecuteWithRetries(async () =>
            {
                result.Clear();

                var answer = await _chatClient.GenerateAnswer(gigachatPrompt);

                var splitted = answer.Split("\n")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

                foreach (var splittedItem in splitted)
                {
                    var kandinskyPrompt = splittedItem.Split(';')[0];
                    var gigaChatDesc = splittedItem.Split(';')[1];

                    var kndResponse = await _kandinskyApi.Generate(kandinskyPrompt);
                    
                }
            });

            return result;
        }
    }
}
