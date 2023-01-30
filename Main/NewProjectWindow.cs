using ArtCore_Editor.etc;
using System.Collections.Generic;
using System;
using System.ComponentModel.Design;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;
using Path = System.IO.Path;
using ArtCore_Editor.AdvancedAssets.SceneManager;
using System.Xml.Linq;
using ArtCore_Editor.Assets;
using Newtonsoft.Json;

namespace ArtCore_Editor.Main
{
    public partial class NewProjectWindow : Form
    {
        public NewProjectWindow()
        {
            InitializeComponent();Program.ApplyTheme(this);
        }

        private void NewProjectWindow_Load(object sender, System.EventArgs e)
        {
            string directory1 = Directory.GetCurrentDirectory() + "\\" + "StandardBehaviour" + "\\";
            Directory.CreateDirectory(directory1);
            foreach (string enumerateFile in Directory.EnumerateFiles(directory1))
            {
                cbl_standard_behaviour.Items.Add(Path.GetFileNameWithoutExtension(enumerateFile));
            }

            chb_target_1.Text = "Windows (x64) 7, 8, 8.1, 10, 11";
            chb_target_2.Text = "Linux (x64)    --IN VERSION 1.0"; chb_target_2.Enabled = false;
            chb_target_4.Text = "Android (x64)  --IN VERSION 1.0"; chb_target_4.Enabled = false;

            string directory2 = Program.ProgramDirectory + "\\" + "Core" + "\\" + "StandardBehaviourTemplates" + "\\";
            if (Directory.Exists(directory2))
            {
                foreach (string enumerateFile in Directory.EnumerateFiles(directory2))
                {
                    cbl_standard_behaviour.Items.Add(Path.GetFileNameWithoutExtension(enumerateFile));
                }
            }
        }
        
        private bool CreateNewProject()
        {
            // standard check
            if(Functions.Functions.ErrorCheck(tbx_project_path.Text.Length > 0, "Select project path!")) return false;
            if(Functions.Functions.ErrorCheck(tbx_project_name.Text.Length > 0, "Select project path, to get project name.")) return false;
            if(Functions.Functions.ErrorCheck(
                   chb_target_1.Checked || chb_target_2.Checked || chb_target_4.Checked 
                   , "Select at least one target platform")) return false;

            // all ok, create project
            LoadScreen loadScreen = new LoadScreen(true);
            loadScreen.Show();
            
            GameProject.ProjectPath = tbx_project_path.Text;
            Directory.CreateDirectory(GameProject.ProjectPath);

            GameProject globalProject = new GameProject();
            globalProject.Prepare_new();
            globalProject.ProjectName = tbx_project_name.Text;

            if (chb_target_1.Checked) globalProject.TargetPlatforms.Add(chb_target_1.Text.Split(' ').First());
            if (chb_target_2.Checked) globalProject.TargetPlatforms.Add(chb_target_2.Text.Split(' ').First());
            if (chb_target_4.Checked) globalProject.TargetPlatforms.Add(chb_target_4.Text.Split(' ').First());
            

            foreach (string standardBehaviour in cbl_standard_behaviour.CheckedItems)
            {
                string target = Program.ProgramDirectory + "\\" +"Core" + "\\" + "StandardBehaviourTemplates" + "\\" +
                                standardBehaviour + Program.FileExtensions_ArtCode;
                if (File.Exists(target))
                {
                    File.Copy(target,
                        GameProject.ProjectPath + "\\" + "object" + "\\" + "StandardBehaviour" + "\\" +
                        standardBehaviour + Program.FileExtensions_ArtCode
                    );
                }
            }

            // project properties checkboxes
            if (chb_import_standard_main_menu.Checked)
            {
                string target = Program.ProgramDirectory + "\\" + "Core" + "\\" + "SceneTemplates" + "\\" +
                                "scn_main_menu" + ".zip";
                if (Functions.Functions.ErrorCheck(File.Exists(target), "Can not find '" + target + "'"))
                {
                    // do nothing
                }else
                {
                    ZipFile.ExtractToDirectory(target, 
                        GameProject.ProjectPath + "\\" + "scene" + "\\");

                    using StreamReader reader = new StreamReader(
                        File.Open(
                            GameProject.ProjectPath + "\\" + "scene" + "\\" + 
                            "scn_main_menu" + "\\" + "scn_main_menu" + Program.FileExtensions_SceneObject, 
                            FileMode.Open));

                    Scene scnMainMenu = JsonConvert.DeserializeObject<Scene>(reader.ReadToEnd());
                    if (scnMainMenu != null)
                    {
                        globalProject.Scenes.Add("scn_main_menu", scnMainMenu);
                    }

                }
            }

            // adjust standard options for core, sdl and opengl
            MessageBox.Show("Almost ready, now look at ArtCore settings. You can adjust it now or just click Apply.\n" +
                            "You can edit settings in upper bar: Game->Settings");

            loadScreen.Close();

            globalProject.UpdateUserSettings();
            if (globalProject.UserProperties.Count == 0)
            {
                // error, user must update core files
                // roll back all
                Functions.Functions.CleanDirectory(GameProject.ProjectPath);
                globalProject.Dispose();
            }
            else
            {
                // all ok
                MainWindow.GetInstance().GlobalProject = globalProject;
            }

            return true;
        }

        private void btn_accept_Click(object sender, EventArgs e)
        {
            if (CreateNewProject())
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void brn_project_path_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog1 = new FolderBrowserDialog();
            if (folderBrowserDialog1.ShowDialog() != DialogResult.OK) return;

            // avoid directory not found error and create new
            // project must be in new empty directory to avoid conflicts
            if (!Directory.Exists(folderBrowserDialog1.SelectedPath))
            {
                if (MessageBox.Show("Create new directory?", "Directory not exists", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    return;
                }
                Directory.CreateDirectory(folderBrowserDialog1.SelectedPath);
                
            }

            if(Functions.Functions.ErrorCheck(
                Directory.GetFiles(folderBrowserDialog1.SelectedPath).Length == 0,
                "Directory must be empty!"))
                return;

            tbx_project_path.Text = folderBrowserDialog1.SelectedPath;
            tbx_project_name.Text = folderBrowserDialog1.SelectedPath.Split('\\').Last();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
