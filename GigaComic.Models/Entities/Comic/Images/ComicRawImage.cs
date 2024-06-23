using GigaComic.Core.Entities;
using GigaComic.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Models.Entities.Comic
{
    public class ComicRawImage : AbstractComicImage
    {
        public string GeneratingRequest { get; set; }
        public string Title { get; set; }
        public bool IsCensored { get; set; }
        public RawImageState State { get; set; }

        [ForeignKey(nameof(ComicId))]
        public virtual Comic Comic { get; set; }
        public long ComicId { get; set; }
    }
}
