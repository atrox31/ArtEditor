using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ArtCore_Editor.Assets.Texture
{
    public partial class TextureManager : Form
    {
        string _aid = null;
        private void SetInfoBox()
        {
            label1.Text = "Width: " + pictureBox1.Image.Width.ToString() + "px\n" +
                "Height: " + pictureBox1.Image.Height.ToString() + "px\n" +
                "In project location:\n" + "assets/textures/" + textBox1.Text;
        }
        private const string ProjectPath = "assets\\textures";
        string _projectPath;
        string _fileName;

        public TextureManager(string assetId = null)
        {
            InitializeComponent(); Program.ApplyTheme(this);
            _aid = assetId;
            if (assetId != null)
            {

                _projectPath = MainWindow.GetInstance().GlobalProject.Textures[assetId].ProjectPath;
                _fileName = MainWindow.GetInstance().GlobalProject.Textures[assetId].FileName;

                textBox1.Text = MainWindow.GetInstance().GlobalProject.Textures[assetId].Name;
                textBox2.Text = _projectPath + _fileName;

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
                File.Copy(ofile, GameProject.ProjectPath + ProjectPath + "\\" + textBox1.Text + ".png", true);

                _projectPath = ProjectPath + "\\";
                _fileName = textBox1.Text + ".png";

                textBox2.Text = _projectPath + _fileName;
                pictureBox1.Image?.Dispose();
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
                if (_aid == null)
                {
                    // add new
                    _aid = textBox1.Text;
                    MainWindow.GetInstance().GlobalProject.Textures.Add(textBox1.Text, new Asset());
                }
                MainWindow.GetInstance().GlobalProject.Textures[_aid].Name = textBox1.Text;
                MainWindow.GetInstance().GlobalProject.Textures[_aid].FileName = _fileName;
                MainWindow.GetInstance().GlobalProject.Textures[_aid].ProjectPath = _projectPath;
               
                if (_aid != MainWindow.GetInstance().GlobalProject.Textures[_aid].Name)
                {
                    MainWindow.GetInstance().GlobalProject.Textures.RenameKey(_aid, textBox1.Text);
                }

                if (GameProject.ProjectPath + "\\" + textBox2.Text != GameProject.ProjectPath + ProjectPath + "\\" + textBox1.Text + ".png")
                {
                    pictureBox1.Image.Dispose();
                    pictureBox1.Image = null;
                    File.Copy(GameProject.ProjectPath + "\\" + textBox2.Text, GameProject.ProjectPath + ProjectPath + "\\" + textBox1.Text + ".png");
                    File.Delete(GameProject.ProjectPath + "\\" + textBox2.Text);
                    MainWindow.GetInstance().GlobalProject.Textures[_aid].FileName = ProjectPath + "\\" + textBox1.Text + ".png";
                }
                DialogResult = DialogResult.OK;
                Close();

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {// view in full
            if (pictureBox1.Image != null)
            {
                Form temp = new Form();
                temp.Size = pictureBox1.Image.Size;
                temp.BackgroundImage = pictureBox1.Image;
                temp.Text = textBox1.Text;
                temp.ShowDialog();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
