using System.Collections.Generic;

namespace GigaComic.Shared.Responses
{
    public interface IResult
    {
        List<string> Messages { get; set; }

        bool Succeeded { get; set; }

        string GetMessages();
    }

    public interface IResult<out T> : IResult
    {
        T Data { get; }
    }
}