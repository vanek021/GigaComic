using GigaComic.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Shared.Responses.Comic
{
    public class ComicRawImageResponse
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
