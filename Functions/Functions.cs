using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows.Forms;
using Color = System.Drawing.Color;
using Point = System.Drawing.Point;

namespace ArtCore_Editor.Functions
{
    public static class Functions
    {
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
        /// <summary>
        /// Delete file if exists
        /// </summary>
        /// <param name="pathToFile">Path to file</param>
        public static void FileDelete(string pathToFile)
        {
            if (File.Exists(pathToFile))
            {
                File.Delete(pathToFile);
            }
        }
        /// <summary>
        /// Prepare new directory, delete files if exists
        /// </summary>
        /// <param name="pathToDirectory">Path to directory</param>
        public static void CleanDirectory(string pathToDirectory)
        {
            if (Directory.Exists(pathToDirectory))
            {
                Directory.Delete(pathToDirectory, true);
            }
            Directory.CreateDirectory(pathToDirectory);
        }

        /// <summary>
        /// Convert type to another type
        /// </summary>
        /// <typeparam name="T">Target type</typeparam>
        /// <param name="obj">Sender</param>
        /// <returns>Converted type</returns>
        public static T ForceType<T>(this object obj)
        {
            T res = Activator.CreateInstance<T>();
            Type x = obj.GetType();
            Type y = res.GetType();
            foreach (PropertyInfo destinationProp in y.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance))
            {
                PropertyInfo sourceProp = x.GetProperty(destinationProp.Name);
                if (sourceProp != null && sourceProp.GetValue(obj) != null)
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
            Stack<Control> stack = new Stack<Control>();
            stack.Push(root);

            while (stack.Any())
            {
                Control next = stack.Pop();
                foreach (Control child in next.Controls)
                    stack.Push(child);

                yield return next;
            }
        }

        /// <summary>
        /// Return Hash checksum of file content
        /// </summary>
        /// <param name="filename">path to file</param>
        /// <returns></returns>
        public static string CalculateHash(string filename)
        {
            if (File.Exists(filename))
            {
                using SHA1 sha = System.Security.Cryptography.SHA1.Create();
                byte[] stream = File.ReadAllBytes(filename);
                return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-", "");
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Get linear value convert
        /// </summary>
        /// <param name="value"></param>
        /// <param name="valueMin"></param>
        /// <param name="valueMax"></param>
        /// <param name="minScale"></param>
        /// <param name="maxScale"></param>
        /// <returns></returns>
        public static int Scale(int value, int valueMin, int valueMax, int minScale, int maxScale)
        {
            float scaled = (float)minScale + (float)(value - valueMin) / (float)(valueMax - valueMin) * (float)(maxScale - minScale);
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
        /// Get distance between two points
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
            Rectangle destRect = new Rectangle(0, 0, width, height);
            Bitmap destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (Graphics graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (ImageAttributes wrapMode = new ImageAttributes())
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
            if (!condition)
                MessageBox.Show(errorMsg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return !condition;
        }

        /// <summary>
        /// Delete target directory and all files inside, if exists
        /// </summary>
        /// <param name="path">Path to directory</param>
        public static void DeleteDirectory(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
        }
    }
}
