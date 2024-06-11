using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Shared.Requests.Comic
{
    public class CompleteAbstractCreationRequest : IRequest
    {
        public long ComicId { get; set; }

        public List<AbstractRequest> Abstracts { get; set; } = [];

    }

    public class AbstractRequest : IRequest
    {
        public long Id { get; set; }
        public int Order { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
    }
}
