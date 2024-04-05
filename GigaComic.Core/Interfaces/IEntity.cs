using IDKEY = System.Int64;

namespace GigaComic.Core.Interfaces
{
    public interface IEntity
    {
        IDKEY Id { get; set; }
    }
}
