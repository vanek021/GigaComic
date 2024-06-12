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
    }
}
