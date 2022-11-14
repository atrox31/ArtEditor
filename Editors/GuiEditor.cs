using System;
using System.Windows.Forms;


namespace ArtCore_Editor
{
    public partial class GuiEditor : Form
    {
        //[DllImport("SDL2#.dll")]
        public GuiEditor()
        {
            InitializeComponent(); Program.ApplyTheme(this);
        }

        private void GuiEditor_Load(object sender, EventArgs e)
        {

        }
    }
}
