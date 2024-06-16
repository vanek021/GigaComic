using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Shared.Responses.Comic
{
    /// <summary>
    /// Расширение базовой модели, здесь будет финальный результат комикста с картинками
    /// </summary>
    public class ComicResultResponse : ComicResponse
    {
        public List<ComicImageResponse> Images { get; set; } = [];
    }

    public class ComicImageResponse : IResponse 
    {
        public string ImageUrl { get; set; }
    }
}
