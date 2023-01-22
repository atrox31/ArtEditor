using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing;

namespace ArtCore_Editor.Functions
{
    public static class BitmapExtensions
    {
        public static Image SetOpacity(this Image image, float opacity)
        {
            ColorMatrix colorMatrix = new ColorMatrix
            {
                Matrix33 = opacity
            };
            ImageAttributes imageAttributes = new ImageAttributes();
            imageAttributes.SetColorMatrix(
                colorMatrix,
                ColorMatrixFlag.Default,
                ColorAdjustType.Bitmap);
            Bitmap output = new Bitmap(image.Width, image.Height);
            using Graphics gfx = Graphics.FromImage(output);
            gfx.SmoothingMode = SmoothingMode.HighSpeed;
            gfx.DrawImage(
                image,
                new Rectangle(0, 0, image.Width, image.Height),
                0,
                0,
                image.Width,
                image.Height,
                GraphicsUnit.Pixel,
                imageAttributes);
            return output;
        }
    }
}
