using Newtonsoft.Json;
using System;
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
        public static IntPtr ContentHandle;
        bool ShowGrid = true;
        bool SnapGrid = true;
        string aid = null;
        GameProject.Scene cScene;
        GameProject.Scene.SceneInstance selected_sceneInstance = null;
        //GameProject.Instance shadow_sceneInstance = null;
        Point Grid;
        bool saved = false;
        ImageList BcPreviewList = new ImageList();
        Image BcTexture = null;

        public void MakeChange()
        {
            //if (saved == false) return;
            saved = false;
            this.Text = "SceneManager - \"" + cScene.Name + "\" *";
        }
        public void MakeSaved()
        {
            saved = true;
            this.Text = "SceneManager - \"" + cScene.Name + "\"";
        }

        public SceneManager(string assetId = null)
        {
            aid = assetId;
            InitializeComponent(); Program.ApplyTheme(this);
            BcPreviewList.ImageSize = new Size(128, 128);
            ContentHandle = Content.Handle;
            Content.AllowDrop = true;
            Grid = new Point(32, 32);
            foreach (var item in GameProject.GetInstance().Textures)
            {
                BcPreviewList.Images.Add(Image.FromFile(GameProject.ProjectPath + "\\" + item.Value.ProjectPath + "\\" + item.Value.FileName).GetThumbnailImage(128, 128, null, IntPtr.Zero));
            }
            if (assetId != null)
            {
                string openingFileName = GameProject.ProjectPath + "\\" + GameProject.GetInstance().Scenes[assetId].FileName;
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

                // background 

                if (cScene.BackGroundType == Scene.BackGroundTypeEnum.DrawColor)
                {
                    r_bc_solidcolor.Select();
                    bc_color_pick_value.Text = Functions.ColorToHex(cScene.BackGroundColor);
                    bc_color_box.BackColor = cScene.BackGroundColor;
                }
                else
                {

                    BcTexture = Image.FromFile(GameProject.ProjectPath + "\\" + cScene.BackGroundTexture);
                    r_bc_texture.Select();
                    if (cScene.BackGroundWrapMode == WrapMode.Tile) rb_td_normal.Checked = true;
                    if (cScene.BackGroundWrapMode == WrapMode.TileFlipX) rb_td_w.Checked = true;
                    if (cScene.BackGroundWrapMode == WrapMode.TileFlipY) rb_td_h.Checked = true;
                    if (cScene.BackGroundWrapMode == WrapMode.TileFlipXY) rb_td_w_h.Checked = true;

                    bc_selected_preview.BackgroundImage = BcTexture.GetThumbnailImage(128, 128, null, IntPtr.Zero);

                }

                MakeSaved();

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
            return GameProject.ProjectPath + "\\scene\\" + cScene.Name + "\\triggers";
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
                    Instance_imagelist.Images.Add(Image.FromFile(GameProject.ProjectPath + item.Value.Sprite.ProjectPath + "\\img\\0.png").GetThumbnailImage(64, 64, null, IntPtr.Zero));
                    instanceListView.Items.Add(item.Key, Instance_imagelist.Images.Count - 1);
                }
            }
            RedrawScene();
        }

        void RedrawScene()
        {
            if (Content.Image != null) Content.Image.Dispose();
            GC.Collect();

            Bitmap bmp = new Bitmap(Content.Width, Content.Height);

            // czyszczenie content
            if (cScene.BackGroundType == Scene.BackGroundTypeEnum.DrawColor)
            {
                using (Graphics gr = Graphics.FromImage(bmp))
                {
                    gr.Clear(cScene.BackGroundColor);
                }
            }
            else
            {
                if (BcTexture != null)
                {
                    using (Graphics g = Graphics.FromImage(bmp))
                    {
                        TextureBrush tBrush = new TextureBrush(BcTexture);
                        /*
                         *  Tile = 0,
                            TileFlipX = 1,
                            TileFlipY = 2,
                            TileFlipXY = 3,
                        */

                        tBrush.WrapMode = cScene.BackGroundWrapMode;

                        g.FillRectangle(tBrush, new Rectangle(0, 0, Content.Width, Content.Height));
                    }
                }
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



        void Save()
        {
            if (Functions.ErrorCheck(textBox2.Text.Length > 0, "Scene must have name!")) return;
            cScene.Name = textBox2.Text;


            if (aid == null)
            {
                GameProject.GetInstance().Scenes.Add(cScene.Name, new GameProject.Scene());
                aid = cScene.Name;
            }


            string path_to_object_data = GameProject.ProjectPath + "\\scene\\" + cScene.Name;
            if (Directory.Exists(path_to_object_data))
            {
                Directory.Delete(path_to_object_data, true);
            }
            Directory.CreateDirectory(path_to_object_data);
            cScene.FileName = "\\scene\\" + cScene.Name + "\\" + cScene.Name + ".scd";
            cScene.ProjectPath = "\\scene\\" + cScene.Name;

            using (FileStream createStream = File.Create(GameProject.ProjectPath + cScene.FileName))
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

            if (aid != cScene.Name)
            {
                Functions.RenameKey(GameProject.GetInstance().Scenes, aid, cScene.Name);
                aid = cScene.Name;
            }

            GameProject.GetInstance().Scenes[aid] = (GameProject.Scene)cScene.Clone();

            GC.Collect();
            MakeSaved();

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            // width change
            cScene.Width = (int)numericUpDown1.Value;
            Content.Width = cScene.Width;
            RedrawScene();
            MakeChange();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            // height change
            cScene.Height = (int)numericUpDown2.Value;
            Content.Height = cScene.Height;
            RedrawScene();
            MakeChange();
        }




        private void SceneManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (saved)
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.No;
            }
        }





    }
}
