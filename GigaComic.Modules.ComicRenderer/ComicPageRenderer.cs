
using System.Drawing;

namespace GigaComic.Modules.ComicRenderer
{
    public class ComicPageRenderer
    {
        public Bitmap RenderPage(Bitmap[] images, string[] texts, PageLayout layout)
        {
            if (images.Length != layout.ImagesCount)
                throw new ArgumentException("Количество переданных изображений не совпадает с лэйаутом");
            if (texts.Length != layout.TextsCount)
                throw new ArgumentException("Количество переданных надписей не совпадает с лэйаутом");

            var page = new Bitmap(layout.Template);
            using var g = Graphics.FromImage(page);

            for (int i = 0; i < images.Length; i++)
                g.DrawImage(images[i], layout.ImagePlacements[i]);

            var font = new Font(FontFamily.GenericSerif, 32);
            var format = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            for (int i = 0; i < texts.Length; i++)
            {
                var adjFont = GetAdjustedFont(g, texts[i], font, layout.TextPlacements[i].Size, 54, 12, true, format);
                g.DrawString(texts[i],
                    adjFont,
                    Brushes.Black, layout.TextPlacements[i], format);
            }

            return page;
        }

        private Font GetAdjustedFont(
            Graphics g,
            string graphicString,
            Font originalFont,
            Size container,
            int maxFontSize,
            int minFontSize,
            bool smallestOnFail,
            StringFormat format)
        {
            Font testFont = null;
            // We utilize MeasureString which we get via a control instance           
            for (int adjustedSize = maxFontSize; adjustedSize >= minFontSize; adjustedSize--)
            {
                testFont = new Font(originalFont.Name, adjustedSize, originalFont.Style);

                // Test the string with the new size
                SizeF adjustedSizeNew = g.MeasureString(graphicString, testFont, container.Width, format);

                if (container.Height > adjustedSizeNew.Height)
                {
                    // Good font, return it
                    return testFont;
                }
            }

            // If you get here there was no fontsize that worked
            // return minimumSize or original?
            if (smallestOnFail)
            {
                return testFont;
            }
            else
            {
                return originalFont;
            }
        }
    }
}
