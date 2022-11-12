using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace ArtCore_Editor
{
    public partial class GridControl : Form
    {
        public int GWidth;
        public int GHeight;
        public GridControl(int width, int height)
        {
            InitializeComponent();
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

        public static void GetGridDimensions(ref Point Dimensions)
        {
            GridControl gridControl = new GridControl(Dimensions.X, Dimensions.Y);
            if(gridControl.ShowDialog() == DialogResult.OK)
            {
                Dimensions.X = gridControl.GWidth;
                Dimensions.Y = gridControl.GHeight;
            }
        }
    }
}
