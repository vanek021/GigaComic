using System.Text.Json.Serialization;
using GigaComic.Models.Enums;

namespace GigaComic.Shared.Responses.Comic
{
    public class ComicResponse : IResponse
    {
        public long Id { get; set; }
        public ComicStage Stage { get; set; }
        public string Theme { get; set; }

        public List<ComicRawImageResponse> RawImages { get; set; } = new();

        public List<ComicAbstractResponse> ComicAbstracts { get; set; } = new();

        [JsonIgnore]
        public List<ComicAbstractResponse> OrderedActiveAbstracts => ComicAbstracts.Where(a => a.IsActive).OrderBy(a => a.Order).ToList();

        [JsonIgnore]
        public List<ComicAbstractResponse> NotActiveAbstracts => ComicAbstracts.Where(a => !a.IsActive).ToList();

    }
}
