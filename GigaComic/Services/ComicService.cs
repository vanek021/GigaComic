using System.Drawing;
using AutoMapper;
using GigaComic.Core.Services.BucketStorage;
using GigaComic.Data;
using GigaComic.Models.Entities.Comic;
using GigaComic.Modules.GigaChat;
using GigaComic.Modules.Kandinsky;
using GigaComic.Shared.Responses;
using GigaComic.Shared.Responses.Comic;
using Microsoft.EntityFrameworkCore;
using Sakura.AspNetCore;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Image = System.Drawing.Image;

namespace GigaComic.Services
{
    public class ComicService : BaseService<Comic>
    {
        private readonly KandinskyApi _kandinskyApi;
        private readonly GigaChatClient _gigaChatClient;
        private readonly IMapper _mapper;
        private readonly IBucket _bucket;

        public ComicService(AppDbContext dbContext, IMapper mapper, IBucketStorageService bucketStorageService/*, KandinskyApi kandinskyApi*/, GigaChatClient gigaChatClient) : base(dbContext)
        {
            //_kandinskyApi = kandinskyApi;
            _gigaChatClient = gigaChatClient;
            _mapper = mapper;
            _bucket = bucketStorageService.GetBucket("comic");
        }

        public async Task CompleteSetup()
        {
            var kdPrompts = "";
        }

        public async Task<PaginatedResult<ComicResponse>> GetPagedComics(long userId, int page, int pageSize)
        {
            var comics = await Get()
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .ToPagedListAsync(pageSize, page);

            var mappedItems = _mapper.Map<List<ComicResponse>>(comics);

            return PaginatedResult<ComicResponse>.Success(mappedItems, comics.Count, comics.PageIndex, comics.PageIndex);
        }

        public async Task<List<string>> GetLastComicsThemes(long userId)
        {
            var comicThemes = await Get()
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.CreatedAt)
                .Select(c => c.Theme)
                .Distinct()
                .Take(20)
                .ToListAsync();

            return comicThemes;
        }

        public async Task<Comic> GetComicWithIncludes(long comicId)
        {
            var comic = await Get()
                .Where(c => c.Id == comicId)
                .Include(c => c.ComicRawImages)
                .Include(c => c.ComicCompositeImages)
                .SingleAsync();

            return comic;
        }

        public async Task SavePdf(Comic comic)
        {
            var imagesPath = @$"{Directory.GetCurrentDirectory()}\wwwroot\uploads\comic\comic{comic.Id}\pages";
            var pdfPath = @$"{Directory.GetCurrentDirectory()}\wwwroot\uploads\comic\comic{comic.Id}\comic.pdf";
            Document document = new Document(PageSize.A4, 25, 25, 25, 25);

            using (FileStream stream = new FileStream(pdfPath, FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(document, stream);
                document.Open();

                // Получение всех файлов изображений из папки
                string[] imageFiles = Directory.GetFiles(imagesPath, "*.*", SearchOption.TopDirectoryOnly);
                foreach (string imageFile in imageFiles)
                {
                    // Проверка, является ли файл изображением
                    if (IsImageFile(imageFile))
                    {
                        // Конвертация изображения в iTextSharp.image
                        using (FileStream imageStream = new FileStream(imageFile, FileMode.Open, FileAccess.Read))
                        {
                            iTextSharp.text.Image pdfImage = iTextSharp.text.Image.GetInstance(imageStream);

                            // Изменение размера изображения для размещения на странице
                            pdfImage.ScaleToFit(document.PageSize.Width - document.LeftMargin - document.RightMargin,
                                document.PageSize.Height - document.TopMargin - document.BottomMargin);
                            pdfImage.Alignment = Element.ALIGN_CENTER;
                            document.Add(pdfImage);
                            document.NewPage();
                        }
                    }
                }

                document.Close();
                writer.Close();
            }
        }
        
        static bool IsImageFile(string filePath)
        {
            try
            {
                using (Image img = Image.FromFile(filePath))
                {
                }
                return true;
            }
            catch (OutOfMemoryException)
            {
                // Файл не является изображением
                return false;
            }
        }
    }
}
