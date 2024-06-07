using GigaComic.Data;
using GigaComic.Models.Entities.Comic;
using GigaComic.Modules.GigaChat;

namespace GigaComic.Services
{
    public class ComicAbstractService : BaseService<ComicAbstract>
    {
        private readonly GigaChatClient _chatClient;

        public ComicAbstractService(AppDbContext dbContext, GigaChatClient chatClient) : base(dbContext)
        {
            _chatClient = chatClient;
        }

        public async Task<List<ComicAbstract>> CreateAbstracts(int n, Comic comic)
        {
            var prompt = $"Напиши историю в {n} действиях. Каждое действие в новой строке. " +
                         $"Только предложения с сюжетом, без нумерации действий. " +
                         $"Без слова действие в начале предложений. " +
                         $"В предложении кратко опиши сюжет действия. Сюжет: {comic.Theme}";


            var answer = await _chatClient.GenerateAnswer(prompt);

            return answer.Split("\n")
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => new ComicAbstract() { Comic = comic, Name = x })
                .ToList();
        }
    }
}
