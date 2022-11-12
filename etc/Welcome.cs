using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent();
        }
        bool have_last = false;
        public string open_project = null;
        public List<string> last_projects = new List<string>();

        private void Welcome_Load(object sender, EventArgs e)
        {

            if (File.Exists(Program.LAST_FILENAME))
            {
                List<string> projects = new List<string>();
                List<string> list = File.ReadAllLines(Program.LAST_FILENAME).ToList();
                if (list.Count > 0)
                {
                    have_last = true;
                    foreach (string line in list)
                    {
                        if (File.Exists(line + "\\" + Program.PROJECT_FILENAME))
                        {
                            last_projects.Add(line);
                            listBox1.Items.Add(line.Split('\\').Last() + " (" + Functions.ShortString(line, 30) + ")");
                            projects.Add(line);
                        }
                    }
                    File.Delete(Program.LAST_FILENAME);
                    File.WriteAllLines(Program.LAST_FILENAME, projects);
                }
                else
                {
                    listBox1.Items.Add("<no last projects>");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // create new project
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void Welcome_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                // open project
                if (!have_last) return;
                open_project = last_projects[listBox1.SelectedIndex];
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }
    }
}
