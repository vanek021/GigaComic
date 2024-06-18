using GigaComic.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Models.Entities.Comic
{
    public abstract class AbstractComicImage : BaseRecord
    {
        public string? PublicUrl { get; set; }
        public int Order { get; set; }
    }
}
