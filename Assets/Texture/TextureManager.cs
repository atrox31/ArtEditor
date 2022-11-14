using ArtCore_Editor.Assets;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    public partial class TextureManager : Form
    {
        string aid = null;
        private void SetInfoBox()
        {
            label1.Text = "Width: " + pictureBox1.Image.Width.ToString() + "px\n" +
                "Height: " + pictureBox1.Image.Height.ToString() + "px\n" +
                "In project location:\n" + "assets/textures/" + textBox1.Text;
        }

        string ProjectPath;
        string FileName;

        public TextureManager(string AssetId = null)
        {
            InitializeComponent(); Program.ApplyTheme(this);
            aid = AssetId;
            if (AssetId != null)
            {

                ProjectPath = MainWindow.GetInstance().Game_Project.Textures[AssetId].ProjectPath;
                FileName = MainWindow.GetInstance().Game_Project.Textures[AssetId].FileName;

                textBox1.Text = MainWindow.GetInstance().Game_Project.Textures[AssetId].Name;
                textBox2.Text = ProjectPath + FileName;

                if (!File.Exists(GameProject.ProjectPath + "\\" + textBox2.Text))
                {
                    textBox2.Text = "FILE NOT FOUND";
                }
                else
                {
                    pictureBox1.Image = Image.FromFile(GameProject.ProjectPath + "\\" + textBox2.Text);
                    SetInfoBox();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        { // add from file
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "PNG|*.png";
            openFileDialog.Title = "Select texture";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string ofile = openFileDialog.FileName;
                if (textBox1.TextLength == 0)
                {
                    textBox1.Text = ofile.Split('\\').Last().Split('.').First();
                }
                pictureBox1.Image?.Dispose();
                File.Copy(ofile, GameProject.ProjectPath + "\\assets\\texture\\" + textBox1.Text + ".png", true);

                ProjectPath = "assets\\texture\\";
                FileName = textBox1.Text + ".png";

                textBox2.Text = ProjectPath + FileName;
                pictureBox1.Image = Image.FromFile(GameProject.ProjectPath + "\\" + textBox2.Text);
                SetInfoBox();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {// apply
            if ((textBox1.Text.Length > 0)
                && (textBox2.Text.Length > 0)
                && pictureBox1.Image != null
                && File.Exists(GameProject.ProjectPath + "\\" + textBox2.Text))
            {
                if (aid == null)
                {
                    // add new
                    aid = textBox1.Text;
                    MainWindow.GetInstance().Game_Project.Textures.Add(textBox1.Text, new Asset());
                }
                MainWindow.GetInstance().Game_Project.Textures[aid].Name = textBox1.Text;
                MainWindow.GetInstance().Game_Project.Textures[aid].FileName = FileName;
                MainWindow.GetInstance().Game_Project.Textures[aid].ProjectPath = ProjectPath;
                MainWindow.GetInstance().Game_Project.Textures[aid].EditorImage = Functions.ResizeImage(pictureBox1.Image, 32, 32);

                if (aid != MainWindow.GetInstance().Game_Project.Textures[aid].Name)
                {
                    Functions.RenameKey(MainWindow.GetInstance().Game_Project.Textures, aid, textBox1.Text);
                }

                DialogResult = DialogResult.OK;
                Close();

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {// view in full
            if (pictureBox1.Image != null)
            {
                Form _temp = new Form();
                _temp.Size = pictureBox1.Image.Size;
                _temp.BackgroundImage = pictureBox1.Image;
                _temp.Text = textBox1.Text;
                _temp.ShowDialog();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
