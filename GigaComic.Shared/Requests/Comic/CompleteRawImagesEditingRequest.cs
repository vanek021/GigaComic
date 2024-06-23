using GigaComic.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Shared.Requests.Comic
{
    public class CompleteRawImagesEditingRequest : IRequest
    {
        public long ComicId { get; set; }

        public List<RawImageRequest> RawImages { get; set; } = [];

    }

    public class RawImageRequest
    {
        public long Id { get; set; }
        public string GeneratingRequest { get; set; }
        public string Title { get; set; }
        public bool IsCensored { get; set; }
        public RawImageState State { get; set; }
        public string? PublicUrl { get; set; }
        public int Order { get; set; }
    }
}
