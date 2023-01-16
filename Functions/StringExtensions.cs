using System;
using System.Drawing;
using System.Linq;

namespace ArtCore_Editor.Functions
{
    public static class StringExtensions
    {
        // return if string is hex value
        public static bool IsHex(this string text)
        {
            if (!(text.Length == 7 || text.Length == 9)) return false;
            if (text[0] != '#') return false;
            return (System.Text.RegularExpressions.Regex.IsMatch(text, @"#[0-9a-fA-F]{6}") || System.Text.RegularExpressions.Regex.IsMatch(text, @"#[0-9a-fA-F]{8}"));
        }
        public static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        // fix to microsoft Path.Combine
        // combine path from target paths
        public static string Combine(string path1, string path2)
        {
            if (path1 == null) return path2;
            if (path2 == null) return path1;
            return path1.Trim().TrimEnd(System.IO.Path.DirectorySeparatorChar)
                       + System.IO.Path.DirectorySeparatorChar
                       + path2.Trim().TrimStart(System.IO.Path.DirectorySeparatorChar);
        }

        // combine path from target paths
        public static string Combine(string path1, string path2, string path3)
        {
            return Combine(Combine(path1, path2), path3);
        }
        // combine path from target paths
        public static string Combine(string path1, string path2, string path3, string path4)
        {
            return Combine(Combine(path1, path2, path3), path4);
        }

        // convert hex value to color
        public static Color HexToColor(this string hex)
        {
            return (Color)ColorTranslator.FromHtml(hex);
        }

        /// <summary>
        /// Return new string with replaced middle part with "..."
        /// </summary>
        /// <param name="length">max length of string</param>
        /// <returns></returns>
        public static string ShortString(this string text, int length)
        {
            if (text == null) return "";
            if (length == 0) return text;
            if (text.Length == 0) return text;
            if (text.Length <= length) return text;
            if (text.Length - 3 <= length) return text;
            int strLen = (length - 3) / 2;
            if (strLen <= 0) return text;
            string left = text.Substring(0, strLen);
            string right = text.Substring(text.Length - 1 - strLen, strLen);
            return left + "..." + right;
        }
    }
}
