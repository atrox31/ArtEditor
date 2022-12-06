using ArtCore_Editor.Assets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media;
using System.Xml.Linq;
using ArtCore_Editor.Assets.Sound;
using ArtCore_Editor.Assets.Sprite;
using ArtCore_Editor.Assets.Texture;
using ArtCore_Editor.Instance_Manager;
using File = System.IO.File;
using Path = System.IO.Path;

namespace ArtCore_Editor
{
    public partial class MainWindow : Form
    {
        public GameProject GlobalProject;
        private static MainWindow _instance;
        private bool _projectSaved = true;

        private readonly List<string> _filesToDeleteAtSave = new List<string>();

        private string GetTitleString() => $"ArtCore Editor - \"{GlobalProject.ProjectName}\"";

        public void MakeChange()
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
            // change number separator to .
            var customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

            InitializeComponent(); Program.ApplyTheme(this);
            
            // static instance to this object
            _instance = this;
        }

        public static MainWindow GetInstance()
        {
            return _instance;
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GlobalProject.SaveToFile();
            foreach (var file in _filesToDeleteAtSave)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
                if (Directory.Exists(file))
                {
                    Directory.Delete(file, true);
                }
            }
            _filesToDeleteAtSave.Clear();
            MakeSaved();
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!_projectSaved)
            {
                if (MessageBox.Show("Project is unsaved, if You start new all changes are deleted. Are You sure?", "Starting new Project", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            }

            //folderBrowserDialog1.Reset();
            while (true)
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (Directory.GetFiles(folderBrowserDialog1.SelectedPath).Length > 0)
                    {
                        MessageBox.Show("Directory must be empty!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        var loadScreen = new LoadScreen(false);
                        loadScreen.Show();

                        var path = folderBrowserDialog1.SelectedPath;
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
                        Directory.CreateDirectory(path + "\\gui");

                        GlobalProject.SaveToFile();
                        MakeSaved();

                        loadScreen.SetProgress(80);
                        var lines = new List<string>();
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

        }

        private void OpenProject(string pathname = null)
        {
            if (!_projectSaved)
            {
                if (MessageBox.Show("Project is unsaved, if You start new all changes are deleted. Are You sure?", "Starting new Project", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            }
            if (pathname == null)
            {
                var dialog = new OpenFileDialog();
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
                var ls = new LoadScreen(true);
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
            
            var lines = new List<string>();
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + Program.LastFilename))
            {
                lines = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\" + Program.LastFilename).ToList();
            }
            if (lines.Contains(Path.GetDirectoryName(pathname)))
            {
                lines.Remove(Path.GetDirectoryName(pathname));
            }
            lines.Insert(0, Path.GetDirectoryName(pathname));
            File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\" + Program.LastFilename, lines);

        }

        private void loadProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenProject(null);
        }

        private void AddToAssetList<T>(string category, Dictionary<string, T> objects, string categoryIcon)
        {
            var lastNode = ProjectAsserList.Nodes.Count;
            var categoryIndex = ProjectAssetList_imagelist.Images.IndexOfKey(categoryIcon);
            if (categoryIndex < 0)
            {
                ProjectAssetList_imagelist.Images.Add(Properties.Resources.ResourceManager.GetObject(categoryIcon) as Bitmap);
                categoryIndex = ProjectAssetList_imagelist.Images.Count - 1;
            }
            ProjectAsserList.Nodes.Add(category, category, categoryIndex, categoryIndex);
            ProjectAsserList.Nodes[lastNode].Tag = "root";

            if (objects.Count <= 0) return;
            foreach (var asset in objects.Values.ToList())
            {
                var currentAsset = asset.ForceType<Asset>();
                ProjectAsserList.Nodes[lastNode].Nodes.Add(currentAsset.Name, currentAsset.Name);
                ProjectAsserList.Nodes[lastNode].Nodes[^1].Tag = "element";
            }
        }

        public void RefreshListView(bool rememberStates = true)
        {
            var savedExpansionState = new List<string>();
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
            _filesToDeleteAtSave.Add(GameProject.ProjectPath + "\\" + list[name].FileName);
            list.Remove(name);
            RefreshListView();
            MakeChange();
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
            var category = e.Node.FullPath.Split('\\')[0];
            var name = e.Node.FullPath.Split('\\').Last();
            OpenAssetFromList(category, name);
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            var args = Environment.GetCommandLineArgs();
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
                var welcome = new Welcome();
                welcome.ShowDialog();
                if (welcome.DialogResult == DialogResult.OK) // open recent
                {
                    OpenProject(welcome.OpenProject + "\\" + Program.ProjectFilename);
                }
                else if (welcome.DialogResult == DialogResult.Yes) // create
                {
                    newProjectToolStripMenuItem_Click(null, null);

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
            var category = ProjectAsserList.SelectedNode.FullPath.Split('\\')[0];
            var name = ProjectAsserList.SelectedNode.FullPath.Split('\\').Last();
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
                    var item = e.ClickedItem.Text.Split("'")[1];
                    OpenAssetFromList(item, null);
                    break;
            }
        }
        private void listContext_element(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == null) return;
            if (ProjectAsserList.SelectedNode == null) return;

            var category = ProjectAsserList.SelectedNode.FullPath.Split('\\')[0];
            var name = ProjectAsserList.SelectedNode.FullPath.Split('\\').Last();
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
            var category = ProjectAsserList.SelectedNode.FullPath.Split('\\')[0];
            var name = ProjectAsserList.SelectedNode.FullPath.Split('\\').Last();
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
                        var cms = new ContextMenuStrip();
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
                        var cms = new ContextMenuStrip();
                        cms.ItemClicked += listContext_folder;
                        cms.Items.Add("Expand All");
                        cms.Items.Add("Coolapse All");
                        cms.Items.Add("Add new folder");
                        cms.Items.Add("Delete");

                        cms.Show(MousePosition);
                    }
                    else if (e.Node.Tag.ToString() == "element")
                    {
                        var cms = new ContextMenuStrip();
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
            var fileNamePath = $"{AppDomain.CurrentDomain.BaseDirectory}\\Core\\{binPath}\\ArtCore.exe";
            if (!File.Exists(fileNamePath))
            {
                MessageBox.Show("ArtCore not found, try to update application.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var compiler = new Process();
            compiler.StartInfo.FileName = fileNamePath;
            compiler.StartInfo.Arguments = (debugMode ? "-debug " : "") + "-assets \"" + GameProject.ProjectPath + "\\assets.pak\" -game_dat \"" + GameProject.ProjectPath + "\\game.dat\"";
            compiler.StartInfo.UseShellExecute = false;
            compiler.StartInfo.CreateNoWindow = true;
            compiler.StartInfo.RedirectStandardOutput = debugMode;
            compiler.StartInfo.RedirectStandardError = debugMode;

            var sb = new StringBuilder();
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
                var bw = new BackgroundWorker();
                bw.DoWork += (object sender, DoWorkEventArgs e) =>
                {
                    foreach (var line in ((StringBuilder)e.Argument).ToString().Split('\n'))
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
                var gameCompiler = new GameCompiler(false, true);
                if (gameCompiler.ShowDialog() != DialogResult.OK) return;
                RunGame(false);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            // run debug
            if (CheckCoreFiles())
            {
                var gameCompiler = new GameCompiler(true);
                if (gameCompiler.ShowDialog() != DialogResult.OK) return;
            }
            RunGame(true);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var settings = new ArtCoreSettings();
            settings.ShowDialog();
            // Save settings is automatic
        }

        // search for unsused assets
        private int DeleteUnusedFiles<T>(string path, T container) where T : Dictionary<string, Asset>
        {
            var total = 0;

            foreach (var item in Directory.GetFiles(GameProject.ProjectPath + "\\" + path))
            {
                if (GlobalProject.Textures.ContainsKey(Path.GetFileNameWithoutExtension(item))) continue;
                _filesToDeleteAtSave.Add(item);
                total++;
            }

            return total;
        }

        // clean up files in project directory, files not used in project
        private void cleanupFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var total = 0;
            total += DeleteUnusedFiles("\\assets\\texture", GlobalProject.Textures);
            total += DeleteUnusedFiles("\\assets\\font", GlobalProject.Fonts);
            total += DeleteUnusedFiles("\\assets\\music", GlobalProject.Music);
            total += DeleteUnusedFiles("\\assets\\sound", GlobalProject.Sounds);
            total += DeleteUnusedFiles("\\assets\\sprite", (GlobalProject.Sprites).ForceType< Dictionary<string, Asset> > ());
            total += DeleteUnusedFiles("\\scene", (GlobalProject.Scenes).ForceType<Dictionary<string, Asset>>());
            total += DeleteUnusedFiles("\\object", (GlobalProject.Instances).ForceType<Dictionary<string, Asset>>());
            MessageBox.Show($"Cleanup complite, deleted {total.ToString()}  elements", "Cleanup");
        }

        private void setStartSceneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Functions.ErrorCheck(GlobalProject.Scenes.Count > 0, "There is no scenes!")) return;
            var scene = PicFromList.Get(GlobalProject.Scenes.Keys.ToList());
            if (scene == null) return;
            GlobalProject.StartingScene = GlobalProject.Scenes[scene];
        }

        // move game files to output folder, prepare to release game
        private void releaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // run
            if (CheckCoreFiles())
            {
                var gameCompiler = new GameCompiler(false,false,true);
                if (gameCompiler.ShowDialog() != DialogResult.OK) return;

                var output = GameProject.ProjectPath + "\\output\\" + GlobalProject.ProjectName;
                if (Directory.Exists(output))
                {
                    Directory.Delete(output, true);
                }
                Directory.CreateDirectory(output);

                var files = new List<string>();
                files.Add(GameProject.ProjectPath + "\\game.dat");
                files.Add(GameProject.ProjectPath + "\\assets.pak");
                foreach (var cfile in Directory.GetFiles("Core\\bin_Release\\"))
                {
                    files.Add(cfile);
                }

                foreach (var file in files)
                {
                    if (File.Exists(file))
                    {
                        File.Copy(file, output + "\\" + Path.GetFileName(file), true);
                    }
                }

                MessageBox.Show("Game files are prepared for release", "Operation complite");
                Process.Start("explorer.exe", output);

            }
        }


        // split content in filelist.txt in core.tar to separate file names(path included)
        List<string> StripFileList(List<string> content)
        {
            var output = new List<string>();

            var folder = "";
            var readFolderName = true;
            var readFileNames = false;

            foreach (var line in content)
            {
                //folder separator
                if (line.Length == 0)
                {
                    readFolderName = true;
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
                    if (line.Contains("."))
                    {
                        output.Add(folder + "\\" + line);
                    }
                }

            }
            return output;
        }

        bool CheckCoreFiles(bool showMsg = true)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + "Core\\FileList.txt"))
            {
                var list = StripFileList(File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\" + "Core\\FileList.txt").ToList());
                foreach (var line in list)
                {
                    if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + line))
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
                var openFileDialog = new OpenFileDialog();
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

            var tempDirectory = AppDomain.CurrentDomain.BaseDirectory + "\\" + "temp";
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

            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "\\" + "Core"))
            {
                Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + "\\" + "Core", true);
            }
            foreach (var content in StripFileList(File.ReadAllLines(tempDirectory + "\\Core\\FileList.txt").ToList()))
            {
                var path = AppDomain.CurrentDomain.BaseDirectory + "\\" + Path.GetDirectoryName(content);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                File.Copy(tempDirectory, AppDomain.CurrentDomain.BaseDirectory + "\\" + content, true);

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
            ScriptEditor.ClearFunctionList();
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
                var gameCompiler = new GameCompiler(true, true);
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
                var gameCompiler = new GameCompiler(false, true);
                gameCompiler.ShowDialog();
            }
        }

    }
}
