using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    internal static class Program
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if DEBUG
            //AllocConsole();
#endif
            Application.EnableVisualStyles();
            //Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public const string LAST_FILENAME = "last.txt";
        public const string PROJECT_FILENAME = "Project.acg";
        public const float VERSION = 0.25f;
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }

}
