using GigaComic.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Shared.Requests.Comic
{
    public class ComicSetupRequest : IRequest
    {
        public ComicStyle Style { get; set; }

        public ComicGrid Grid { get; set; }

        public ComicLabel Label { get; set; }
    }
}
