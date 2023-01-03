using System.Drawing;

namespace ArtCore_Editor.Functions
{
    public static class ColorExtensions
    {
        // convert color value to hex code #RRGGBB
        public static string ColorToHex(this Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }
    }
}
