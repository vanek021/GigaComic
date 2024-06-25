using System.Drawing;

namespace GigaComic.Modules.ComicRenderer
{
    public class PageLayout
    {
        public int ImagesCount { get; set; }
        public int TextsCount { get; set; }
        public Bitmap Template { get; set; }
        public Point[] ImagePlacements { get; set; }
        public Size[] ImageSizes { get; set; }
        public Rectangle[] TextPlacements { get; set; }
    }
}
