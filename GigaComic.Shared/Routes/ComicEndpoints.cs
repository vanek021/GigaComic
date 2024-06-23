using GigaComic.Shared.Constants;
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
        public const string CompleteRawImagesEditingStage = "api/Comic/CompleteRawImagesEditingStage";
        public const string RegenerateAbstractStory = "api/Comic/RegenerateAbstractStory";
        public const string Comic = "api/Comic/Comic";
        public const string Comics = "api/Comic/Comics";
        public const string LastThemes = "api/Comic/LastComicThemes";
        public const string RegenerateRawImage = "api/Comic/RegenerateRawImage";

        public static string GetComic(long id)
            => $"{Comic}?id={id}";

        public static string GetComics(int page = 1, int pageSize = PageConstants.DefaultPageSize)
            => $"{Comics}?page={page}&pageSize={pageSize}";
    }
}
