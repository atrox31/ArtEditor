using System;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    public partial class animationSequencerForm : Form
    {
        public string f_fullName;
        public string f_indexName;
        public int f_frameFrom;
        public int f_frameTo;
        public int f_frameFromMin;
        public int f_frameToMin;
        public int f_frameFromMax;
        public int f_frameToMax;

        public animationSequencerForm()
        {
            InitializeComponent(); Program.ApplyTheme(this);
        }

        private void animationSequencerForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = f_fullName;
            textBox2.Text = f_indexName;
            numericUpDown1.Minimum = f_frameFromMin;
            numericUpDown2.Minimum = f_frameToMin;
            numericUpDown1.Maximum = f_frameFromMax;
            numericUpDown2.Maximum = f_frameToMax;
            numericUpDown1.Value = f_frameFrom;
            numericUpDown2.Value = (f_frameTo == 0 ? f_frameToMax : f_frameTo);
            if (textBox2.Text.Length > 0) textBox2.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.OK;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            f_fullName = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            f_indexName = textBox2.Text;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            f_frameFrom = Convert.ToInt32(numericUpDown1.Value);
            numericUpDown2.Minimum = numericUpDown1.Value;
            f_frameTo = Convert.ToInt32(numericUpDown2.Value);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            f_frameTo = Convert.ToInt32(numericUpDown2.Value);
            numericUpDown1.Maximum = numericUpDown2.Value;
            f_frameFrom = Convert.ToInt32(numericUpDown1.Value);
        }

        private void animationSequencerForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}