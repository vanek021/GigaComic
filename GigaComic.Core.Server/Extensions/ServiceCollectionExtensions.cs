using GigaComic.Core.Services.BucketStorage.FileSystem;
using GigaComic.Core.Services.BucketStorage;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Core.Server.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddFileSystemBucketStorage(this IServiceCollection serviceCollection, string ServerWebRootPath, string defaultBucketName)
        {
            var storageService = new FileSystemBucketStorageService(ServerWebRootPath);
            serviceCollection.AddSingleton<IBucketStorageService>(storageService);

            var bucket = storageService.GetBucket(defaultBucketName);
            serviceCollection.AddSingleton<IBucket>(bucket);

            return serviceCollection;
        }
    }
}
