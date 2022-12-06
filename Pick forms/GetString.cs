using System;
using System.Windows.Forms;

namespace ArtCore_Editor.Pick_forms
{
    public partial class GetString : Form
    {
        public string Value;
        public static string Get(string name, string defaultValue = null)
        {
            var gs = new GetString(name, defaultValue);
            return gs.ShowDialog() == DialogResult.OK ? gs.Value : defaultValue;
        }
        public GetString(string name, string defaultValue = null)
        {
            InitializeComponent(); Program.ApplyTheme(this);
            groupBox1.Text = name;
            textBox1.Focus();
            if (defaultValue == null) return;
            textBox1.Text = defaultValue;
            textBox1.SelectAll();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Value = textBox1.Text;
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        
        private void GetString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            Value = textBox1.Text;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
