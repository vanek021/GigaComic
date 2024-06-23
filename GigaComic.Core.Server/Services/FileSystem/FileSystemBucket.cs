namespace GigaComic.Core.Services.BucketStorage.FileSystem
{
    public class FileSystemBucket : IBucket
    {
        public string RootPath { get; }
        public string MainDirectory { get; }
        public string BucketName { get; }

        public FileSystemBucket(FileSystemBucketStorageService fileSystem, string bucketName)
        {
            RootPath = fileSystem.RootPath;
            MainDirectory = fileSystem.MainDirectory;
            BucketName = bucketName;
        }

        private string GetFullFilePath(string fileName)
        {
            fileName = fileName.Replace('\\', Path.DirectorySeparatorChar);
            fileName = fileName.Replace('/', Path.DirectorySeparatorChar);
            return Path.Combine(RootPath, MainDirectory, BucketName, fileName);
        }

        public void ReadObject(string fileName, Stream readStream)
        {
            string path = GetFullFilePath(fileName);
            using var file = File.OpenRead(path);
            file.CopyTo(readStream);
        }

        public void WriteObject(string fileName, Stream writeStream)
        {
            string path = GetFullFilePath(fileName);

            string dir = Path.GetDirectoryName(path)!;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using var file = File.OpenWrite(path);
            writeStream.CopyTo(file);
        }

        public void DeleteObject(string fileName)
        {
            if (!ContainsObject(fileName))
                throw new KeyNotFoundException($"Can't find {fileName} at {BucketName}");

            string path = GetFullFilePath(fileName);
            File.Delete(path);
        }

        public bool TryDeleteObject(string fileName)
        {
            try
            {
                DeleteObject(fileName);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string GetPublicURL(string fileName)
        {
            fileName = fileName.Replace('\\', '/');
            return $"/{MainDirectory}/{BucketName}/{fileName}";
        }

        public bool ContainsObject(string fileName)
        {
            if (string.IsNullOrEmpty(BucketName) || string.IsNullOrEmpty(fileName))
                return false;

            string path = GetFullFilePath(fileName);
            return File.Exists(path);
        }
    }
}
