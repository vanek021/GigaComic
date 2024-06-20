
using System.Drawing;

namespace GigaComic.Modules.ComicRenderer
{
    public static class PageLayouts
    {
        public static PageLayout Layout4 { get; private set; }
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
                ImagesPlacement = [new(29, 43), new(479, 43), new(29, 648), new(478, 648)],
                TextsPlacement = [new(29, 513, 429, 114), new(479, 513, 429, 114),
                new(29, 1123, 429, 114), new(479, 1123, 429, 114)]
            };
        }
    }
}
