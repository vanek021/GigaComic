using GigaComic.Core.Services.BucketStorage;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaComic.Core.Extensions
{
    public static class ConfigureExtensions
    {
        public static IApplicationBuilder UseBucketStorageFileServer(this IApplicationBuilder builder, IBucketStorageService service, bool enableDirectoryBrowsing)
        {
            var fileProvider = service as IBucketStorageFileProvider;
            if (fileProvider == null)
                throw new ArgumentException("service should implement IBucketStorageFileProvider");

            return builder.UseBucketStorageFileServer(fileProvider, enableDirectoryBrowsing);
        }

        public static IApplicationBuilder UseBucketStorageFileServer(this IApplicationBuilder builder, IBucketStorageFileProvider fileProvider, bool enableDirectoryBrowsing)
        {
            builder.UseFileServer(new FileServerOptions
            {
                EnableDirectoryBrowsing = enableDirectoryBrowsing,
                FileProvider = fileProvider.FileProvider,
                RequestPath = fileProvider.BasePath,
                EnableDefaultFiles = false
            });

            return builder;
        }
    }
}
