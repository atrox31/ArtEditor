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
            InitializeComponent(); Program.ApplyTheme(this);
        }
        bool _haveLast = false;
        public string OpenProject = null;
        public List<string> LastProjects = new List<string>();

        private void Welcome_Load(object sender, EventArgs e)
        {
            if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + Program.LastFilename))
            {
                listBox1.Items.Add("<no last projects>");
                return;
            }
            List<string> projects = new List<string>();
            List<string> list = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\" + Program.LastFilename).ToList();
            if (list.Count > 0)
            {
                _haveLast = true;
                foreach (var line in list)
                {
                    if (!File.Exists(line + "\\" + Program.ProjectFilename)) continue;
                    LastProjects.Add(line);
                    listBox1.Items.Add(line.Split('\\').Last() + " (" + Functions.ShortString(line, 30) + ")");
                    projects.Add(line);
                }
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\" + Program.LastFilename);
                File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\" + Program.LastFilename, projects);
            }   
            else
            {
                listBox1.Items.Add("<no last projects>");
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

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex <= -1) return;
            // open project
            if (!_haveLast) return;
            OpenProject = LastProjects[listBox1.SelectedIndex];
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
            Close();
        }
    }
}
