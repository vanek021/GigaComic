using GigaComic.Shared.Requests.Comic;
using GigaComic.Shared.Responses.Comic;
using GigaComic.Shared.Responses;

namespace GigaComic.Client.Managers.Comic
{
    public interface IComicManager : IManager
    {
        Task<IResult<ComicResponse>> CreateComicByThemeAsync(CreateComicRequest model);

        Task<IResult<ComicResponse>> CompleteAbstractCreationStageAsync(CompleteAbstractCreationRequest model);
    }
}
