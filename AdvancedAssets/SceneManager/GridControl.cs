using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArtCore_Editor.AdvancedAssets.SceneManager
{
    public partial class GridControl : Form
    {
        public int GWidth;
        public int GHeight;
        public GridControl(int width, int height)
        {
            InitializeComponent(); Program.ApplyTheme(this);
            GWidth = width;
            GHeight = height;
            numericUpDown1.Value = width;
            numericUpDown2.Value = height;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            GWidth = (int)numericUpDown1.Value;
            GHeight = (int)numericUpDown2.Value;
            DialogResult = DialogResult.OK;
            Close();
        }

        public static void GetGridDimensions(ref Point dimensions)
        {
            var gridControl = new GridControl(dimensions.X, dimensions.Y);
            if (gridControl.ShowDialog() != DialogResult.OK) return;
            dimensions.X = gridControl.GWidth;
            dimensions.Y = gridControl.GHeight;
        }
    }
}
