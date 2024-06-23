using GigaComic.Shared.Constants;
using GigaComic.Shared.Extensions;
using GigaComic.Shared.Requests.Comic;
using GigaComic.Shared.Responses;
using GigaComic.Shared.Responses.Comic;
using GigaComic.Shared.Routes;
using System.Net.Http.Json;

namespace GigaComic.Client.Managers.Comic
{
    public class ComicManager : IComicManager
    {
        private readonly HttpClient _httpClient;

        public ComicManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<ComicResponse>> CompleteAbstractCreationStageAsync(CompleteAbstractCreationRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync(ComicEndpoints.CompleteAbstractCreationStage, model);
            var result = await response.ToResultAsync<ComicResponse>();
            return result;
        }

        public async Task<IResult<ComicResponse>> CompleteSetupStageAsync(ComicSetupRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync(ComicEndpoints.CompleteSetupStage, model);
            var result = await response.ToResultAsync<ComicResponse>();
            return result;
        }

        public async Task<IResult<ComicResponse>> CompleteStoriesCreationStageAsync(CompleteStoriesCreationRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync(ComicEndpoints.CompleteStoriesCreationStage, model);
            var result = await response.ToResultAsync<ComicResponse>();
            return result;
        }

        public async Task<IResult<ComicResponse>> CreateComicByThemeAsync(CreateComicRequest model)
        {
            var response = await _httpClient.PostAsJsonAsync(ComicEndpoints.CreateComicByTheme, model);
            var result = await response.ToResultAsync<ComicResponse>();
            return result;
        }

        public async Task<IResult<ComicResponse>> GetComicAsync(long id)
        {
            var response = await _httpClient.GetAsync(ComicEndpoints.GetComic(id));
            var result = await response.ToResultAsync<ComicResponse>();
            return result;
        }

        public async Task<PaginatedResult<ComicResponse>> GetComicsAsync(int page, int pageSize = PageConstants.DefaultPageSize)
        {
            var response = await _httpClient.GetAsync(ComicEndpoints.GetComics(page, pageSize));
            var result = await response.ToPaginatedResultAsync<ComicResponse>();
            return result;
        }

        public async Task<IResult<List<string>?>> GetLastComicThemesAsync()
        {
            var response = await _httpClient.GetAsync(ComicEndpoints.LastThemes);
            var result = await response.ToResultAsync<List<string>?>();
            return result;
        }
    }
}
