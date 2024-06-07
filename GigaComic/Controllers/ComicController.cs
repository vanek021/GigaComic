using AutoMapper;
using GigaComic.Models.Entities.Comic;
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
            var userId = long.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var comic = _mapper.Map<Comic>(model);
            comic.UserId = userId;
            comic.ComicAbstracts = await _comicAbstractService.CreateAbstracts(5, comic);
            
            _comicService.Add(comic);

            return BlazorOk(_mapper.Map<ComicResponse>(comic));
        }
    }
}
