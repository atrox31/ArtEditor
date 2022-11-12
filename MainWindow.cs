﻿using ArtCore_Editor.Assets;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using File = System.IO.File;

namespace ArtCore_Editor
{
    public partial class MainWindow : Form
    {
        public GameProject Game_Project = null;
        static MainWindow _instance;
        bool saved = true;

        List<string> FilesToDeleteAtSave = new List<string>();
        List<string> FoldersToDeleteAtSave = new List<string>();

        public void MakeChange()
        {
            if (saved == false) return;
            saved = false;
            this.Text = "ArtCore Editor - \"" + Game_Project.ProjectName + "\" *";
        }
        public void MakeSaved()
        {
            saved = true;
            this.Text = "ArtCore Editor - \"" + Game_Project.ProjectName + "\"";
        }
        public MainWindow()
        {
            System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";

            System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;
            InitializeComponent();

            _instance = this;


        }

        public static MainWindow GetInstance()
        {
            return _instance;
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Game_Project.SaveToFile();
            foreach (var file in FilesToDeleteAtSave)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
            foreach (var folder in FoldersToDeleteAtSave)
            {
                if (Directory.Exists(folder))
                {
                    Directory.Delete(folder, true);
                }
            }
            FilesToDeleteAtSave.Clear();
            FoldersToDeleteAtSave.Clear();
            MakeSaved();
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!saved)
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
                        LoadScreen loadScreen = new LoadScreen(false);
                        loadScreen.Show();

                        string path = folderBrowserDialog1.SelectedPath;
                        Game_Project = new GameProject();
                        Game_Project.Prepare_new();
                        Game_Project.ProjectName = path.Split('\\').Last();
                        Game_Project.ProjectPath = path;

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

                        Game_Project.SaveToFile();
                        MakeSaved();

                        loadScreen.SetProgress(80);
                        List<string> lines = new List<string>();
                        if (File.Exists(Program.LAST_FILENAME))
                        {
                            lines = File.ReadAllLines(Program.LAST_FILENAME).ToList();
                        }
                        lines.Insert(0, path);
                        File.WriteAllLines(Program.LAST_FILENAME, lines);
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
            if (!saved)
            {
                if (MessageBox.Show("Project is unsaved, if You start new all changes are deleted. Are You sure?", "Starting new Project", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            }
            if (pathname == null)
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "ArtCore Project|" + Program.PROJECT_FILENAME;
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

                if (Game_Project != null)
                {
                    Game_Project.Dispose();
                }
                GC.Collect();

                Game_Project = GameProject.LoadFromFile(pathname);

                ls.Close();
                if (Game_Project != null)
                {
                    RefreshListView(false);
                    MakeSaved();
                }
                else
                {
                    return;
                }

            }



            List<string> lines = new List<string>();
            if (File.Exists(Program.LAST_FILENAME))
            {
                lines = File.ReadAllLines(Program.LAST_FILENAME).ToList();
            }
            lines.Insert(0, pathname);
            File.WriteAllLines(Program.LAST_FILENAME, lines);
        }

        private void loadProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenProject(null);

        }



        private void AddToAssetList<T>(string category, Dictionary<string, T> objects, string category_icon, string default_icon = null)
        {
            int lastnode = ProjectAsserList.Nodes.Count;
            int category_index = ProjectAssetList_imagelist.Images.IndexOfKey(category_icon);
            if (category_index < 0)
            {
                ProjectAssetList_imagelist.Images.Add(Properties.Resources.ResourceManager.GetObject(category_icon) as Bitmap);
                category_index = ProjectAssetList_imagelist.Images.Count - 1;
            }
            ProjectAsserList.Nodes.Add(category, category, category_index, category_index);
            ProjectAsserList.Nodes[lastnode].Tag = "root";


            if (objects.Count > 0)
            {

                foreach (var _asset in objects.Values.ToList())
                {

                    var asset = Functions.ForceType<Asset>(_asset);

                    int item_index = -1;
                    if (asset.EditorImage == null)
                    {
                        item_index = ProjectAssetList_imagelist.Images.IndexOfKey(default_icon);
                        if (item_index < 0)
                        {
                            if (default_icon == null)
                            {
                                item_index = category_index;
                            }
                            else
                            {
                                ProjectAssetList_imagelist.Images.Add(Properties.Resources.ResourceManager.GetObject(default_icon) as Bitmap);
                                item_index = ProjectAssetList_imagelist.Images.Count - 1;
                            }
                        }
                    }
                    else
                    {
                        if (!ProjectAssetList_imagelist.Images.ContainsKey(asset.EditorImage.ToString()))
                        {
                            ProjectAssetList_imagelist.Images.Add(asset.EditorImage);
                            item_index = ProjectAssetList_imagelist.Images.Count - 1;
                        }
                    }
                    ProjectAsserList.Nodes[lastnode].Nodes.Add(asset.Name, asset.Name, item_index, item_index);
                    ProjectAsserList.Nodes[lastnode].Nodes[ProjectAsserList.Nodes[lastnode].Nodes.Count - 1].Tag = "element";
                }
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
            AddToAssetList("textures", Game_Project.Textures, "picture");
            AddToAssetList("sprites", Game_Project.Sprites, "running");
            AddToAssetList("sounds", Game_Project.Sounds, "volume");
            AddToAssetList("music", Game_Project.Music, "music-alt");
            AddToAssetList("font", Game_Project.Fonts, "text");
            AddToAssetList("instances", Game_Project.Instances, "users-alt");
            AddToAssetList("scenes", Game_Project.Scenes, "globe");

            if (rememberStates)
            {
                ProjectAsserList.Nodes.SetExpansionState(savedExpansionState);
                ProjectAsserList.EndUpdate();
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {//texture
            TextureManager textureManager = new TextureManager();
            if (textureManager.ShowDialog() == DialogResult.OK)
            {
                RefreshListView();
                MakeChange();
            }
            textureManager.Dispose();
            GC.Collect();
        }

        private void newToolStripMenuItem1_Click(object sender, EventArgs e)
        {//sprite
            SpriteEditor spriteManager = new SpriteEditor(null);
            if (spriteManager.ShowDialog() == DialogResult.OK)
            {
                RefreshListView();
                MakeChange();
            }
            spriteManager.Dispose();
            GC.Collect();
        }

        private void newToolStripMenuItem2_Click(object sender, EventArgs e)
        {//music
            MusicManager musicManager = new MusicManager();
            if (musicManager.ShowDialog() == DialogResult.OK)
            {
                RefreshListView();
                MakeChange();
            }
            musicManager.Dispose();
            GC.Collect();
        }

        private void newToolStripMenuItem3_Click(object sender, EventArgs e)
        {//sound
            SoundManager soundManager = new SoundManager();
            if (soundManager.ShowDialog() == DialogResult.OK)
            {
                RefreshListView();
                MakeChange();
            }
            soundManager.Dispose();
            GC.Collect();
        }

        private void newToolStripMenuItem4_Click(object sender, EventArgs e)
        {//font
            FontManager fontManager = new FontManager();
            if (fontManager.ShowDialog() == DialogResult.OK)
            {
                RefreshListView();
                MakeChange();
            }
            fontManager.Dispose();
            GC.Collect();
        }
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ObjectManager objectManager = new ObjectManager();
            if (objectManager.ShowDialog() == DialogResult.OK)
            {
                RefreshListView();
                MakeChange();
            }
            objectManager.Dispose();
            GC.Collect();
        }

        void OpenAssetFromList(string category, string name)
        {
            switch (category)
            {
                case "textures":
                    TextureManager textureManager = new TextureManager(name);
                    if (textureManager.ShowDialog() == DialogResult.OK)
                    {
                        RefreshListView();
                        MakeChange();
                    }
                    textureManager.Dispose();
                    break;
                case "sprites":
                    SpriteEditor spriteManager = new SpriteEditor(Game_Project.Sprites[name]);
                    if (spriteManager.ShowDialog() == DialogResult.OK)
                    {
                        RefreshListView();
                        MakeChange();
                    }
                    spriteManager.Dispose();

                    break;
                case "animations":

                    break;
                case "music":
                    MusicManager musicManager = new MusicManager(name);
                    if (musicManager.ShowDialog() == DialogResult.OK)
                    {
                        RefreshListView();
                        MakeChange();
                    }
                    musicManager.Dispose();
                    break;
                case "sounds":
                    SoundManager soundsManager = new SoundManager(name);
                    if (soundsManager.ShowDialog() == DialogResult.OK)
                    {
                        RefreshListView();
                        MakeChange();
                    }
                    soundsManager.Dispose();
                    break;
                case "font":
                    FontManager fontManager = new FontManager(name);
                    if (fontManager.ShowDialog() == DialogResult.OK)
                    {
                        RefreshListView();
                        MakeChange();
                    }
                    fontManager.Dispose();
                    break;
                case "instances":
                    ObjectManager objectManager = new ObjectManager(name);
                    if (objectManager.ShowDialog() == DialogResult.OK)
                    {
                        RefreshListView();
                        MakeChange();
                    }
                    objectManager.Dispose();
                    break;
                case "scenes":

                    SceneManager sceneManager = new SceneManager(name);
                    if (sceneManager.ShowDialog() == DialogResult.OK)
                    {
                        RefreshListView();
                        MakeChange();
                    }
                    sceneManager.Dispose();
                    break;
                default:
                    break;
            }
            GC.Collect();
        }
        void DeleteAssetFromList(string category, string name)
        {
            if (MessageBox.Show("Do You really want to delete '" + name + "'?", "Question?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No) return;
            switch (category)
            {
                case "textures":
                    FilesToDeleteAtSave.Add(Game_Project.ProjectPath + "\\" + Game_Project.Textures[name].FileName);
                    Game_Project.Textures.Remove(name);
                    RefreshListView();
                    MakeChange();
                    break;
                case "sprites":
                    FoldersToDeleteAtSave.Add(Game_Project.ProjectPath + "\\" + Game_Project.Sprites[name].Name);
                    Game_Project.Sprites.Remove(name);
                    RefreshListView();
                    MakeChange();
                    break;
                case "objects":
                    FoldersToDeleteAtSave.Add(Game_Project.ProjectPath + "\\" + Game_Project.Instances[name].Name);
                    Game_Project.Instances.Remove(name);
                    RefreshListView();
                    MakeChange();
                    break;
                case "music":
                    FilesToDeleteAtSave.Add(Game_Project.ProjectPath + "\\" + Game_Project.Music[name].FileName);
                    Game_Project.Music.Remove(name);
                    RefreshListView();
                    MakeChange();
                    break;
                case "sounds":
                    FilesToDeleteAtSave.Add(Game_Project.ProjectPath + "\\" + Game_Project.Sounds[name].FileName);
                    Game_Project.Sounds.Remove(name);
                    RefreshListView();
                    MakeChange();
                    break;
                case "font":
                    FilesToDeleteAtSave.Add(Game_Project.ProjectPath + "\\" + Game_Project.Fonts[name].FileName);
                    Game_Project.Fonts.Remove(name);
                    RefreshListView();
                    MakeChange();
                    break;

                case "instances":
                    FoldersToDeleteAtSave.Add(Game_Project.ProjectPath + "\\" + Game_Project.Instances[name].Name);
                    Game_Project.Instances.Remove(name);
                    RefreshListView();
                    MakeChange();
                    break;
                case "scenes":
                    FoldersToDeleteAtSave.Add(Game_Project.ProjectPath + "\\" + Game_Project.Scenes[name].Name);
                    Game_Project.Scenes.Remove(name);
                    RefreshListView();
                    MakeChange();
                    break;
                default:
                    break;
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // czy kategoria
            if (e.Node.Parent != null)
            {
                //MessageBox.Show(e.Node.Parent.Name + " - " + e.Node.Text);
                string category = e.Node.FullPath.Split('\\')[0];
                string name = e.Node.FullPath.Split('\\').Last();
                OpenAssetFromList(category, name);
            }

        }

        private void updateCoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // TODO
            // online update
            Process update = new Process();
            update.StartInfo.FileName = @"..\..\CoreCopy.bat";
            update.Start();
            update.WaitForExit();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            while (Game_Project == null)
            {
                Welcome welcome = new Welcome();
                welcome.ShowDialog();
                if (welcome.DialogResult == DialogResult.OK) // open recent
                {
                    OpenProject(welcome.open_project + "\\" + Program.PROJECT_FILENAME);
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
        }


        private void listContext_root(object Sender, ToolStripItemClickedEventArgs e)
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

                    break;
                default: break;
            }
        }
        private void listContext_element(object Sender, ToolStripItemClickedEventArgs e)
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

                    break;
                default: break;
            }
        }
        private void listContext_folder(object Sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == null) return;
            string category = ProjectAsserList.SelectedNode.FullPath.Split('\\')[0];
            string name = ProjectAsserList.SelectedNode.FullPath.Split('\\').Last();
            switch (e.ClickedItem.Text)
            {
                case "Open":

                    break;
                case "Delete":

                    break;
                case "Move":

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
                        cms.Items.Add("Expand All");
                        cms.Items.Add("Coolapse All");
                        cms.Items.Add("Add new folder");

                        cms.Show(MousePosition);
                    }
                    else if (e.Node.Tag.ToString() == "folder")
                    {
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
                        cms.Items.Add("Move");

                        cms.Show(MousePosition);
                    }
                }
            }
        }

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SceneManager sceneManager = new SceneManager();
            if (sceneManager.ShowDialog() == DialogResult.OK)
            {
                RefreshListView();
                MakeChange();
            }
            sceneManager.Dispose();
            GC.Collect();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // run
            GameCompiler gameCompiler = new GameCompiler(false);
            if (gameCompiler.ShowDialog() == DialogResult.OK)
            {
                // run game
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            // run debug
            GameCompiler gameCompiler = new GameCompiler(true);
            if (gameCompiler.ShowDialog() == DialogResult.OK)
            {
                // run game in debug mode
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArtCore_Settings settings = new ArtCore_Settings();
            if (settings.ShowDialog() == DialogResult.OK)
            {
                // appy is automatic
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            GuiEditor ged = new GuiEditor();
            ged.ShowDialog();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
