using System;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    internal static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllocConsole();
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
        public static class HttpHelper
        {
            private static readonly HttpClient _httpClient = new HttpClient();

            public static async void DownloadFileAsync(string uri
                 , string outputPath)
            {
                Uri uriResult;

                if (!Uri.TryCreate(uri, UriKind.Absolute, out uriResult))
                    throw new InvalidOperationException("URI is invalid.");

                
                byte[] fileBytes = await _httpClient.GetByteArrayAsync(uri);
                File.WriteAllBytes(outputPath, fileBytes);
            }
        }
        public static void ApplyTheme(Control type)
        {
            foreach (var c in Functions.GetAllControls(type))
            {
                c.BackColor = Color.FromArgb(57, 91, 100);
                c.ForeColor = Color.FromArgb(165, 201, 202);
                if (c.GetType() == typeof(Button))
                {
                    c.Paint += Functions.ButtonAlter_Paint;

                }
            }

        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public const string LAST_FILENAME = "last.txt";
        public const string PROJECT_FILENAME = "Project.acg";
        public const float VERSION = 0.28f;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

}
