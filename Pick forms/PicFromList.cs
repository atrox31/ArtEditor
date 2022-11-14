using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    public partial class PicFromList : Form
    {
        public string Selected;
        public int SelectedIndex;
        public PicFromList(List<string> vars)
        {
            InitializeComponent(); Program.ApplyTheme(this);
            Selected = null;
            SelectedIndex = -1;
            listBox1.Items.AddRange(vars.ToArray());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                Selected = listBox1.SelectedItem.ToString();
                SelectedIndex = listBox1.SelectedIndex;
            }
            else
            {
                Selected = null;
                SelectedIndex = -1;
            }
            DialogResult = DialogResult.OK;
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
        public static int GetIndex(List<string> vars)
        {
            PicFromList picFromList = new PicFromList(vars);
            if (picFromList.ShowDialog() == DialogResult.OK)
            {
                return picFromList.SelectedIndex;
            }
            return -1;
        }
    }
}
