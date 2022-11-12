using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    public partial class PicFromList : Form
    {
        public string Selected;
        public PicFromList(List<string> vars)
        {
            InitializeComponent();Program.ApplyTheme(this);
            Selected = "";
            listBox1.Items.AddRange(vars.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Selected = listBox1.SelectedItem.ToString();
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public static string Get(List<string> vars)
        {
            PicFromList picFromList = new PicFromList(vars);
            if (picFromList.ShowDialog() == DialogResult.OK)
            {
                return picFromList.Selected;
            }
            return null;
        }
    }
}
