
using Microsoft.Extensions.FileProviders;

namespace GigaComic.Core.Services.BucketStorage
{
    public interface IVirtualFileSystem
    {
        IEnumerable<IFileInfo> EnumerateChildObjects(string path);
        IFileInfo GetObject(string path);
    }
}
