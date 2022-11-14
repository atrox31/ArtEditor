using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    public partial class Coder : Form
    {
        public Dictionary<string, List<string>> Category;
        public string c_Content = "";
        public string[] c_Return;
        public string fuction = "";

        public Coder(ObjectManager _parrent)
        {
            InitializeComponent(); Program.ApplyTheme(this);
            Category = new Dictionary<string, List<string>>();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            foreach (var item in Category[comboBox1.SelectedItem.ToString()])
            {
                comboBox2.Items.Add(item.Split(';')[1]);
            }
        }

        void Picker(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        public void ContentRefresh()
        {
            string[] tokens = c_Content.Split('|');
            int current_pos = 0;
            int current_link = 0;
            Content.Text = "";
            Content.Links.Clear();

            foreach (string token in tokens)
            {
                if (token.Contains(':')) // picker
                {
                    string[] parser = token.Split(':');

                    string text_to_add = "";
                    if (c_Return[current_link] != null)
                    {
                        text_to_add = c_Return[current_link];
                    }
                    else
                    {
                        text_to_add = parser[1];
                    }

                    Content.Text += text_to_add;
                    Content.Links.Add(current_pos, text_to_add.Length);
                    Content.Links[current_link].Description = token;
                    current_link++;

                    current_pos += text_to_add.Length;
                }
                else
                {   // normal text
                    Content.Text += token;
                    current_pos += token.Length;
                }
            }
            /*      0               1                   2                                       3
                "i_move_point;Move to point;Move to |point:point| with |float:speed|.;2",
            */
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] temp = Category[comboBox1.SelectedItem.ToString()][comboBox2.SelectedIndex].Split(';');
            c_Content = temp[2];
            c_Return = new string[Convert.ToInt32(temp[3])];

            fuction = temp[0];
            ContentRefresh();
        }

        private void Coder_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(Category.Keys.ToArray());
        }
    }
}
