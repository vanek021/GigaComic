
using System.Drawing;

namespace GigaComic.Modules.ComicRenderer
{
    public static class PageLayouts
    {
        public static PageLayout Layout4 { get; private set; }
        public static PageLayout Layout3 { get; private set; }
        public static PageLayout Layout1 { get; private set; }
        static PageLayouts()
        {
            var projectPath = Directory.GetParent(Environment.CurrentDirectory).FullName;
            var folderPath = Directory.GetDirectories(
                projectPath,
                "templates", SearchOption.AllDirectories)[0];

            var template = new Bitmap(Path.Combine(folderPath, "template4.jpg"));
            Layout4 = new PageLayout() {
                Template = template,
                ImagesCount = 4,
                TextsCount = 4,
                ImagePlacements = [new(29, 43), new(479, 43), new(29, 648), new(478, 648)],
                TextPlacements = [new(29, 513, 429, 114), new(479, 513, 429, 114),
                new(29, 1123, 429, 114), new(479, 1123, 429, 114)],
                ImageSizes = [new(430, 465), new(430, 465), new(430, 465), new(430, 465)]
            };
            template = new Bitmap(Path.Combine(folderPath, "template3.jpg"));
            Layout3 = new PageLayout()
            {
                Template = template,
                ImagesCount = 3,
                TextsCount = 3,
                ImagePlacements = [new(28, 43), new(28, 647), new(478, 647)],
                TextPlacements = [new (28, 519, 881, 107), new (28, 1123, 428, 117),
                    new (479, 1123, 428, 117)],
                ImageSizes = [new(881, 473), new(430, 470), new(430, 470)]
            };
            template = new Bitmap(Path.Combine(folderPath, "template1.jpg"));
            Layout1 = new PageLayout()
            {
                Template = template,
                ImagesCount = 1,
                TextsCount = 1,
                ImagePlacements = [new(31, 44)],
                TextPlacements = [new(31, 876, 963, 130)],
                ImageSizes = [new(961, 827)]
                
            };
        }
    }
}
