using GigaComic.Shared.Requests.Comic;
using GigaComic.Shared.Responses.Comic;
using GigaComic.Shared.Responses;
using GigaComic.Shared.Constants;

namespace GigaComic.Client.Managers.Comic
{
    public interface IComicManager : IManager
    {
        Task<IResult<ComicResponse>> CreateComicByThemeAsync(CreateComicRequest model);

        Task<IResult<ComicResponse>> CompleteAbstractCreationStageAsync(CompleteAbstractCreationRequest model);

        Task<IResult<ComicResponse>> CompleteStoriesCreationStageAsync(CompleteStoriesCreationRequest model);

        Task<IResult<ComicResponse>> CompleteRawImagesEditingStageAsync(CompleteRawImagesEditingRequest model);

        Task<IResult<ComicResponse>> CompleteSetupStageAsync(ComicSetupRequest model);

        Task<IResult<ComicResponse>> GetComicAsync(long id);

        Task<PaginatedResult<ComicResponse>> GetComicsAsync(int page, int pageSize = PageConstants.DefaultPageSize);

        Task<IResult<List<string>?>> GetLastComicThemesAsync();

        Task<IResult<ComicRawImageResponse>> RegenerateRawImageAsync(RegenerateRawImageRequest model);
    }
}
