using GigaComic.Core.Services.BucketStorage;
using GigaComic.Data;
using GigaComic.Models.Entities.Comic;
using GigaComic.Models.Enums;
using GigaComic.Modules.GigaChat;
using GigaComic.Modules.Kandinsky;
using GigaComic.Services.ImageGeneration.Strategies;
using GigaComic.Shared.Requests.Comic;
using Microsoft.EntityFrameworkCore;

namespace GigaComic.Services.Generation
{
    public class ComicImageGenerationService : BaseService<ComicRawImage>
    {
        private readonly AppDbContext _appDbCtx;
        private readonly KandinskyApi _kandinskyApi;
        private readonly GigaChatClient _chatClient;
        private readonly IBucket _bucket;

        public ComicImageGenerationService(AppDbContext appDbCtx, KandinskyApi kandinskyApi, IBucketStorageService bucketStorageService, GigaChatClient chatClient)
            : base(appDbCtx)
        {
            _appDbCtx = appDbCtx;
            _kandinskyApi = kandinskyApi;
            _bucket = bucketStorageService.GetBucket("comic");
            _chatClient = chatClient;
        }

        public async Task PrepareRawImages(Comic comic)
        {
            var strategy = new GigachatOneRequestGenerationStrategy(_chatClient, _kandinskyApi, _bucket, _appDbCtx);
            await strategy.GenerateFor(comic);
        }

        public async Task GenerateRawImages()
        {
            var pendingRawIamges = _appDbCtx.ComicRawImages
                .Where(c => c.State == RawImageState.Created)
                .Include(c => c.Comic)
                .ToList();

            foreach (var pendingImage in pendingRawIamges)
            {
                await GenerateRawImage(pendingImage);
            }
        }

        public string GetPathForRawImage(Comic comic, ComicRawImage rawImage)
        {
            return Path.Combine($"comic{comic.Id}", $"rawImages", $"rawImage{rawImage.Id}.png");
        }

        public async Task<ComicRawImage> RegenerateRawImage(RegenerateRawImageRequest model, long userId)
        {
            var rawImage = DbContext.ComicRawImages.SingleOrDefault(c => c.Id == model.Id && c.Comic.UserId == userId);

            if (rawImage == null)
                throw new ArgumentNullException("Не удалось обновить указанную картинку");

            rawImage.GeneratingRequest = model.GeneratingRequest;

            rawImage = await GenerateRawImage(rawImage);
            return rawImage;
        }

        private async Task<ComicRawImage> GenerateRawImage(ComicRawImage pendingImage)
        {
            try
            {
                var result = await _kandinskyApi.GenerateAndWait(pendingImage.GeneratingRequest, style: pendingImage.Comic.Style);
                if (result.Censored)
                {
                    pendingImage.IsCensored = true;
                }
                else
                {
                    var image = result.Images.Single();
                    var path = GetPathForRawImage(pendingImage.Comic, pendingImage);
                    var bytes = Convert.FromBase64String(image);
                    using var contentStream = new MemoryStream(bytes);

                    if (_bucket.ContainsObject(path))
                        _bucket.DeleteObject(path);

                    _bucket.WriteObject(path, contentStream);

                    pendingImage.PublicUrl = _bucket.GetPublicURL(path);
                }

                pendingImage.State = RawImageState.Processed;
            }
            catch (Exception ex)
            {
                pendingImage.State = RawImageState.Fail;
                pendingImage.PublicUrl = null;
            }

            pendingImage.UpdatedAt = DateTime.UtcNow;

            _appDbCtx.Update(pendingImage);
            await _appDbCtx.SaveChangesAsync();

            return pendingImage;
        }
    }
}
