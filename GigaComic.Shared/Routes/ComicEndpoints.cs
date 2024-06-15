﻿using GigaComic.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Shared.Routes
{
    public static class ComicEndpoints
    {
        public const string CreateComicByTheme = "api/Comic/CreateComicByTheme";
        public const string CompleteAbstractCreationStage = "api/Comic/CompleteAbstractCreationStage";
        public const string CompleteStoriesCreationStage = "api/Comic/CompleteStoriesCreationStage";
        public const string CompleteSetupStage = "api/Comic/CompleteSetupStage";
        public const string RegenerateAbstractStory = "api/Comic/RegenerateAbstractStory";
        public const string ComicResult = "api/Comic/ComicResult";
        public const string ComicResults = "api/Comic/ComicResult";

        public static string GetComicResult(Guid id)
            => $"{ComicResult}?id={id}";

        public static string GetComicResults(Guid id, int page = 1, int pageSize = PageConstants.DefaultPageSize)
            => $"{ComicResults}?id={id}&page={page}&pageSize={pageSize}";
    }
}
