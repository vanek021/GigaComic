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
        public long Id { get; set; }
        public ComicStage Stage { get; set; }

        public List<ComicAbstractResponse> ComicAbstracts { get; set; } = new();

        public List<ComicAbstractResponse> OrderedActiveAbstracts => ComicAbstracts.Where(a => a.IsActive).OrderBy(a => a.Order).ToList();
        public List<ComicAbstractResponse> NotActiveAbstracts => ComicAbstracts.Where(a => !a.IsActive).ToList();

    }
}
