using Newtonsoft.Json;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static ArtCore_Editor.GameProject;
using ListViewItem = System.Windows.Forms.ListViewItem;

namespace ArtCore_Editor
{
    public partial class SceneManager : Form
    {
        public static IntPtr ContentHandle;
        bool ShowGrid = true;
        bool SnapGrid = true;
        string aid = null;
        GameProject.Scene cScene;
        GameProject.Scene.SceneInstance selected_sceneInstance = null;
        //GameProject.Instance shadow_sceneInstance = null;
        Point Grid;
        public SceneManager(string assetId = null)
        {
            aid = assetId;
            InitializeComponent();
            ContentHandle = Content.Handle;
            Content.AllowDrop = true;
            Grid = new Point(32, 32);
            if (assetId != null)
            {
                string openingFileName = GameProject.GetInstance().ProjectPath + "\\" + GameProject.GetInstance().Scenes[assetId].FileName;
                if (!File.Exists(openingFileName))
                {
                    MessageBox.Show("File: '" + openingFileName + "' not exists", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string fileContents;
                using (StreamReader reader = new StreamReader(File.Open(openingFileName, FileMode.Open)))
                {
                    fileContents = reader.ReadToEnd();
                }
                cScene = JsonConvert.DeserializeObject<Scene>(fileContents);

                Name = cScene.Name;
                textBox2.Text = cScene.Name;
                Content.Height = cScene.Height;
                Content.Width = cScene.Width;
                numericUpDown1.Value = Content.Width;
                numericUpDown2.Value = Content.Height;
                for (int i = 0; i < cScene.SceneTriggers.Count; i++)
                {
                    // convert path to script -> script
                    cScene.SceneTriggers[i] = System.IO.File.ReadAllText(TriggerPath() + "\\" + cScene.SceneTriggers[i]);
                }
            }
            else
            {
                cScene = new GameProject.Scene();
            }
        }


        string TriggerPath()
        {
            return GameProject.GetInstance().ProjectPath + "\\scene\\" + cScene.Name + "\\triggers";
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
                    Instance_imagelist.Images.Add(item.Value.Sprite.EditorImage);
                    instanceListView.Items.Add(item.Key, Instance_imagelist.Images.Count - 1);
                }
            }
            RedrawScene();
        }

        private void instanceListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            instanceListView.DoDragDrop(e.Item, DragDropEffects.Move);

            //shadow_sceneInstance = GameProject.GetInstance().Instances[((ListViewItem)e.Item).Text];
        }

        private void Content_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListViewItem)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        void RedrawScene()
        {
            if (Content.Image != null) Content.Image.Dispose();
            GC.Collect();

            Bitmap bmp = new Bitmap(Width, Height);


            using (Graphics gr = Graphics.FromImage(bmp))
            {
                gr.Clear(Color.FromKnownColor(KnownColor.ScrollBar));
            }
            using (Graphics g = Graphics.FromImage(bmp))
            {
                if (ShowGrid)
                {
                    for (int x = 0; x < bmp.Width; x += Grid.X)
                    {
                        g.DrawLine(new Pen(Color.LightGray, 1.0f), x, 0, x, bmp.Height);
                    }
                    for (int y = 0; y < bmp.Height; y += Grid.Y)
                    {
                        g.DrawLine(new Pen(Color.LightGray, 1.0f), 0, y, bmp.Width, y);
                    }
                }

                // instances
                foreach (GameProject.Scene.SceneInstance item in cScene.SceneInstances)
                {


                    g.DrawImage(item.img, item.x - (int)item.editorMask, item.y - (int)item.editorMask, item.img.Width, item.img.Height);
                    if (item == selected_sceneInstance)
                    {
                        g.DrawRectangle(new Pen(Color.Red), new Rectangle(item.x - (int)item.editorMask, item.y - (int)item.editorMask, item.img.Width, item.img.Height));

                        StringFormat format = new StringFormat();
                        format.LineAlignment = StringAlignment.Center;
                        format.Alignment = StringAlignment.Center;
                        g.DrawString(item.instance.Name, DefaultFont, new SolidBrush(Color.Red), new Point(item.x, item.y - (int)item.editorMask - 8), format);
                        g.DrawString("X= " + item.x.ToString() + " | Y=" + item.y.ToString(), DefaultFont, new SolidBrush(Color.Red), new Point(item.x, item.y + (int)item.editorMask + 8), format);
                    }

                }

            }
            Content.Image = bmp;

        }

        private void Content_DragDrop(object sender, DragEventArgs e)
        {
            var item = (ListViewItem)e.Data.GetData(typeof(ListViewItem));

            GameProject.Instance t_ins = GameProject.GetInstance().Instances[item.Text];

            Point point = Content.PointToClient(new Point(e.X, e.Y));

            // grid
            if (SnapGrid)
            {
                point.X = (int)(Math.Round((decimal)point.X / Grid.X) * Grid.X);
                point.Y = (int)(Math.Round((decimal)point.Y / Grid.Y) * Grid.Y);
            }

            cScene.SceneInstances.Add(new GameProject.Scene.SceneInstance(point.X, point.Y, t_ins));
            RedrawScene();
        }

        private void Content_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = new Point(e.X, e.Y);
            foreach (var item in cScene.SceneInstances)
            {
                if (Functions.GetDistance(point, new Point(item.x, item.y)) < item.editorMask)
                {
                    selected_sceneInstance = item;
                    RedrawScene();
                    return;
                }
            }
            selected_sceneInstance = null;
            RedrawScene();

        }

        void Save()
        {
            if (Functions.ErrorCheck(textBox2.Text.Length > 0, "Scene must have name!")) return;
            cScene.Name = textBox2.Text;


            if (aid == null)
            {
                GameProject.GetInstance().Scenes.Add(cScene.Name, new GameProject.Scene());
                aid = cScene.Name;
            }


            string path_to_object_data = GameProject.GetInstance().ProjectPath + "\\scene\\" + cScene.Name;
            if (Directory.Exists(path_to_object_data))
            {
                Directory.Delete(path_to_object_data, true);
            }
            Directory.CreateDirectory(path_to_object_data);
            cScene.FileName = "\\scene\\" + cScene.Name + "\\" + cScene.Name + ".scd";
            cScene.ProjectPath = "\\scene\\" + cScene.Name;

            using (FileStream createStream = File.Create(GameProject.GetInstance().ProjectPath + cScene.FileName))
            {
                byte[] buffer = JsonConvert.SerializeObject(cScene).Select(c => (byte)c).ToArray();
                createStream.Write(buffer);
            }

            System.IO.Directory.CreateDirectory(TriggerPath());
            for (int i = 0; i < cScene.SceneTriggers.Count; i++)
            {
                // convert script to file
                System.IO.File.WriteAllText(TriggerPath() + "\\trigger_" + i.ToString() + ".art", cScene.SceneTriggers[i].ToString());
                cScene.SceneTriggers[i] = "trigger_" + i.ToString() + ".art"; ;
            }
            GameProject.GetInstance().Scenes[aid] = (GameProject.Scene)cScene.Clone();
            GC.Collect();

            DialogResult = DialogResult.OK;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // save
            Save();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // width change
            cScene.Width = (int)numericUpDown1.Value;
            Content.Width = cScene.Width;
            RedrawScene();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            // height change
            cScene.Height = (int)numericUpDown2.Value;
            Content.Height = cScene.Height;
            RedrawScene();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowGrid = !ShowGrid;
            showToolStripMenuItem.Checked = ShowGrid;
            RedrawScene();
        }

        private void dimensionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridControl.GetGridDimensions(ref Grid);
            RedrawScene();
        }

        private void snapToGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SnapGrid = !SnapGrid;
            snapToGridToolStripMenuItem.Checked = SnapGrid;
        }

        private void gUIEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process process = new Process();
            process.StartInfo.FileName = "..\\Core\\gui-bulider\\gui-builder.exe";
            bool fex = File.Exists(GameProject.GetInstance().ProjectPath + cScene.ProjectPath + "\\gui.txt");
            process.StartInfo.Arguments = (fex ? "" : "-new ") + GameProject.GetInstance().ProjectPath + cScene.ProjectPath + "\\gui.txt";
            process.StartInfo.UseShellExecute = false;


            process.Start();
            process.WaitForExit();
        }
    }
}
