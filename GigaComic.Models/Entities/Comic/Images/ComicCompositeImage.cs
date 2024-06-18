using GigaComic.Core.Entities;
using GigaComic.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Models.Entities.Comic
{
    public class ComicCompositeImage : AbstractComicImage
    {
        public CompositeImageGrid ImageGrid { get;set; }
        public virtual List<ComicRawImage> RawImages { get; set; }
    }
}
