using ArtCore_Editor.etc;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ArtCore_Editor.Main
{
    public partial class NewProjectWindow : Form
    {
        public string NewProjectFile;
        public NewProjectWindow()
        {
            InitializeComponent();Program.ApplyTheme(this);
        }

        private void NewProjectWindow_Load(object sender, System.EventArgs e)
        {
            foreach (string enumerateFile in Directory.EnumerateFiles(Directory.GetCurrentDirectory() + "\\StandardBehaviour"))
            {
                cbl_standard_behaviour.Items.Add(Path.GetFileNameWithoutExtension(enumerateFile));
            }

            foreach (CheckBox item in chl_project_target_platform.Items)
            {
                if (!item.Text.Contains("Windows"))
                {
                    item.Enabled = false;
                    item.Text += " --IN VERSION 1.0";
                }
            }
        }

        private void chb_use_default_font_CheckedChanged(object sender, System.EventArgs e)
        {
            txb_default_font_path.Enabled = !chb_use_default_font.Checked;
            btn_default_font.Enabled = !chb_use_default_font.Checked;
            nud_default_font_size.Enabled = !chb_use_default_font.Checked;
        }

        private void CreateNewProject()
        {

            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                if (Directory.GetFiles(folderBrowserDialog1.SelectedPath).Length > 0)
                {
                    MessageBox.Show("Directory must be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    LoadScreen loadScreen = new LoadScreen(false);
                    loadScreen.Show();

                    string path = folderBrowserDialog1.SelectedPath;
                    GlobalProject = new GameProject();
                    GlobalProject.Prepare_new();
                    GlobalProject.ProjectName = path.Split('\\').Last();
                    GameProject.ProjectPath = path;

                    loadScreen.SetProgress(20);

                    Directory.CreateDirectory(path + "\\assets");
                    Directory.CreateDirectory(path + "\\assets\\texture");
                    Directory.CreateDirectory(path + "\\assets\\sprite");
                    Directory.CreateDirectory(path + "\\assets\\music");
                    Directory.CreateDirectory(path + "\\assets\\sound");
                    Directory.CreateDirectory(path + "\\assets\\font");
                    Directory.CreateDirectory(path + "\\database");
                    Directory.CreateDirectory(path + "\\object");
                    Directory.CreateDirectory(path + "\\scene");
                    Directory.CreateDirectory(path + "\\levels");
                    Directory.CreateDirectory(path + "\\output");

                    GlobalProject.SaveToFile();

                    // update last.txt
                    loadScreen.SetProgress(80);
                    List<string> lines = new List<string>();
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + Program.LastFilename))
                    {
                        lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\" + Program.LastFilename).ToList();
                    }
                    lines.Insert(0, path);
                    File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\" + Program.LastFilename, lines);


                    loadScreen.SetProgress(100);
                    loadScreen.Close();
                    return;
                }
            }
            else
            {
                return;
            }
        }

        private void btn_accept_Click(object sender, EventArgs e)
        {
            if (true)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {


            }
        }

        private void brn_project_path_Click(object sender, EventArgs e)
        {

        }
    }
}
