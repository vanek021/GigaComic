using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Shared.Requests.Comic
{
    public class RegenerateRawImageRequest : IRequest
    {
        public long Id { get; set; }
        public string GeneratingRequest { get; set; }
    }
}
