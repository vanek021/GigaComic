using System.Drawing;

namespace GigaComic.Modules.ComicRenderer
{
    public class PageLayout
    {
        public int ImagesCount { get; set; }
        public int TextsCount { get; set; }
        public Bitmap Template { get; set; }
        public Point[] ImagesPlacement { get; set; }
        public Rectangle[] TextsPlacement { get; set; }
    }
}
