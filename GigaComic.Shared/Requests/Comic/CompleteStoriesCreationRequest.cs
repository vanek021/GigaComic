using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Shared.Requests.Comic
{
    public class CompleteStoriesCreationRequest : IRequest
    {
        public long ComicId { get; set; }

        public List<StoryRequest> Stories { get; set; } = [];
    }

    public class StoryRequest : IRequest
    {
        public long Id { get; set; }
        //public int Order { get; set; }
        //public string Name { get; set; }
        public string Content { get; set; }
        //public bool IsActive { get; set; }
    }
}
