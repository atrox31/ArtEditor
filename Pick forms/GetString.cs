using System;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    public partial class GetString : Form
    {
        public string value;
        public static string Get(string name, string default_value = null)
        {
            GetString gs = new GetString(name, default_value);
            if (gs.ShowDialog() == DialogResult.OK)
            {
                return gs.value;
            }
            return default_value;
        }
        public GetString(string name, string default_value = null)
        {
            InitializeComponent();Program.ApplyTheme(this);
            groupBox1.Text = name;
            textBox1.Focus();
            if (default_value != null)
            {
                textBox1.Text = default_value;
                textBox1.SelectAll();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            value = textBox1.Text;
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void GetString_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                value = textBox1.Text;
                Close();
            }
        }
    }
}
