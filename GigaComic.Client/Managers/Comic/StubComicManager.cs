using GigaComic.Models.Enums;
using GigaComic.Shared.Requests.Comic;
using GigaComic.Shared.Responses;
using GigaComic.Shared.Responses.Comic;

namespace GigaComic.Client.Managers.Comic
{
    public class StubComicManager : IComicManager
    {
        public async Task<IResult<ComicResponse>> CompleteAbstractCreationStageAsync(CompleteAbstractCreationRequest model)
        {
            var result = await CreateComicByThemeAsync(new CreateComicRequest());

            foreach (var data in result.Data.OrderedActiveAbstracts)
            {
                data.Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
            }

            result.Data.Stage = ComicStage.StoriesEditing;

            return result;
        }

        public async Task<IResult<ComicResponse>> CompleteSetupStageAsync(ComicSetupRequest model)
        {
            var result = await CompleteStoriesCreationStageAsync(new CompleteStoriesCreationRequest());

            result.Data.Stage = ComicStage.Completed;

            return result;
        }

        public async Task<IResult<ComicResponse>> CompleteStoriesCreationStageAsync(CompleteStoriesCreationRequest model)
        {
            var result = await CompleteAbstractCreationStageAsync(new CompleteAbstractCreationRequest());

            result.Data.Stage = ComicStage.ComicSetup;

            return result;
        }

        public async Task<IResult<ComicResponse>> CreateComicByThemeAsync(CreateComicRequest model)
        {
            return await Task.FromResult<IResult<ComicResponse>>(Result<ComicResponse>.Success(new ComicResponse()
            {
                Id = 0,
                ComicAbstracts =
                [
                    new()
                    {
                        Id = 0,
                        IsActive = true,
                        Name = "Тезис 0",
                        Order = 0,
                    },
                    new()
                    {
                        Id = 1,
                        IsActive = true,
                        Name = "Тезис 1",
                        Order = 1,
                    },
                    new()
                    {
                        Id = 2,
                        IsActive = true,
                        Name = "Тезис 2 Тезис 2 Тезис 2",
                        Order = 2,
                    },
                    new()
                    {
                        Id = 3,
                        IsActive = true,
                        Name = "Тезис 3 Тезис 3 Тезис 3 Тезис 3 Тезис 3",
                        Order = 3,

                    },
                    new()
                    {
                        Id = 4,
                        IsActive = true,
                        Name = "Тезис 4 Тезис 4 Тезис 4 Тезис 4 Тезис 4 Тезис 4 Тезис 4 Тезис 4",
                        Order = 4,
                    },
                    new()
                    {
                        Id = 5,
                        IsActive = true,
                        Name = "Тезис 5 Тезис 5 Тезис 5 Тезис 5 Тезис 5 Тезис 5 Тезис 5 Тезис 5 Тезис 5 Тезис 5 Тезис 5",
                        Order = 5,
                    },
                ],
                Stage = ComicStage.AbstractsCreation,
            }));
        }
    }
}
