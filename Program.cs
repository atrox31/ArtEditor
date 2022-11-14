using System;
using System.Drawing;
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
