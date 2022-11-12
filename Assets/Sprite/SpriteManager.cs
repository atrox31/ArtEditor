using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    public partial class SpriteManager : Form
    {
        public SpriteManager()
        {
            InitializeComponent();
        }

        private void spriteRenderer_DoubleClick(object sender, EventArgs e)
        {
            colorDialog1 = new ColorDialog();
            if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                spriteRenderer.BackColor = colorDialog1.Color;
            }
        }
    }
}
