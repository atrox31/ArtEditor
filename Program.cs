using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace ArtCore_Editor;

internal static class Program
{
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    /////   Program const variables, used to have standard names across editor   ////////////////////////////////////////////////////////////////////
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
    // project folder structure
    public static readonly List<string> ProjectDirectoryStructure = new List<string>()
    {
        "\\assets",
        "\\assets\\texture",
        "\\assets\\sprite",
        "\\assets\\music",
        "\\assets\\sound",
        "\\assets\\font",
            
        "\\object",
        "\\object\\StandardBehaviour",
        "\\scene",

        "\\output",
    };

    // ReSharper disable InconsistentNaming CommentTypo
    // Art   [S][p][r]ite  Definition - conflict with scene so ase, not asd 
    public const string FileExtensions_Sprite = ".spr";             
    // Art   [O][b][j]ect  [Definition 
    public const string FileExtensions_InstanceObject = ".obj";     
    // [A]rt [S]cene       [D]efinition
    public const string FileExtensions_SceneObject = ".asd";        
    // [A]rt [S]cript      [C]ode
    public const string FileExtensions_ArtCode = ".asc";            
    // [A]rt [C]om[p]iled  Code
    public const string FileExtensions_CompiledArtCode = ".acp";    
    // Art   [P][a]c[k]ed  Data 
    public const string FileExtensions_AssetPack = ".pak";          
    // Art   [D][a][t]a    File
    public const string FileExtensions_GameDataPack = ".dat";            
    // [A]rt   [L]evel    [F]ile
    public const string FileExtensions_SceneLevel = ".alf";       
    // ReSharper restore InconsistentNaming CommentTypo
    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    // Program exe directory
    public static string ProgramDirectory;
    [STAThread]
    private static void Main()
    {
        // change number separator to .
        CultureInfo customCulture =
            (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

        ProgramDirectory = AppDomain.CurrentDomain.BaseDirectory;

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