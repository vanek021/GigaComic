using AutoMapper;
using GigaComic.Models.Entities.Comic;
using GigaComic.Models.Enums;
using GigaComic.Services;
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
        private readonly IMapper _mapper;

        public ComicController(ComicService comicService, ComicAbstractService comicAbstractService, IMapper mapper)
        {
            _comicService = comicService;
            _comicAbstractService = comicAbstractService;
            _mapper = mapper;
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

                var comic = _comicService.Get(model.ComicId);

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

                var comic = _comicService.Get(model.ComicId);

                if (comic is null)
                    return BlazorNotFound($"Theres no comic with {model.ComicId} bruh");
                if (comic.UserId != userId)
                    return BlazorBadRequest($"Cant touch this");

                _mapper.Map(model, comic);

                _comicService.Update(comic);

                return BlazorOk(_mapper.Map<ComicResponse>(comic));
            }
            catch (Exception ex)
            {
                return BlazorBadRequest(ex.Message);
            }
        }
    }
}
