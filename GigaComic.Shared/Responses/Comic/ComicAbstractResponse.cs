using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Shared.Responses.Comic
{
    public class ComicAbstractResponse : IResponse
    {
        public long Id { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string? Content { get; set; }
    }
}
