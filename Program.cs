using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace ArtCore_Editor;

internal static class Program
{
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // name for last open projects storage
    public const string LastFilename = "last.txt";
    // name for new created project files
    public const string ProjectFilename = "Project.acg";
    // global program version
    public const float Version = 0.60f;
    // list of filet that is packed to game release archive
    public static readonly List<string[]> coreFiles = new List<string[]>()
    {
        new string[]{ "pack", "gamecontrollerdb.txt" },
        new string[]{ "pack", "TitilliumWeb-Light.ttf" },
        new string[]{ "shaders", "bloom.frag" },
        new string[]{ "shaders", "common.vert" },
        new string[]{ "", "AScript.lib" },
    };

    // used project directory
    public static string ProjectPath;
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    [STAThread]
    private static void Main()
    {
        // change number separator to .
        CultureInfo customCulture =
            (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

        Application.EnableVisualStyles();
        //Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainWindow());
    }

    // change colors of all elements 
    public static void ApplyTheme(Control type)
    {
        foreach (Control control in Functions.Functions.GetAllControls(type))
        {
            control.BackColor = Color.FromArgb(32, 44, 57);
            
            control.ForeColor = Color.FromArgb(184, 176, 141);
            if (control.GetType() == typeof(Button)) control.Paint += Functions.Functions.ButtonAlter_Paint;
            if (control.GetType() == typeof(LinkLabel))
            {
                LinkLabel linkLabel = (LinkLabel)control;
                linkLabel.VisitedLinkColor = Color.FromArgb(184, 176, 141);
                linkLabel.LinkColor = Color.FromArgb(184, 176, 141);
            }
        }
    }
}