﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

namespace ArtCore_Editor
{
    public static class Functions
    {
        // return if string is hex value
        public static bool IsHex(this string text)
        {
            if ( !(text.Length == 7 || text.Length == 9)) return false;
            if (text[0] != '#') return false;
            return (System.Text.RegularExpressions.Regex.IsMatch(text, @"#[0-9a-fA-F]{6}") || System.Text.RegularExpressions.Regex.IsMatch(text, @"#[0-9a-fA-F]{8}"));
        }

        // convert color value to hex code #RRGGBB
        public static string ColorToHex(Color color)
        {
            return "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
        }

        // convert hex value to color
        public static Color HexToColor(string hex)
        {
            return (Color)ColorTranslator.FromHtml(hex);
        }

        public static void ButtonAlter_Paint(object sender, PaintEventArgs e)
        {
            dynamic btn = (Button)sender;
            dynamic drawBrush = new SolidBrush(btn.Enabled ? btn.ForeColor : Color.FromArgb(57, 91, 100));

            dynamic sf = new StringFormat
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };
            e.Graphics.DrawString(btn.Text, btn.Font, drawBrush, e.ClipRectangle, sf);
            //btn.Text = string.Empty;
            drawBrush.Dispose();
            sf.Dispose();

        }

        // Convert object to target type <T>
        public static T ForceType<T>(this object obj)
        {
            T res = Activator.CreateInstance<T>();
            Type x = obj.GetType();
            Type y = res.GetType();
            foreach (var destinationProp in y.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                var sourceProp = x.GetProperty(destinationProp.Name);
                if (sourceProp != null)
                {
                    destinationProp.SetValue(res, sourceProp.GetValue(obj));
                }
            }

            return res;
        }

        /// <summary>
        /// traverse a tree and get all of the child controls (at all depths) below any control.
        /// ex. var textboxes = GetAllControls(someForm).OfTYpe<Textbox>();
        /// </summary>
        /// <param name="root">Control to get childs - include root</param>
        /// <returns></returns>
        public static IEnumerable<Control> GetAllControls(Control root)
        {
            var stack = new Stack<Control>();
            stack.Push(root);

            while (stack.Any())
            {
                var next = stack.Pop();
                foreach (Control child in next.Controls)
                    stack.Push(child);

                yield return next;
            }
        }
        /// <summary>
        /// Return MD5 checksum
        /// </summary>
        /// <param name="filename">Filename</param>
        /// <returns></returns>
        public static string CalculateMd5(string filename)
        {
            using var md5 = System.Security.Cryptography.MD5.Create();
            var stream = File.ReadAllBytes(filename);
            //var hash = md5.ComputeHash(stream);
            return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "");
        }
        /// <summary>
        /// Get linear value convert
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <param name="minScale"></param>
        /// <param name="maxScale"></param>
        /// <returns></returns>
        public static int Scale(int value, int valueMin, int ValueMax, int minScale, int maxScale)
        {
            var scaled = (float)minScale + (float)(value - valueMin) / (float)(ValueMax - valueMin) * (float)(maxScale - minScale);
            return (int)Math.Round(scaled);
        }
        /// <summary>
        /// Rename Key in dictionary
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dic">Dictionary object</param>
        /// <param name="fromKey">Replace this</param>
        /// <param name="toKey">With that</param>
        public static void RenameKey<TKey, TValue>(this IDictionary<TKey, TValue> dic,
                                      TKey fromKey, TKey toKey)
        {
            TValue value = dic[fromKey];
            dic.Remove(fromKey);
            dic[toKey] = value;
        }
        /// <summary>
        /// Get max lenght of string and replace in middle with "..." defined by parametr
        /// </summary>
        /// <param name="text">string to short</param>
        /// <param name="length">max lenght of string</param>
        /// <returns></returns>
        public static string ShortString(string text, int length)
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
        /// <summary>
        /// Get distance beetwen two points
        /// </summary>
        /// <param name="p1">Point A</param>
        /// <param name="p2">Point B</param>
        /// <returns></returns>
        public static float GetDistance(Point p1, Point p2)
        {
            return (float)Math.Sqrt(Math.Pow((p2.X - p1.X), 2) + Math.Pow((p2.Y - p1.Y), 2));
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Image ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return Image.FromHbitmap(destImage.GetHbitmap());
        }
        /// <summary>
        /// if(ErrorCheck( condition, message )) return;
        /// If condition is false, shows error message and return true
        /// </summary>
        /// <param name="condition">If this is true nothing happen, if else show error-box</param>
        /// <param name="errorMsg">Message with error</param>
        /// <returns></returns>
        public static bool ErrorCheck(bool condition, string errorMsg)
        {
            if (condition == false)
                MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return !condition;
        }
    }
}
