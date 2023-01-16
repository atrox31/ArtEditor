using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ArtCore_Editor.AdvancedAssets.Instance_Manager;
using ArtCore_Editor.Assets;
using ArtCore_Editor.Functions;
using ArtCore_Editor.Main;
using Newtonsoft.Json;
using static ArtCore_Editor.Main.GameProject;

namespace ArtCore_Editor.AdvancedAssets.SceneManager
{
    public partial class SceneManager : Form
    {
        private bool _showGrid = true;
        private bool _snapGrid = true;
        private string _aid = null;
        private readonly Scene _cScene;
        private Point _grid;
        private bool _saved = false;
        private readonly ImageList _bcPreviewList = new ImageList();
        private Image _bcTexture = null;

        public class SceneInstance
        {
            [JsonIgnore] private static readonly Dictionary<string, Image> InstanceSprites = new Dictionary<string, Image>();
            public int X { get; set; }
            public int Y { get; set; }
            public float EditorMask { get; set; }
            //TODO change Instance type to string type, for better saving system
            public Instance Instance { get; set; }
            public readonly Image Img;
            public SceneInstance(int x, int y, Instance instance)
            {
                X = x;
                Y = y;
                Instance = instance;
                Img = Properties.Resources.interrogation1;

                if(instance?.Sprite != null)
                {
                    // first search in sprite list, if not add
                    if (!InstanceSprites.ContainsKey(instance.Sprite.FileName))
                    {
                        string pathToArchive = StringExtensions.Combine(ProjectPath, instance.Sprite.DataPath);
                        Bitmap image = ZipIO.ReadImageFromArchive(pathToArchive, "0.png");

                        InstanceSprites.Add(instance.Sprite.FileName,
                            image == null ? Properties.Resources.interrogation : image);
                    } 
                    Img = InstanceSprites[instance.Sprite.FileName];
                }

                EditorMask = (float)((Img.Width + Img.Height) / 4);
            }
        }

        private SceneInstance _selectedSceneInstance = null;

        private void MakeChange()
        {
            _saved = false;
            Text = "SceneManager - \"" + _cScene.Name + "\" *";
        }

        private void MakeSaved()
        {
            _saved = true;
            Text = "SceneManager - \"" + _cScene.Name + "\"";
        }

        public SceneManager(string assetId = null)
        {
            _aid = assetId;
            InitializeComponent();
            Program.ApplyTheme(this);
            Content.AllowDrop = true;
            _grid = new Point(32, 32);
            
            // background selector use this for scene drawing background options
            _bcPreviewList.ImageSize = new Size(128, 128);
            foreach (Asset item in GameProject.GetInstance().Textures.Values)
            {
                if (File.Exists(item.GetFilePath()))
                {
                    _bcPreviewList.Images.Add(Image
                        .FromFile(item.GetFilePath())
                        .GetThumbnailImage(128, 128, null, IntPtr.Zero));
                }
            }

            if (assetId == null)
            {
                _cScene = new Scene();
                return;
            }

            _cScene = GameProject.GetInstance().Scenes[assetId];

            Name = _cScene.Name;
            textBox2.Text = _cScene.Name;
            Content.Height = _cScene.ViewHeight;
            Content.Width = _cScene.ViewWidth;
            chb_enable_camera.Checked = _cScene.EnableCamera;
            numericUpDown1.Value = Content.Width;
            numericUpDown2.Value = Content.Height;

            // background 
            if (_cScene.BackGroundType == Scene.BackGroundTypeEnum.DrawColor)
            {
                r_bc_solidcolor.Select();
                bc_color_pick_value.Text = _cScene.BackGroundColor.ColorToHex();
                bc_color_box.BackColor = _cScene.BackGroundColor;
            }
            else
            {
                if (File.Exists( _cScene.BackGroundTexture.GetFilePath()))
                {
                    _bcTexture = Image.FromFile(_cScene.BackGroundTexture.GetFilePath());
                    r_bc_texture.Select();
                    if (_cScene.BackGroundWrapMode == WrapMode.Tile) rb_td_normal.Checked = true;
                    if (_cScene.BackGroundWrapMode == WrapMode.TileFlipX) rb_td_w.Checked = true;
                    if (_cScene.BackGroundWrapMode == WrapMode.TileFlipY) rb_td_h.Checked = true;
                    if (_cScene.BackGroundWrapMode == WrapMode.TileFlipXY) rb_td_w_h.Checked = true;

                    bc_selected_preview.BackgroundImage = _bcTexture.GetThumbnailImage(128, 128, null, IntPtr.Zero);
                }
            }

            MakeSaved();
        }

        private void SceneManager_Load(object sender, EventArgs e)
        {
            // load all assets
            foreach (KeyValuePair<string, Instance> item in GameProject.GetInstance().Instances)
            {
                if (item.Value.Sprite == null)
                {
                    instanceListView.Items.Add(item.Key, 0);
                }
                else
                {
                    Bitmap image = ZipIO.ReadImageFromArchive(
                        StringExtensions.Combine(ProjectPath, item.Value.Sprite.DataPath),
                        "0.png");
                    if(image != null)
                    {
                        Instance_imagelist.Images.Add(image.GetThumbnailImage(64, 64, null, IntPtr.Zero));
                    }

                    instanceListView.Items.Add(item.Key,
                        image == null ? 0 : Instance_imagelist.Images.Count - 1
                        );
                }
            }
            RedrawScene();
        }

        private void RedrawScene()
        {
            Content.Image?.Dispose();
            GC.Collect();

            Bitmap bmp = new Bitmap(Content.Width, Content.Height);

            // clear content
            if (_cScene.BackGroundType == Scene.BackGroundTypeEnum.DrawColor)
            {
                using Graphics gr = Graphics.FromImage(bmp);
                gr.Clear(_cScene.BackGroundColor);
            }
            else
            {
                if (_bcTexture != null)
                {
                    using Graphics g = Graphics.FromImage(bmp);
                    TextureBrush tBrush = new TextureBrush(_bcTexture);
                    /*
                         *  Tile = 0,
                            TileFlipX = 1,
                            TileFlipY = 2,
                            TileFlipXY = 3,
                        */

                    tBrush.WrapMode = _cScene.BackGroundWrapMode;

                    g.FillRectangle(tBrush, new Rectangle(0, 0, Content.Width, Content.Height));
                }
            }
            using (Graphics g = Graphics.FromImage(bmp))
            {
                if (_showGrid)
                {
                    for (int x = 0; x < bmp.Width; x += _grid.X)
                    {
                        g.DrawLine(new Pen(Color.LightGray, 1.0f), x, 0, x, bmp.Height);
                    }
                    for (int y = 0; y < bmp.Height; y += _grid.Y)
                    {
                        g.DrawLine(new Pen(Color.LightGray, 1.0f), 0, y, bmp.Width, y);
                    }
                }

                // instances
                foreach (SceneInstance item in _cScene.SceneInstances)
                {
                    g.DrawImage(item.Img, item.X - (int)item.EditorMask, item.Y - (int)item.EditorMask, item.Img.Width, item.Img.Height);
                    if (item == _selectedSceneInstance)
                    {
                        g.DrawRectangle(new Pen(Color.Red), new Rectangle(item.X - (int)item.EditorMask, item.Y - (int)item.EditorMask, item.Img.Width, item.Img.Height));

                        StringFormat format = new StringFormat();
                        format.LineAlignment = StringAlignment.Center;
                        format.Alignment = StringAlignment.Center;
                        g.DrawString(item.Instance.Name, DefaultFont, new SolidBrush(Color.Red), new Point(item.X, item.Y - (int)item.EditorMask - 8), format);
                        g.DrawString("X= " + item.X.ToString() + " | Y=" + item.Y.ToString(), DefaultFont, new SolidBrush(Color.Red), new Point(item.X, item.Y + (int)item.EditorMask + 8), format);
                    }

                }

            }
            // draw entire scene
            Content.Image = bmp;

        }



        void Save()
        {
            if (Functions.Functions.ErrorCheck(textBox2.Text.Length > 0, "Scene must have name!")) return;
            if (Functions.Functions.ErrorCheck(!textBox2.Text.Contains('#'), "Scene name can not contains chars like '#' or '&'")) return;
            if (Functions.Functions.ErrorCheck(!textBox2.Text.Contains('&'), "Scene name can not contains chars like '#' or '&'")) return;
            _cScene.Name = textBox2.Text;

            if (_aid == null)
            {
                GameProject.GetInstance().Scenes.Add(_cScene.Name, new Scene());
                _aid = _cScene.Name;
            }

            string pathToObjectData = GameProject.ProjectPath + "\\scene\\" + _cScene.Name;
            _cScene.FileName = "\\scene\\" + _cScene.Name + "\\" + _cScene.Name + Program.FileExtensions_SceneObject;
            _cScene.ProjectPath = "\\scene\\" + _cScene.Name;

            _cScene.SceneInstancesList.Clear();
            foreach (SceneInstance ins in _cScene.SceneInstances)
            {
                _cScene.SceneInstancesList.Add($"{ins.Instance.Name}|{ins.X}|{ins.Y}");
            }

            Directory.CreateDirectory(GameProject.ProjectPath + _cScene.ProjectPath);
            if (_aid != _cScene.Name)
            {
                if (GameProject.GetInstance().Scenes.ContainsKey(_cScene.Name))
                {
                    MessageBox.Show("Scene with target name exists! Can not override, first delete other scene.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                GameProject.GetInstance().Scenes.RenameKey(_aid, _cScene.Name);
                foreach (string enumerateFile in Directory.EnumerateFiles(GameProject.ProjectPath + "\\scene\\" + _aid, "*" + Program.FileExtensions_ArtCode))
                {// move all scene triggers
                    File.Move(enumerateFile, 
                        pathToObjectData + "\\" + Path.GetFileName(enumerateFile)
                        );
                }

                if (File.Exists(GameProject.ProjectPath + "\\scene\\" + _aid + "\\GuiSchema.json"))
                { // move gui schema
                    File.Move(GameProject.ProjectPath + "\\scene\\" + _aid + "\\GuiSchema.json", GameProject.ProjectPath + "\\scene\\" + _cScene.Name + "\\GuiSchema.json");
                }
                
                if (Directory.Exists(GameProject.ProjectPath + "\\scene\\" + _aid))
                { // delete old folder
                    Directory.Delete(GameProject.ProjectPath + "\\scene\\" + _aid, true);
                }

                _aid = _cScene.Name;
            }

            using (FileStream createStream = File.Create(GameProject.ProjectPath + _cScene.FileName))
            {
                byte[] buffer = JsonConvert.SerializeObject(_cScene).Select(c => (byte)c).ToArray();
                createStream.Write(buffer);
            }

            GameProject.GetInstance().Scenes[_aid] = (Scene)_cScene.Clone();

            GC.Collect();
            MakeSaved();

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // width change
            _cScene.ViewWidth = (int)numericUpDown1.Value;
            Content.Width = _cScene.ViewWidth;
            RedrawScene();
            MakeChange();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            // height change
            _cScene.ViewHeight = (int)numericUpDown2.Value;
            Content.Height = _cScene.ViewHeight;
            RedrawScene();
            MakeChange();
        }
        
        private void SceneManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_saved)
            {
                this.DialogResult = DialogResult.OK;
                return;
            }
            switch (MessageBox.Show("You made some changes, save?", "Save variables?",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    Save();
                    this.DialogResult = DialogResult.OK;
                    break;
                case DialogResult.No:
                    this.DialogResult = DialogResult.No;
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
            }
            
        }
    }
}
