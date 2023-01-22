using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ArtCore_Editor.AdvancedAssets.Instance_Manager;
using ArtCore_Editor.AdvancedAssets.SceneManager.GuiEditor;
using ArtCore_Editor.AdvancedAssets.SceneManager.LevelEditor;
using ArtCore_Editor.Assets;
using ArtCore_Editor.Functions;
using ArtCore_Editor.Main;
using ArtCore_Editor.Pick_forms;

using Newtonsoft.Json;

using static ArtCore_Editor.Main.GameProject;

namespace ArtCore_Editor.AdvancedAssets.SceneManager
{
    public partial class SceneManager : Form
    {
        private bool _showGrid = true;
        private bool _snapGrid = true;
        private string _aid;
        private readonly Scene _cScene;
        private Point _grid;
        private bool _saved;
        private readonly ImageList _bcPreviewList = new ImageList();
        private Image _bcTexture;

        public class SceneInstance
        {
            [JsonIgnore] private static readonly Dictionary<string, Image> InstanceSprites = new Dictionary<string, Image>();
            public int X { get; set; }
            public int Y { get; set; }
            public float EditorMask { get; set; }
            //TODO change Instance type to string type, for better saving system
            public Instance Instance { get; set; }
            public readonly Image Img;
            public override string ToString()
            {
                return $"{Instance.Name}|{X}|{Y}";
            }

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
                            image ?? Properties.Resources.interrogation);
                    } 
                    Img = InstanceSprites[instance.Sprite.FileName];
                }

                EditorMask = ((float)(Img.Width + Img.Height) / 4);
            }
        }

        private SceneInstance _selectedSceneInstance;

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
            foreach (Asset item in GameProject.GetInstance().Textures.Values.Where(item => File.Exists(item.GetFilePath())))
            {
                _bcPreviewList.Images.Add(
                    Image.FromFile(item.GetFilePath()).GetThumbnailImage(128, 128, null, IntPtr.Zero)
                    );
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
                    switch (_cScene.BackGroundWrapMode)
                    {
                        case WrapMode.Tile:
                            rb_td_normal.Checked = true;
                            break;
                        case WrapMode.TileFlipX:
                            rb_td_w.Checked = true;
                            break;
                        case WrapMode.TileFlipY:
                            rb_td_h.Checked = true;
                            break;
                        case WrapMode.TileFlipXY:
                            rb_td_w_h.Checked = true;
                            break;
                    }

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
                // cheek if can be placed in scene
                if(!item.Value.EditorShowInScene) continue;

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

            string pathToObjectData = ProjectPath + "\\scene\\" + _cScene.Name;
            _cScene.FileName = "\\scene\\" + _cScene.Name + "\\" + _cScene.Name + Program.FileExtensions_SceneObject;
            _cScene.ProjectPath = "\\scene\\" + _cScene.Name;

            _cScene.SceneInstancesList.Clear();
            foreach (SceneInstance ins in _cScene.SceneInstances)
            {
                _cScene.SceneInstancesList.Add($"{ins.Instance.Name}|{ins.X}|{ins.Y}");
            }

            Directory.CreateDirectory(ProjectPath + _cScene.ProjectPath);
            if (_aid != _cScene.Name)
            {
                if (GameProject.GetInstance().Scenes.ContainsKey(_cScene.Name))
                {
                    MessageBox.Show("Scene with target name exists! Can not override, first delete other scene.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                GameProject.GetInstance().Scenes.RenameKey(_aid, _cScene.Name);
                foreach (string enumerateFile in Directory.EnumerateFiles(ProjectPath + "\\scene\\" + _aid, "*" + Program.FileExtensions_ArtCode))
                {// move all scene triggers
                    File.Move(enumerateFile, 
                        pathToObjectData + "\\" + Path.GetFileName(enumerateFile)
                        );
                }

                if (File.Exists(ProjectPath + "\\scene\\" + _aid + "\\GuiSchema.json"))
                { // move gui schema
                    File.Move(ProjectPath + "\\scene\\" + _aid + "\\GuiSchema.json", ProjectPath + "\\scene\\" + _cScene.Name + "\\GuiSchema.json");
                }
                
                if (Directory.Exists(ProjectPath + "\\scene\\" + _aid))
                { // delete old folder
                    Directory.Delete(ProjectPath + "\\scene\\" + _aid, true);
                }

                _aid = _cScene.Name;
            }

            using (FileStream createStream = File.Create(ProjectPath + _cScene.FileName))
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
                DialogResult = DialogResult.OK;
                return;
            }
            switch (MessageBox.Show("You made some changes, save?", "Save variables?",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    Save();
                    DialogResult = DialogResult.OK;
                    break;
                case DialogResult.No:
                    DialogResult = DialogResult.No;
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
            }
            
        }

        #region navigation bar
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _showGrid = !_showGrid;
            showToolStripMenuItem.Checked = _showGrid;
            RedrawScene();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void dimensionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridControl.GetGridDimensions(ref _grid);
            RedrawScene();
        }

        private void snapToGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _snapGrid = !_snapGrid;
            snapToGridToolStripMenuItem.Checked = _snapGrid;
        }

        private void editSchemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuiEditor.GuiEditor guiEditor = new GuiEditor.GuiEditor(_cScene.Name);
            guiEditor.ShowDialog();
        }

        private void guiTriggersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string guiSchemaFile =
                ProjectPath + "\\" + "scene" + "\\" + _cScene.Name + "\\" + "GuiSchema.json";
            if (!File.Exists(guiSchemaFile)) return;

            // copy schema
            GuiElement rootElement =
                JsonConvert.DeserializeObject<GuiElement>(File.ReadAllText(guiSchemaFile));
            if (rootElement == null) return;

            // must assign parents
            rootElement.SetAllParents();

            TriggerEditor.TriggerEditor editor = new TriggerEditor.TriggerEditor(_cScene, rootElement);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                // no action
            }
        }
        private void editTriggersToolStripMenuItem_Click(object sender, EventArgs e)
        {

            TriggerEditor.TriggerEditor editor = new TriggerEditor.TriggerEditor(_cScene, null);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                // no action
            }
        }


        private void setStartupTriggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _cScene.SceneStartingTrigger =
                PicFromList.Get(Directory.GetFiles(ProjectPath + _cScene.ProjectPath, "scene&*" + Program.FileExtensions_ArtCode)
                    .Select(Path.GetFileNameWithoutExtension).ToList());

        }
        private void sceneVariablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VariablesEditor variablesEditor = new VariablesEditor(_cScene);
            if (variablesEditor.ShowDialog() == DialogResult.OK)
            {
                MakeChange();
            }
        }
        #endregion

        #region content (scene renderer)
        private bool _mouseDrag;
        SceneInstance GetInstance(Point point, bool first)
        {
            SceneInstance selected = null;
            foreach (SceneInstance item in _cScene.SceneInstances)
            {
                if (Functions.Functions.GetDistance(point, new Point(item.X, item.Y)) < item.EditorMask)
                {
                    selected = item;
                    if (first)
                    {
                        return selected;
                    }
                }
            }
            return selected;
        }
        private void Content_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_mouseDrag) return;
            if (_selectedSceneInstance == null) return;
            if (e.Button != MouseButtons.Left) return;
            Point point = new Point(e.X, e.Y);

            if (_selectedSceneInstance != GetInstance(point, false))
            {
                _selectedSceneInstance = null;
                _mouseDrag = false;
                return;
            }

            if (_snapGrid)
            {
                point.X = (int)(Math.Round((decimal)point.X / _grid.X) * _grid.X);
                point.Y = (int)(Math.Round((decimal)point.Y / _grid.Y) * _grid.Y);
            }

            point.X = (point.X < 0 ? 0 : (point.X > _cScene.ViewWidth ? _cScene.ViewWidth : point.X));
            point.Y = (point.Y < 0 ? 0 : (point.Y > _cScene.ViewHeight ? _cScene.ViewHeight : point.Y));

            _selectedSceneInstance.X = point.X;
            _selectedSceneInstance.Y = point.Y;
            RedrawScene();
        }

        private void Content_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            if (e.Button == MouseButtons.Left)
            {
                if (_selectedSceneInstance != GetInstance(point, false))
                {
                    _selectedSceneInstance = null;
                }
                _selectedSceneInstance = GetInstance(point, false);
                _mouseDrag = _selectedSceneInstance != null;
                RedrawScene();
            }

            if (e.Button != MouseButtons.Right) return;
            if (_selectedSceneInstance == null) return;

            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            contextMenuStrip.Items.Add("Delete");
            contextMenuStrip.ItemClicked += (_,_) =>
            {
                if (MessageBox.Show(
                        "Delete instance '" + _selectedSceneInstance.Instance.Name + "'?", "Delete",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
                _cScene.SceneInstances.Remove(_selectedSceneInstance);
                RedrawScene();
            };
            contextMenuStrip.Show(Cursor.Position);
        }
        private void instanceListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Item != null) instanceListView.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void Content_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data != null && e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void Content_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data != null)
            {
                ListViewItem selectedItem = (ListViewItem)e.Data.GetData(typeof(ListViewItem));
                if (selectedItem == null) return;

                Instance draggedObject = GameProject.GetInstance().Instances[selectedItem.Text];
                if (draggedObject == null) return;

                Point point = Content.PointToClient(new Point(e.X, e.Y));
                if (_snapGrid)
                {
                    point.X = (int)(Math.Round((decimal)point.X / _grid.X) * _grid.X);
                    point.Y = (int)(Math.Round((decimal)point.Y / _grid.Y) * _grid.Y);
                }

                _cScene.SceneInstances.Add(new SceneInstance(point.X, point.Y, draggedObject));
            }

            _saved = false;
            RedrawScene();
        }

        #endregion

        #region scene background
        private void button4_Click(object sender, EventArgs e)
        {
            PicFromList picFromList = new PicFromList(GameProject.GetInstance().Textures.Keys.ToList());
            if (picFromList.ShowDialog() == DialogResult.OK)
            {
                bc_selected_preview.BackgroundImage = _bcPreviewList.Images[picFromList.SelectedIndex];
                _cScene.BackGroundTextureName = picFromList.Selected;
            }

        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (_cScene.BackGroundTextureName == null) return;
            r_bc_texture.Select();

            _bcTexture?.Dispose();
            GC.Collect();

            _cScene.BackGroundType = Scene.BackGroundTypeEnum.DrawTexture;

            _cScene.BackGroundTexture = GameProject.GetInstance().Textures[_cScene.BackGroundTextureName];
            _bcTexture = Image.FromFile(ProjectPath + "\\" + GameProject.GetInstance().Textures[_cScene.BackGroundTextureName].ProjectPath + "\\" + GameProject.GetInstance().Textures[_cScene.BackGroundTextureName].FileName);

            if (rb_td_normal.Checked) _cScene.BackGroundWrapMode = WrapMode.Tile;
            if (rb_td_w.Checked) _cScene.BackGroundWrapMode = WrapMode.TileFlipX;
            if (rb_td_h.Checked) _cScene.BackGroundWrapMode = WrapMode.TileFlipY;
            if (rb_td_w_h.Checked) _cScene.BackGroundWrapMode = WrapMode.TileFlipXY;

            MakeChange();
            RedrawScene();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                bc_color_pick_value.Text = colorDialog.Color.ColorToHex();
                bc_color_box.BackColor = colorDialog.Color;
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            r_bc_solidcolor.Select();
            if (!bc_color_pick_value.Text.StartsWith('#'))
            {
                bc_color_pick_value.Text = bc_color_pick_value.Text.Insert(0, "#");
            }

            if (Functions.Functions.ErrorCheck(bc_color_pick_value.Text.Length > 0,
                    "Hex color value is empty")) return;
            if (Functions.Functions.ErrorCheck(bc_color_pick_value.Text.IsHex(),
                    "Hex color value is invalid")) return;

            _cScene.BackGroundColor = bc_color_pick_value.Text.HexToColor();
            _cScene.BackGroundType = Scene.BackGroundTypeEnum.DrawColor;
            RedrawScene();
            MakeChange();

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            MakeChange();
        }

        private void bc_color_pick_value_TextChanged(object sender, EventArgs e)
        {
            if (bc_color_pick_value.Text.Length == 6
            && !bc_color_pick_value.Text.StartsWith('#'))
            {
                bc_color_pick_value.Text = bc_color_pick_value.Text.Insert(0, "#");
            }

            if (bc_color_pick_value.Text.Length != 7) return;
            Color tmp = bc_color_pick_value.Text.HexToColor();
            if (tmp.IsEmpty) return;
            bc_color_box.BackColor = tmp;
            MakeChange();
        }
        private void r_bc_texture_CheckedChanged(object sender, EventArgs e)
        {
            gb_bc_color_pic.Enabled = false;
            gb_bc_tex.Enabled = true;
        }

        private void r_bc_solidcolor_CheckedChanged(object sender, EventArgs e)
        {
            gb_bc_tex.Enabled = false;
            gb_bc_color_pic.Enabled = true;
        }
        #endregion

        private void createToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // i delete this option later, too many the same code
            LevelEditorMain levelEditor = new LevelEditorMain(_cScene, "level", _snapGrid ? _grid : new Point(-1,-1), StringExtensions.Combine(GameProject.ProjectPath, _cScene.ProjectPath, "levels"));
            if (levelEditor.ShowDialog() == DialogResult.OK)
            {
                MakeChange();
            }
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LevelListSelect levelList = new LevelListSelect(_cScene, "level", _snapGrid ? _grid : new Point(-1, -1));
            if (levelList.ShowDialog() == DialogResult.OK)
            {
                MakeChange();
            }
        }
    }
}
