﻿using GigaComic.Data;
using GigaComic.Data.Migrations;
using GigaComic.Models.Entities.Comic;
using GigaComic.Modules.GigaChat;
using System.Text.RegularExpressions;

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

            var answers = answer.Split("\n")
                .Where(x => !string.IsNullOrEmpty(x))
            .ToList();

            if (answers.Count < 3)
            {
                string[] sentences = Regex.Split(answer, @"(?<=[\.!\?])\s+");
                answers = [.. sentences];
            } 

            return answers.Select(x => new ComicAbstract() { Comic = comic, Name = x })
                .ToList();
        }

        public async Task AddPlots(List<ComicAbstract> abstracts)
        {
            var answer = await _chatClient.GenerateAnswer(BuildPromptForPlotAdd(abstracts));

            var plots = answer.Split("\n")
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            var j = 0;
            var retryCount = 5;

            while ((plots.Count < abstracts.Count || 
                plots.Any(p => abstracts.Any(a => a.Name == p))) &&
                j < retryCount)
            {
                var prompt = $"Сюжетов должно быть {abstracts.Count}. " +
                    $"Сгенерируй мне {abstracts.Count} сюжетов каждая в НОВОЙ СТРОКЕ. " +
                    "Ты молодец у тебя получится";

                answer = await _chatClient.GenerateAnswer(prompt);

                plots = answer.Split("\n")
                    .Where(x => !string.IsNullOrEmpty(x))
                    .ToList();

                j++;
            }

            if (plots.Count < abstracts.Count ||
                plots.Any(p => abstracts.Any(a => a.Name == p)))
                throw new Exception("Ai vashe kapec dawn");

            for (int i = 0; i < abstracts.Count; i++)
            {
                abstracts[i].Content = plots[i];
            }
        }

        public string BuildPromptForPlotAdd(List<ComicAbstract> abstracts)
        {
            var strAbstracts = string.Join("\n", abstracts.Select(x => x.Name));
            var prompt = "Тебе даны краткие сюжеты действий комикса. Напиши ОДИН развернутый сюжет для каждого действия.\r\n" +
                         "Каждый сюжет с новой строки. Один сюжет ОДНА строка. Развернутые сюжеты должны быть длиннее кратких сюжетов. \r\n Действия: \r\n" +
                         strAbstracts;

            return prompt;
        }
    }
}
