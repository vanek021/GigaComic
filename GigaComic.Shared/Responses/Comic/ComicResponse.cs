using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GigaComic.Models.Enums;

namespace GigaComic.Shared.Responses.Comic
{
    public class ComicResponse : IResponse
    {
        public ComicStage Stage { get; set; }
    }
}
