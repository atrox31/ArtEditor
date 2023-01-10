using ArtCore_Editor.Assets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ArtCore_Editor.AdvancedAssets.Instance_Manager;
using ArtCore_Editor.AdvancedAssets.Instance_Manager.Behavior;
using ArtCore_Editor.AdvancedAssets.SceneManager;
using ArtCore_Editor.AdvancedAssets.SpriteManager;
using ArtCore_Editor.Assets.Font;
using ArtCore_Editor.Assets.Music;
using ArtCore_Editor.Assets.Sound;
using ArtCore_Editor.Assets.Texture;
using ArtCore_Editor.etc;
using ArtCore_Editor.Functions;
using ArtCore_Editor.Pick_forms;
using File = System.IO.File;
using Path = System.IO.Path;
using ArtCore_Editor.Main;

namespace ArtCore_Editor
{
    public partial class MainWindow : Form
    {
        public GameProject GlobalProject;
        private static MainWindow _instance;
        private bool _projectSaved = true;
        private readonly string _aCompilerVersion = null;

        private string GetTitleString() => $"ArtCore Editor({Program.Version}) ArtCompiler({_aCompilerVersion ?? "not found"}) - \"{GlobalProject.ProjectName}\"";

        private void MakeChange()
        {
            _projectSaved = false;
            Text = GetTitleString()+" *";
        }

        public void MakeSaved()
        {
            _projectSaved = true;
            Text = GetTitleString();
        }

        public MainWindow()
        {
            
            InitializeComponent();
            Program.ApplyTheme(this);

            // static instance to this object
            _instance = this;

            // get version of art compiler
            if (File.Exists(Program.ProgramDirectory + "\\" + "Core\\ACompiler.exe"))
            {
                Process compiler = new Process();
                compiler.StartInfo.FileName = Program.ProgramDirectory + "\\" + "Core\\ACompiler.exe";
                compiler.StartInfo.Arguments = "-version";
                compiler.StartInfo.RedirectStandardOutput = true;
                compiler.StartInfo.UseShellExecute = false;
                compiler.StartInfo.CreateNoWindow = true;
                compiler.Start();
                List<string> output = new List<string>();
                while (compiler.StandardOutput.ReadLine() is { } standardOutput)
                {
                    output.Add(standardOutput);
                }

                compiler.WaitForExit();
                if (output.Count >= 2)
                {
                    _aCompilerVersion = output.Last();
                }
            }
        }

        public static MainWindow GetInstance()
        {
            return _instance;
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalProject.SaveToFile();
            MakeSaved();
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_projectSaved)
            {
                if (MessageBox.Show("Project is unsaved, if You start new all changes are deleted. Are You sure?",
                        "Starting new Project", MessageBoxButtons.YesNo, MessageBoxIcon.Question) ==
                    DialogResult.No) return;
            }
            NewProjectWindow npw = new NewProjectWindow();
            if (npw.ShowDialog() == DialogResult.OK)
            {
                MakeSaved();
                RefreshListView(false);
            }
        }

        private void OpenProject(string pathname = null)
        {
            if (!_projectSaved)
            {
                if (MessageBox.Show("Project is unsaved, if You start new all changes are deleted. Are You sure?", "Starting new Project", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            }
            if (pathname == null)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "ArtCore Project|" + Program.ProjectFilename;
                dialog.AddExtension = true;
                dialog.Multiselect = false;
                dialog.Title = "Select Project";
                dialog.CheckFileExists = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    pathname = dialog.FileName;
                }
                else
                {
                    return;
                }
            }

            if (File.Exists(pathname))
            {
                LoadScreen ls = new LoadScreen(true);
                ls.Show();

                if (GlobalProject != null)
                {
                    GlobalProject.Dispose();
                }
                GC.Collect();

                GameProject.ProjectPath = Path.GetDirectoryName(pathname);
                GlobalProject = GameProject.LoadFromFile(pathname);

                ls.Close();
                if (GlobalProject != null)
                {
                    RefreshListView(false);
                    MakeSaved();
                }
                else
                {
                    return;
                }

            }

            // update last.txt
            List<string> lines = new List<string>();
            if (File.Exists(Program.ProgramDirectory + "\\" + Program.LastFilename))
            {
                lines = File.ReadAllLines(Program.ProgramDirectory + "\\" + Program.LastFilename).ToList();
            }
            // delete doubled projects
            lines = lines.Distinct().ToList();
            if (lines.Contains(Path.GetDirectoryName(pathname)))
            {
                lines.Remove(Path.GetDirectoryName(pathname));
            }
            lines.Insert(0, Path.GetDirectoryName(pathname));
            File.WriteAllLines(Program.ProgramDirectory + "\\" + Program.LastFilename, lines);

        }

        private void loadProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenProject(null);
        }

        private void AddToAssetList<T>(string category, Dictionary<string, T> objects, string categoryIcon)
        {
            int lastNode = ProjectAsserList.Nodes.Count;
            int categoryIndex = ProjectAssetList_imagelist.Images.IndexOfKey(categoryIcon);
            if (categoryIndex < 0)
            {
                ProjectAssetList_imagelist.Images.Add(Properties.Resources.ResourceManager.GetObject(categoryIcon) as Bitmap);
                categoryIndex = ProjectAssetList_imagelist.Images.Count - 1;
            }
            ProjectAsserList.Nodes.Add(category, category, categoryIndex, categoryIndex);
            ProjectAsserList.Nodes[lastNode].Tag = "root";

            if (objects.Count <= 0) return;
            foreach (T asset in objects.Values.ToList())
            {
                Asset currentAsset = asset.ForceType<Asset>();
                ProjectAsserList.Nodes[lastNode].Nodes.Add(currentAsset.Name, currentAsset.Name);
                ProjectAsserList.Nodes[lastNode].Nodes[^1].Tag = "element";
            }
        }

        public void RefreshListView(bool rememberStates = true)
        {
            List<string> savedExpansionState = new List<string>();
            if (rememberStates)
            {
                savedExpansionState = ProjectAsserList.Nodes.GetExpansionState();
                ProjectAsserList.BeginUpdate();
            }

            ProjectAsserList.Nodes.Clear();
            AddToAssetList("textures", GlobalProject.Textures, "picture");
            AddToAssetList("sprites", GlobalProject.Sprites, "running");
            AddToAssetList("sounds", GlobalProject.Sounds, "volume");
            AddToAssetList("music", GlobalProject.Music, "music-alt");
            AddToAssetList("font", GlobalProject.Fonts, "text");
            AddToAssetList("instances", GlobalProject.Instances, "users-alt");
            AddToAssetList("scenes", GlobalProject.Scenes, "globe");

            if (rememberStates)
            {
                ProjectAsserList.Nodes.SetExpansionState(savedExpansionState);
                ProjectAsserList.EndUpdate();
            }
        }
        
        private void OpenManager<T>(T obj) where T : Form
        {
            if (obj.ShowDialog() == DialogResult.OK)
            {
                RefreshListView();
                MakeChange();
            }
            obj.Dispose();
            GC.Collect();
        }
        
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {//open texture manager and create new asset
            OpenManager(new TextureManager());
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {//open sprite manager and create new asset
            OpenManager(new SpriteEditor());  
        }

        private void newToolStripMenuItem2_Click(object sender, EventArgs e)
        {//open music manager and create new asset
            OpenManager(new MusicManager());
        }

        private void newToolStripMenuItem3_Click(object sender, EventArgs e)
        {//open sound manager and create new asset
            OpenManager(new SoundManager());
        }

        private void newToolStripMenuItem4_Click(object sender, EventArgs e)
        {//open font manager and create new asset
            OpenManager(new FontManager());
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {//open object manager and create new asset
            OpenManager(new ObjectManager());
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {//open scene manager and create new asset
            OpenManager(new SceneManager());
        }

        private void OpenAssetFromList(string category, string name)
        {
            switch (category)
            {
                case "textures":
                    OpenManager(new TextureManager(name));
                    break;
                case "sprites":
                    OpenManager(new SpriteEditor(name));
                    break;
                case "music":
                    OpenManager(new MusicManager(name));
                    break;
                case "sounds":
                    OpenManager(new SoundManager(name));
                    break;
                case "font":
                    OpenManager(new FontManager(name));
                    break;
                case "instances":
                    OpenManager(new ObjectManager(name));
                    break;
                case "scenes":
                    OpenManager(new SceneManager(name));
                    break;
            }
        }

        private void DeleteAssetFilesFromProject<T>(T list, string name) where T : Dictionary<string, Asset>
        {
            if (list.ContainsKey(name))
            {
                list.Remove(name);
                RefreshListView();
                MakeChange();
            }
            else
            {
                // error?
            }
        }
        private void DeleteAssetFromList(string category, string name)
        {
            if (MessageBox.Show("Do You really want to delete '" + name + "'?", "Question?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            switch (category)
            {
                case "textures":
                    DeleteAssetFilesFromProject(GlobalProject.Textures, name);
                    break;
                case "sprites":
                    DeleteAssetFilesFromProject((GlobalProject.Sprites).ForceType<Dictionary<string, Asset>>(), name);
                    break;
                case "music":
                    DeleteAssetFilesFromProject(GlobalProject.Music, name);
                    break;
                case "sounds":
                    DeleteAssetFilesFromProject(GlobalProject.Sounds, name);
                    break;
                case "font":
                    DeleteAssetFilesFromProject(GlobalProject.Fonts, name);
                    break;
                case "instances":
                    DeleteAssetFilesFromProject((GlobalProject.Instances).ForceType<Dictionary<string, Asset>>(), name);
                    break;
                case "scenes":
                    DeleteAssetFilesFromProject((GlobalProject.Scenes).ForceType<Dictionary<string, Asset>>(), name);
                    break;
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // if asset is clicked, nor category
            if (e.Node.Parent == null) return;
            string category = e.Node.FullPath.Split('\\')[0];
            string name = e.Node.FullPath.Split('\\').Last();
            OpenAssetFromList(category, name);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                OpenProject(args[1]);
                if(GlobalProject == null)
                {
                    MessageBox.Show("Error while opening file'" + args[1] + "'", "Error opening", MessageBoxButtons.OK, MessageBoxIcon.Stop); return;
                }
                
            }
            while (GlobalProject == null)
            {
                Welcome welcome = new Welcome();
                welcome.ShowDialog();
                if (welcome.DialogResult == DialogResult.OK) // open recent
                {
                    OpenProject(welcome.OpenProject + "\\" + Program.ProjectFilename);
                }
                else if (welcome.DialogResult == DialogResult.Yes) // create
                {

                    NewProjectWindow npw = new NewProjectWindow();
                    if (npw.ShowDialog() == DialogResult.OK)
                    {
                        OpenProject(GameProject.ProjectPath);
                    }

                }
                else if (welcome.DialogResult == DialogResult.Cancel) // open from file
                {
                    OpenProject();
                }
                else if (welcome.DialogResult == DialogResult.Abort) // exit
                {
                    Application.Exit();
                    return;
                }
            }

            RefreshListView();
            MakeSaved();
            if (!CheckCoreFiles())
            {
                if (!UpdateProgram())
                {
                    MessageBox.Show("Cannot find core files, You can create game but not enable to test or release it. Try  'Project->Update Core' to update or download latest 'Core.tar' from 'https://github.com/atrox31/ArtCore'", "Cannot find 'Core'", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }


        private void listContext_root(object sender, ToolStripItemClickedEventArgs e)
        {
            /* 
             * cms.Items.Add("Expand All");
               cms.Items.Add("Coolapse All");
               cms.Items.Add("Add new folder");
            */
            if (e.ClickedItem == null) return;
            string category = ProjectAsserList.SelectedNode.FullPath.Split('\\')[0];
            string name = ProjectAsserList.SelectedNode.FullPath.Split('\\').Last();
            switch (e.ClickedItem.Text)
            {
                case "Expand All":
                    ProjectAsserList.ExpandAll();
                    break;
                case "Coolapse All":
                    ProjectAsserList.CollapseAll();
                    break;
                case "Add new folder":
                    // TODO
                    break;
                default:
                    //"New item '" + e.Node.Text + "'"
                    string item = e.ClickedItem.Text.Split("'")[1];
                    OpenAssetFromList(item, null);
                    break;
            }
        }
        private void listContext_element(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == null) return;
            if (ProjectAsserList.SelectedNode == null) return;

            string category = ProjectAsserList.SelectedNode.FullPath.Split('\\')[0];
            string name = ProjectAsserList.SelectedNode.FullPath.Split('\\').Last();
            switch (e.ClickedItem.Text)
            {
                case "Open":
                    OpenAssetFromList(category, name);
                    break;
                case "Delete":
                    DeleteAssetFromList(category, name);
                    break;
                case "Move":
                    // TODO
                    break;
                default: break;
            }
        }
        private void listContext_folder(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == null) return;
            string category = ProjectAsserList.SelectedNode.FullPath.Split('\\')[0];
            string name = ProjectAsserList.SelectedNode.FullPath.Split('\\').Last();
            switch (e.ClickedItem.Text)
            {
                case "Open":
                    // TODO
                    break;
                case "Delete":
                    // TODO
                    break;
                case "Move":
                    // TODO
                    break;
                default: break;
            }
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.Node.Tag != null)
                {
                    ProjectAsserList.SelectedNode = e.Node;

                    if (e.Node.Tag.ToString() == "root")
                    {
                        ContextMenuStrip cms = new ContextMenuStrip();
                        cms.ItemClicked += listContext_root;

                        cms.Items.Add("New item '" + e.Node.Text + "'");
                        cms.Items.Add("Expand All");
                        cms.Items.Add("Coolapse All");
                        //TODO cms.Items.Add("Add new folder");

                        cms.Show(MousePosition);
                    }
                    else if (e.Node.Tag.ToString() == "folder")
                    {
                        // TODO
                        ContextMenuStrip cms = new ContextMenuStrip();
                        cms.ItemClicked += listContext_folder;
                        cms.Items.Add("Expand All");
                        cms.Items.Add("Coolapse All");
                        cms.Items.Add("Add new folder");
                        cms.Items.Add("Delete");

                        cms.Show(MousePosition);
                    }
                    else if (e.Node.Tag.ToString() == "element")
                    {
                        ContextMenuStrip cms = new ContextMenuStrip();
                        cms.ItemClicked += listContext_element;
                        cms.Items.Add("Open");
                        cms.Items.Add("Delete");
                        //TODO cms.Items.Add("Move");

                        cms.Show(MousePosition);
                    }
                }
            }
        }

        void RunGame(bool debugMode)
        {
            string binPath = debugMode ? "bin_Debug" : "bin_Release";
            string fileNamePath = $"{Program.ProgramDirectory}\\Core\\{binPath}\\ArtCore.exe";
            if (!File.Exists(fileNamePath))
            {
                MessageBox.Show("ArtCore not found, try to update application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Process compiler = new Process();
            compiler.StartInfo.FileName = fileNamePath;
            compiler.StartInfo.Arguments = (debugMode ? "-debug " : "") + "-assets \"" + GameProject.ProjectPath + "\\assets.pak\" -game_dat \"" + GameProject.ProjectPath + "\\game.dat\"";
            compiler.StartInfo.UseShellExecute = false;
            compiler.StartInfo.CreateNoWindow = true;
            compiler.StartInfo.RedirectStandardOutput = debugMode;
            compiler.StartInfo.RedirectStandardError = debugMode;

            StringBuilder sb = new StringBuilder();
            listBox1.Items.Clear();
            listBox1.Items.Add(compiler.StartInfo.Arguments);
            if (debugMode)
            {
                compiler.OutputDataReceived += (sender, args) => sb.AppendLine(args.Data);
                compiler.ErrorDataReceived += (sender, args) => sb.AppendLine(args.Data);
            }

            compiler.Start();
            if (debugMode)
            {
                compiler.BeginOutputReadLine();
                compiler.BeginErrorReadLine();
            }
            compiler.WaitForExit();
            listBox1.Items.Add("Process exit with code: " + compiler.ExitCode.ToString());

            if (!debugMode) return;
            {
                listBox1.Items.Add("wait for console output...");
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += (object sender, DoWorkEventArgs e) =>
                {
                    foreach (string line in ((StringBuilder)e.Argument).ToString().Split('\n'))
                    {
                        listBox1.Invoke(new Action(() => listBox1.Items.Add(line)));
                    }
                };
                bw.RunWorkerAsync(sb);
            }

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // run
            if (CheckCoreFiles())
            {
                GameCompiler gameCompiler = new GameCompiler(false, true);
                if (gameCompiler.ShowDialog() != DialogResult.OK) return;
                RunGame(false);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            // run debug
            if (CheckCoreFiles())
            {
                GameCompiler gameCompiler = new GameCompiler(true);
                if (gameCompiler.ShowDialog() != DialogResult.OK) return;
            }
            RunGame(true);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArtCoreSettings settings = new ArtCoreSettings();
            settings.ShowDialog();
            // Save settings is automatic
        }

        private void setStartSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Functions.Functions.ErrorCheck(GlobalProject.Scenes.Count > 0, "There is no scenes!")) return;
            string scene = PicFromList.Get(GlobalProject.Scenes.Keys.ToList());
            if (scene == null) return;
            GlobalProject.StartingScene = GlobalProject.Scenes[scene].Name;
        }

        // move game files to output folder, prepare to release game
        private void ReleaseGame(string platform)
        {
            // run
            if (CheckCoreFiles())
            {
                GameCompiler gameCompiler = new GameCompiler(false, false, true);
                if (gameCompiler.ShowDialog() != DialogResult.OK) return;

                string output = GameProject.ProjectPath + "\\output_" + platform + "\\" + GlobalProject.ProjectName;
                if (Directory.Exists(output))
                {
                    Directory.Delete(output, true);
                }
                Directory.CreateDirectory(output);

                List<string> files = new List<string>
                {
                    // standard assets multi-platform
                    GameProject.ProjectPath + "\\game" + Program.FileExtensions_GameDataPack,
                    GameProject.ProjectPath + "\\assets.pak"
                };
                files.AddRange(Directory.GetFiles("Core\\bin_Release_" + platform + "+\\"));

                foreach (string file in files.Where(File.Exists))
                {
                    File.Copy(file, output + "\\" + Path.GetFileName(file), true);
                }

                MessageBox.Show("Game files are prepared for release", "Operation complete");
                Process.Start("explorer.exe", output);

            }
        }

        // split content in filelist.txt in core.tar to separate file names(path included)
        List<string> StripFileList(List<string> content)
        {
            List<string> output = new List<string>();

            string folder = "";
            bool readFolderName = true;
            bool readFileNames = false;

            foreach (string line in content)
            {
                //folder separator
                if (line.Length == 0)
                {
                    readFolderName = true;
                    readFileNames = false;
                    folder = "";
                    continue;
                }

                // new folder name
                if (readFolderName)
                {
                    if (line[1] == ':')
                    {
                        folder = "Core";
                    }
                    else
                    {
                        folder = "Core" + line.Substring(1, line.Length - 2).Replace('/', '\\');
                    }
                    readFolderName = false;
                    readFileNames = true;
                    continue;
                }

                // files in list
                if (readFileNames)
                {
                    if (line.Contains('.'))
                    {
                        output.Add(folder + "\\" + line);
                    }
                }

            }
            return output;
        }

        bool CheckCoreFiles(bool showMsg = true)
        {
            if (File.Exists(Program.ProgramDirectory + "\\" + "Core\\FileList.txt"))
            {
                List<string> list = StripFileList(File.ReadAllLines(Program.ProgramDirectory + "\\" + "Core\\FileList.txt").ToList());
                foreach (string line in list)
                {
                    if (!File.Exists(Program.ProgramDirectory + "\\" + line))
                    {
                        if (showMsg)
                            MessageBox.Show("Cannot find '" + line + "', download latest release from github\n 'https://github.com/atrox31/ArtCore'", "Missing file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                return true;
            }
            if (showMsg)
                MessageBox.Show("Cannot find core list file, download latest release from github\n 'https://github.com/atrox31/ArtCore'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private void updateCoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateProgram();
        }

        bool UpdateProgram(string file = null)
        {
            string fileName;
            if (file == null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Core.tar|Core.tar";
                openFileDialog.Multiselect = false;
                openFileDialog.AddExtension = true;
                openFileDialog.CheckFileExists = true;
                if (openFileDialog.ShowDialog() != DialogResult.OK) return false;
                fileName = openFileDialog.FileName;
            }
            else
            {
                fileName = file;
            }

            string tempDirectory = Program.ProgramDirectory + "\\" + "temp";
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
            //Directory.CreateDirectory("temp");
            try
            {
                Tar.ExtractTar(fileName, tempDirectory);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while opening '" + fileName + "'\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (!File.Exists(tempDirectory + "\\Core\\FileList.txt"))
            {
                MessageBox.Show("Error while opening 'FileList.txt'\nDownload latest release from github\n 'https://github.com/atrox31/ArtCore'", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (Directory.Exists(Program.ProgramDirectory + "\\" + "Core"))
            {
                Directory.Delete(Program.ProgramDirectory + "\\" + "Core", true);
            }
            foreach (string content in StripFileList(File.ReadAllLines(tempDirectory + "\\Core\\FileList.txt").ToList()))
            {
                string path = Program.ProgramDirectory + "\\" + Path.GetDirectoryName(content);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                
                File.Copy(tempDirectory + "\\" + content, Program.ProgramDirectory + "\\" + content, true);

            }
            if (Directory.Exists(tempDirectory))
            {
                Directory.Delete(tempDirectory, true);
            }
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            if (file == null) // silent
            {
                MessageBox.Show("Core files update!", "Complite", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // clear ArtLib content
            ScriptEditor.UpdateFunctionList();
            return true;
        }

        private void runToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //test in debug run
            if (CheckCoreFiles())
            {
                RunGame(true);
            }
        }

        private void compileRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // test compile and run debug
            MakeSaved();
            if (CheckCoreFiles())
            {
                GameCompiler gameCompiler = new GameCompiler(true, true);
                gameCompiler.ShowDialog();
            }
        }

        private void runToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            // test release
            if (CheckCoreFiles())
            {
                RunGame(false);
            }
        }

        private void compileAndRunToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // test compile and run release
            MakeSaved();
            if (CheckCoreFiles())
            {
                GameCompiler gameCompiler = new GameCompiler(false, true);
                gameCompiler.ShowDialog();
            }
        }

        private void llToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ReleaseGame(platform)
        }
    }
}
