using System;
using System.Windows.Forms;

namespace ArtCore_Editor.AdvancedAssets.SpriteManager
{
    public partial class AnimationSequencerForm : Form
    {
        public string FFullName;
        public string FIndexName;
        public int FFrameFrom;
        public int FFrameTo;
        public int FFrameFromMin;
        public int FFrameToMin;
        public int FFrameFromMax;
        public int FFrameToMax;

        public AnimationSequencerForm()
        {
            InitializeComponent(); Program.ApplyTheme(this);
        }

        private void animationSequencerForm_Load(object sender, EventArgs e)
        {
            textBox1.Text = FFullName;
            textBox2.Text = FIndexName;
            numericUpDown1.Minimum = FFrameFromMin;
            numericUpDown2.Minimum = FFrameToMin;
            numericUpDown1.Maximum = FFrameFromMax;
            numericUpDown2.Maximum = FFrameToMax;
            numericUpDown1.Value = FFrameFrom;
            numericUpDown2.Value = (FFrameTo == 0 ? FFrameToMax : FFrameTo);
            if (textBox2.Text.Length > 0) textBox2.ReadOnly = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.OK;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            FFullName = textBox1.Text;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            FIndexName = textBox2.Text;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            FFrameFrom = Convert.ToInt32(numericUpDown1.Value);
            numericUpDown2.Minimum = numericUpDown1.Value;
            FFrameTo = Convert.ToInt32(numericUpDown2.Value);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            FFrameTo = Convert.ToInt32(numericUpDown2.Value);
            numericUpDown1.Maximum = numericUpDown2.Value;
            FFrameFrom = Convert.ToInt32(numericUpDown1.Value);
        }

        private void animationSequencerForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}