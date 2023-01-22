#pragma warning disable IDE0017 // for using abc obj = new abc()
using ArtCore_Editor.Functions;
using ArtCore_Editor.Main;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static ArtCore_Editor.AdvancedAssets.SceneManager.SceneManager;

using MessageBox = System.Windows.Forms.MessageBox;
using Point = System.Drawing.Point;

namespace ArtCore_Editor.AdvancedAssets.SceneManager.LevelEditor
{
    public partial class LevelEditorMain : Form
    {
        private readonly LevelEditorTools _toolWindow;    // toll window
        private bool _saved;                    // any changes flag
        private readonly string _levelPath;     // path to data file
        private readonly string _name;     

        private readonly Point _snapGrid;      // snapping

        private readonly List<SceneInstance> _instances;
        private SceneInstance _selectedSceneInstance;
        private readonly Rectangle _sceneDimensions;
        public LevelEditorMain(Scene scene, string name, Point snapGrid, string levelPath)
        {
            InitializeComponent();Program.ApplyTheme(this);
            _saved = true;
            _selectedSceneInstance = null;
            _instances = new List<SceneInstance>();
            _snapGrid = snapGrid;
            _sceneDimensions = new Rectangle(0,0,scene.ViewWidth,scene.ViewHeight);
            this.Size = new System.Drawing.Size(scene.ViewWidth + 16, scene.ViewHeight + 16);
            
            if (name.HaveExtension())
            {
                _name = name.WithoutExtension();
            }
            else
            {
                int levelCount = Directory.GetFiles(levelPath).Count(fame => fame.WithoutExtension().StartsWith(name));
                _name = name + (levelCount > 0 ? levelCount.ToString() : "");
            }

            _levelPath = StringExtensions.Combine(levelPath, _name + Program.FileExtensions_SceneLevel);

            // this load triggers too
            _toolWindow = new LevelEditorTools(scene, _levelPath);
            _toolWindow.Location = new Point(this.Left + 16, this.Top + 32);
        }

        public string GetName()
        {
            return _name;
        }

        public Image GetPreview(int width, int height)
        {
            LoadInstanceList();
            Rectangle rect1 = Content.ClientRectangle;
            using Bitmap bmp = new Bitmap(rect1.Width, rect1.Height);
            Content.DrawToBitmap(bmp, rect1);
            return bmp.GetThumbnailImage(width, height, null, IntPtr.Zero);
        }

        private void LoadInstanceList()
        {
            List<string> instances = ZipIO.ReadFromZip(
                _levelPath,
                "instances.txt"
            ).Split('\n').ToList();

            foreach (string[] arguments in instances.
                         Select(instance => instance.Split('|')).
                         Where(arguments => arguments.Length == 3
                                            && GameProject.GetInstance().Instances.ContainsKey(arguments[0])))
            {
                _instances.Add(new SceneInstance(
                        Convert.ToInt32(arguments[1]),
                        Convert.ToInt32(arguments[2]),
                        GameProject.GetInstance().Instances[arguments[0]]
                    )
                );
            }
        }

        private void LevelEditor_main_Load(object sender, EventArgs e)
        {
            LoadInstanceList();
            _toolWindow.Show();
        }
        private void Save()
        {
            Functions.Functions.FileDelete( _levelPath );

            _toolWindow.SaveTriggers();

            ZipIO.WriteListToArchive(
                _levelPath,
                "instances.txt",
                _instances.Select(sceneInstance => sceneInstance.ToString()).ToList(),
                true
                );
        }

        private void LevelEditor_main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_saved)
            {
                this.DialogResult = DialogResult.OK;
                _toolWindow.Close();
                return;
            }
            switch (MessageBox.Show("You made some changes, save?", "Save?",
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
            _toolWindow.Close();
        }
        SceneInstance GetInstance(Point point, bool first)
        {
            SceneInstance selected = null;
            foreach (SceneInstance item in _instances.Where(
                         item => Functions.Functions.GetDistance(point, new Point(item.X, item.Y)) < item.EditorMask))
            {
                selected = item;
                if (first)
                {
                    return selected;
                }
            }
            return selected;
        }

        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            if (e.Button == MouseButtons.Right)
            {
                if(_selectedSceneInstance != null && GetInstance(point, false) == _selectedSceneInstance)
                {
                    _instances.Remove(_selectedSceneInstance);
                    _saved = false;
                    return;
                }
                _selectedSceneInstance = null;
                _toolWindow.SelectedInstance = null;
            }
            if (e.Button != MouseButtons.Left) return;

            SceneInstance clickedInstance = GetInstance(point, false);

            _selectedSceneInstance = null;

            if (_toolWindow.SelectedInstance == null)
            {
                _selectedSceneInstance = clickedInstance;
            }
            else
            {
                if (clickedInstance != null) return;
                SceneInstance draggedObject = _toolWindow.SelectedInstance;
                if (draggedObject == null) return;

                Point pointInContent = Content.PointToClient(MousePosition);
                if (!_sceneDimensions.Contains(pointInContent)) return;
                if (_snapGrid.X != -1) // this is null
                {
                    pointInContent.X = (int)(Math.Round((decimal)pointInContent.X / _snapGrid.X) * _snapGrid.X);
                    pointInContent.Y = (int)(Math.Round((decimal)pointInContent.Y / _snapGrid.Y) * _snapGrid.Y);
                }

                _instances.Add(new SceneInstance(pointInContent.X, pointInContent.Y, draggedObject.Instance));
                _saved = false;
            }
        }

        private void PictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            foreach (SceneInstance item in _instances)
            {
                g.DrawImage(item.Img, item.X - (int)item.EditorMask, item.Y - (int)item.EditorMask, item.Img.Width, item.Img.Height);
            }

            using Pen redPen = new Pen(Color.Red);
            if(_selectedSceneInstance != null)
            {
                g.DrawRectangle(redPen,
                    new Rectangle(
                        _selectedSceneInstance.X - (int)_selectedSceneInstance.EditorMask,
                        _selectedSceneInstance.Y - (int)_selectedSceneInstance.EditorMask,
                        _selectedSceneInstance.Img.Width,
                        _selectedSceneInstance.Img.Height));

                StringFormat format = new StringFormat();
                format.LineAlignment = StringAlignment.Center;
                format.Alignment = StringAlignment.Center;
                g.DrawString(_selectedSceneInstance.Instance.Name,
                    DefaultFont, new SolidBrush(Color.Red),
                    new Point(
                        _selectedSceneInstance.X,
                        _selectedSceneInstance.Y - (int)_selectedSceneInstance.EditorMask - 8),
                    format);
                using SolidBrush redSolidBrush = new SolidBrush(Color.Red);
                g.DrawString(
                    "X= " + _selectedSceneInstance.X.ToString() + " | Y=" + _selectedSceneInstance.Y.ToString(),
                    DefaultFont, redSolidBrush,
                    new Point(_selectedSceneInstance.X,
                    _selectedSceneInstance.Y + (int)_selectedSceneInstance.EditorMask + 8),
                    format);

            }

            if (_selectedSceneInstance == null && _toolWindow.SelectedInstance != null)
            {
                Point pointInContent = Content.PointToClient(MousePosition);
                if (_sceneDimensions.Contains(pointInContent))
                {
                    if (_snapGrid.X != -1) // this is null
                    {
                        pointInContent.X = (int)(Math.Round((decimal)pointInContent.X / _snapGrid.X) * _snapGrid.X);
                        pointInContent.Y = (int)(Math.Round((decimal)pointInContent.Y / _snapGrid.Y) * _snapGrid.Y);
                    }
                    g.DrawImage(_toolWindow.SelectedInstance.Img.SetOpacity(0.5f),
                        pointInContent.X - (int)_toolWindow.SelectedInstance.EditorMask,
                        pointInContent.Y - (int)_toolWindow.SelectedInstance.EditorMask,
                        _toolWindow.SelectedInstance.Img.Width, _toolWindow.SelectedInstance.Img.Height);
                }
            }

            g.DrawRectangle(redPen, _sceneDimensions);

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

            Content.Refresh();
        }
    }
}
