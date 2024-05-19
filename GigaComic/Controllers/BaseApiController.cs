using GigaComic.Shared.Responses;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GigaComic.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected IActionResult BlazorNotFound(string message)
        {
            return NotFound(Result.Fail(message));
        }

        protected IActionResult BlazorNotFound(List<string> messages)
        {
            return NotFound(Result.Fail(messages));
        }

        protected IActionResult BlazorBadRequest(string message)
        {
            return BadRequest(Result.Fail(message));
        }

        protected IActionResult BlazorBadRequest()
        {
            return BadRequest(Result.Fail());
        }

        protected IActionResult BlazorBadRequest(List<string> messages)
        {
            return BadRequest(Result.Fail(messages));
        }

        protected IActionResult BlazorOk<T>(T message)
        {
            return Ok(Result<T>.Success(message));
        }

        protected IActionResult PaginatedBlazorOk<T>(PaginatedResult<T> message)
        {
            return Ok(message);
        }

        protected IActionResult BlazorOk()
        {
            return Ok(Result.Success());
        }

        protected List<string> GetModelErrors()
        {
            return ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage)).ToList();
        }
    }
}
