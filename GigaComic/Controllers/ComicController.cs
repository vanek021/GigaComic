using AutoMapper;
using GigaComic.Models.Entities.Comic;
using GigaComic.Models.Enums;
using GigaComic.Services;
using GigaComic.Services.Generation;
using GigaComic.Shared.Constants;
using GigaComic.Shared.Requests.Comic;
using GigaComic.Shared.Responses.Comic;
using GigaComic.Shared.Routes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GigaComic.Controllers
{
    [Authorize]
    public class ComicController : BaseApiController
    {
        private readonly ComicService _comicService;
        private readonly ComicAbstractService _comicAbstractService;
        private readonly ComicImageGenerationService _comicImageGenerationService;
        private readonly IMapper _mapper;

        public ComicController(ComicService comicService, ComicAbstractService comicAbstractService, IMapper mapper, 
            ComicImageGenerationService comicImageGenerationService)
        {
            _comicService = comicService;
            _comicAbstractService = comicAbstractService;
            _mapper = mapper;
            _comicImageGenerationService = comicImageGenerationService;
        }

        [Route(ComicEndpoints.CreateComicByTheme)]
        [HttpPost]
        public async Task<IActionResult> CreateComic([FromBody] CreateComicRequest model)
        {
            try
            {
                var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var comic = _mapper.Map<Comic>(model);
                comic.UserId = userId;
                comic.ComicAbstracts = await _comicAbstractService.CreateAbstracts(5, comic);

                _comicService.Add(comic);

                return BlazorOk(_mapper.Map<ComicResponse>(comic));
            }
            catch (Exception ex)
            {
                return BlazorBadRequest(ex.Message);
            }
        }

        [Route(ComicEndpoints.CompleteAbstractCreationStage)]
        [HttpPost]
        public async Task<IActionResult> CompleteAbstracts([FromBody] CompleteAbstractCreationRequest model)
        {
            try
            {
                var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var comic = _comicService.Get(model.ComicId);

                if (comic is null)
                    return BlazorNotFound($"Theres no comic with {model.ComicId} bruh");
                if (comic.UserId != userId)
                    return BlazorBadRequest($"Cant touch this");

                comic.ComicAbstracts = _mapper.Map<List<ComicAbstract>>(model.Abstracts);
                comic.Stage = ComicStage.StoriesEditing;

                await _comicAbstractService.AddPlots(comic.ComicAbstracts);

                _comicService.Update(comic);

                return BlazorOk(_mapper.Map<ComicResponse>(comic));
            }
            catch (Exception ex)
            {
                return BlazorBadRequest(ex.Message);
            }
        }

        [Route(ComicEndpoints.CompleteStoriesCreationStage)]
        [HttpPost]
        public async Task<IActionResult> CompleteStoriesCreation([FromBody] CompleteStoriesCreationRequest model)
        {
            try
            {
                var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var comic = await _comicService.GetAsync(model.ComicId);

                if (comic is null)
                    return BlazorNotFound($"Theres no comic with {model.ComicId} bruh");
                if (comic.UserId != userId)
                    return BlazorBadRequest($"Cant touch this");

                for (int i = 0; i < comic.ComicAbstracts.Count; i++)
                {
                    comic.ComicAbstracts[i].Content = model.Stories[i].Content;
                }

                comic.Stage = ComicStage.ComicSetup;

                _comicService.Update(comic);

                return BlazorOk(_mapper.Map<ComicResponse>(comic));
            }
            catch (Exception ex)
            {
                return BlazorBadRequest(ex.Message);
            }
        }

        [Route(ComicEndpoints.CompleteSetupStage)]
        [HttpPost]
        public async Task<IActionResult> CompleteSetup([FromBody] ComicSetupRequest model)
        {
            try
            {
                var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

                var comic = await _comicService.GetAsync(model.ComicId);

                if (comic is null)
                    return BlazorNotFound($"Theres no comic with {model.ComicId} bruh");
                if (comic.UserId != userId)
                    return BlazorBadRequest($"Cant touch this");

                _mapper.Map(model, comic);

                comic.Stage = ComicStage.RawImagesEditing;

                _comicService.Update(comic);

                await _comicImageGenerationService.PrepareRawImages(comic);

                return BlazorOk(_mapper.Map<ComicResponse>(comic));
            }
            catch (Exception ex)
            {
                return BlazorBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route(ComicEndpoints.Comic)]
        public async Task<IActionResult> GetComic(long id)
        {
            try
            {
                var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var comic = await _comicService.GetAsync(id);

                if (comic is null || comic.UserId != userId)
                    return BlazorNotFound($"Не существует комикса с указанным id.");

                var response = _mapper.Map<ComicResponse>(comic);

                return BlazorOk(response);
            }
            catch (Exception ex)
            {
                return BlazorBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route(ComicEndpoints.Comics)]
        public async Task<IActionResult> GetComics(int page, int pageSize)
        {
            try
            {
                var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var comics = await _comicService.GetPagedComics(userId, page, pageSize);

                return PaginatedBlazorOk(comics);
            }
            catch (Exception ex)
            {
                return BlazorBadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route(ComicEndpoints.LastThemes)]
        public async Task<IActionResult> GetLastComicThemes()
        {
            try
            {
                var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var comicThemes = await _comicService.GetLastComicsThemes(userId);

                return BlazorOk(comicThemes);
            }
            catch (Exception ex)
            {
                return BlazorBadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route(ComicEndpoints.RegenerateRawImage)]
        public async Task<IActionResult> RegenerateRawImage(RegenerateRawImageRequest model)
        {
            try
            {
                var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var rawImage = await _comicImageGenerationService.RegenerateRawImage(model, userId);
                var response = _mapper.Map<ComicRawImageResponse>(rawImage);

                return BlazorOk(response);
            }
            catch (Exception ex)
            {
                return BlazorBadRequest(ex.Message);
            }
        }
    }
}
