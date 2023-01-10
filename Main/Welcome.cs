using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ArtCore_Editor.Functions;

namespace ArtCore_Editor.etc
{
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent(); Program.ApplyTheme(this);
        }

        private bool _haveLast = false;
        public string OpenProject = null;
        private readonly List<string> LastProjects = new List<string>();

        private void Welcome_Load(object sender, EventArgs e)
        {
            if (!File.Exists(Program.ProgramDirectory + "\\" + Program.LastFilename))
            {
                listBox1.Items.Add("<no last projects>");
                return;
            }
            List<string> projects = new List<string>();
            List<string> list = File.ReadAllLines(Program.ProgramDirectory + "\\" + Program.LastFilename).ToList();
            if (list.Count > 0)
            {
                _haveLast = true;
                foreach (string line in list)
                {
                    if (!File.Exists(line + "\\" + Program.ProjectFilename)) continue;
                    LastProjects.Add(line);
                    listBox1.Items.Add(line.Split('\\').Last() + " (" + line.ShortString(30) + ")");
                    projects.Add(line);
                }
                File.Delete(Program.ProgramDirectory + "\\" + Program.LastFilename);
                File.WriteAllLines(Program.ProgramDirectory + "\\" + Program.LastFilename, projects);
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
