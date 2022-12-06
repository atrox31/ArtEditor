using ArtCore_Editor.Instance_Manager;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static ArtCore_Editor.GameProject;

namespace ArtCore_Editor
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
            public Instance Instance { get; set; }
            public readonly Image Img;
            public SceneInstance(int x, int y, Instance instance)
            {
                X = x;
                Y = y;
                Instance = instance;

                if (instance == null)
                {
                    Img = Properties.Resources.interrogation1;
                }
                else
                {
                    if (instance.Sprite == null)
                    {
                        Img = Properties.Resources.interrogation;
                    }
                    else
                    {
                        if (!InstanceSprites.ContainsKey(instance.Sprite.FileName))
                        {
                            Image tmp = Image.FromFile(GameProject.ProjectPath + "\\" + instance.Sprite.ProjectPath + "\\img\\0.png");
                            InstanceSprites.Add(instance.Sprite.FileName, tmp);
                        }
                        Img = InstanceSprites[instance.Sprite.FileName];
                    }
                }

                EditorMask = (Img.Width + Img.Height) / 4;
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
            foreach (var item in GameProject.GetInstance().Textures)
            {
                _bcPreviewList.Images.Add(Image
                    .FromFile(ProjectPath + "\\" + item.Value.ProjectPath + "\\" + item.Value.FileName)
                    .GetThumbnailImage(128, 128, null, IntPtr.Zero));
            }
            
            if (assetId == null)
            {
                _cScene = new Scene();
                return;
            }

            _cScene = GameProject.GetInstance().Scenes[assetId];

            Name = _cScene.Name;
            textBox2.Text = _cScene.Name;
            Content.Height = _cScene.Height;
            Content.Width = _cScene.Width;
            numericUpDown1.Value = Content.Width;
            numericUpDown2.Value = Content.Height;

            // background 
            if (_cScene.BackGroundType == Scene.BackGroundTypeEnum.DrawColor)
            {
                r_bc_solidcolor.Select();
                bc_color_pick_value.Text = Functions.ColorToHex(_cScene.BackGroundColor);
                bc_color_box.BackColor = _cScene.BackGroundColor;
            }
            else
            {
                if (File.Exists(ProjectPath + "\\" + _cScene.BackGroundTexture))
                {
                    _bcTexture = Image.FromFile(ProjectPath + "\\" + _cScene.BackGroundTexture);
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
            foreach (var item in GameProject.GetInstance().Instances)
            {
                if (item.Value.Sprite == null)
                {
                    instanceListView.Items.Add(item.Key, 0);
                }
                else
                {
                    Instance_imagelist.Images.Add(Image.FromFile(ProjectPath + item.Value.Sprite.ProjectPath + "\\img\\0.png").GetThumbnailImage(64, 64, null, IntPtr.Zero));
                    instanceListView.Items.Add(item.Key, Instance_imagelist.Images.Count - 1);
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
            if (Functions.ErrorCheck(textBox2.Text.Length > 0, "Scene must have name!")) return;
            _cScene.Name = textBox2.Text;


            if (_aid == null)
            {
                GameProject.GetInstance().Scenes.Add(_cScene.Name, new Scene());
                _aid = _cScene.Name;
            }


            string pathToObjectData = ProjectPath + "\\scene\\" + _cScene.Name;
            if (Directory.Exists(pathToObjectData))
            {
                Directory.Delete(pathToObjectData, true);
            }
            Directory.CreateDirectory(pathToObjectData);
            _cScene.FileName = "\\scene\\" + _cScene.Name + "\\" + _cScene.Name + ".scd";
            _cScene.ProjectPath = "\\scene\\" + _cScene.Name;

            _cScene.SceneInstancesList.Clear();
            foreach (var ins in _cScene.SceneInstances)
            {
                _cScene.SceneInstancesList.Add($"{ins.Instance.Name}|{ins.X}|{ins.Y}");
            }

            using (FileStream createStream = File.Create(ProjectPath + _cScene.FileName))
            {
                byte[] buffer = JsonConvert.SerializeObject(_cScene).Select(c => (byte)c).ToArray();
                createStream.Write(buffer);
            }
            
            if (_aid != _cScene.Name)
            {
                GameProject.GetInstance().Scenes.RenameKey(_aid, _cScene.Name);
                _aid = _cScene.Name;
            }

            GameProject.GetInstance().Scenes[_aid] = (Scene)_cScene.Clone();

            GC.Collect();
            MakeSaved();

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // width change
            _cScene.Width = (int)numericUpDown1.Value;
            Content.Width = _cScene.Width;
            RedrawScene();
            MakeChange();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            // height change
            _cScene.Height = (int)numericUpDown2.Value;
            Content.Height = _cScene.Height;
            RedrawScene();
            MakeChange();
        }




        private void SceneManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult = _saved ? DialogResult.OK : DialogResult.No;
        }

    }
}
