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

        public async Task AddPlots(List<ComicAbstract> abstracts)
        {
            var answer = await _chatClient.GenerateAnswer(BuildPromptForPlotAdd(abstracts));

            var plots = answer.Split("\n")
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            if (plots.Count < abstracts.Count)
                throw new Exception("Ai vashe kapec dawn");

            for (int i = 0; i < abstracts.Count; i++)
            {
                abstracts[i].Content = plots[i];
            }
        }

        public string BuildPromptForPlotAdd(List<ComicAbstract> abstracts)
        {
            var strAbstracts = string.Join("\n", abstracts.Select(x => x.Name));
            var prompt = "Тебе даны действия комикса. Напиши ОДИН развернутый сюжет для каждого действия.\r\n" +
                         "Каждый сюжет с новой строки. Один сюжет ОДНА строка \r\n" +
                         strAbstracts;

            return prompt;
        }
    }
}
