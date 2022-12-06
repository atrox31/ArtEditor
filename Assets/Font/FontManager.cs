using ArtCore_Editor.Assets;
using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    public partial class FontManager : Form
    {
        private string _aid;
        private Font _font;

        private string _fileName;
        private const string ProjectPath = "\\assets\\fonts\\";

        public FontManager(string assetId = null)
        {
            InitializeComponent(); Program.ApplyTheme(this);
            _aid = assetId;
            if (assetId != null)
            {
                textBox1.Text = MainWindow.GetInstance().GlobalProject.Fonts[assetId].Name;
                _fileName = MainWindow.GetInstance().GlobalProject.Fonts[assetId].FileName;

                textBox2.Text = ProjectPath + "\\" + _fileName;
                if (!File.Exists(GameProject.ProjectPath + "\\" + textBox2.Text))
                {
                    textBox2.Text = "FILE NOT FOUND";
                }
                else
                {
                    PrivateFontCollection collection = new PrivateFontCollection();
                    collection.AddFontFile(GameProject.ProjectPath + "\\" + textBox2.Text);
                    FontFamily fontFamily = new FontFamily(collection.Families[0].Name, collection);
                    _font = new Font(fontFamily, 8);
                    richTextBox1.Font = _font;
                }
            }
            else
            {
                _font = new Font("Arial", 8);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // laod
            // open
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "TTF|*.ttf";
            openFileDialog.Title = "Select file";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string ofile = openFileDialog.FileName;
                if (textBox1.TextLength == 0)
                {
                    textBox1.Text = ofile.Split('\\').Last().Split('.').First();
                }
                File.Copy(ofile, GameProject.ProjectPath + ProjectPath + "\\" + textBox1.Text + ".ttf", true);
                textBox2.Text = ProjectPath + textBox1.Text + ".ttf";
                _fileName = ofile.Split('\\').Last();

                PrivateFontCollection collection = new PrivateFontCollection();
                collection.AddFontFile(GameProject.ProjectPath + ProjectPath + "\\" + textBox1.Text + ".ttf");
                FontFamily fontFamily = new FontFamily(collection.Families[0].Name, collection);
                _font = new Font(fontFamily, 8);
                richTextBox1.Font = _font;
                button1.Enabled = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // apply
            if ((textBox1.Text.Length > 0)
                && (textBox2.Text.Length > 0)
                && File.Exists(GameProject.ProjectPath + "\\" + textBox2.Text))
            {
                if (_aid == null)
                {
                    // add new
                    _aid = textBox1.Text;
                    MainWindow.GetInstance().GlobalProject.Fonts.Add(textBox1.Text, new Asset());
                }
                MainWindow.GetInstance().GlobalProject.Fonts[_aid].Name = textBox1.Text;
                MainWindow.GetInstance().GlobalProject.Fonts[_aid].FileName = _fileName;
                MainWindow.GetInstance().GlobalProject.Fonts[_aid].ProjectPath = ProjectPath;

                if (_aid != MainWindow.GetInstance().GlobalProject.Fonts[_aid].Name)
                {
                    MainWindow.GetInstance().GlobalProject.Fonts.RenameKey(_aid, textBox1.Text);
                }
                if (GameProject.ProjectPath + "\\" + textBox2.Text != GameProject.ProjectPath + ProjectPath + "\\" + textBox1.Text + ".ttf")
                {
                    _font.Dispose();
                    File.Copy(GameProject.ProjectPath + "\\" + textBox2.Text, GameProject.ProjectPath + ProjectPath + "\\" + textBox1.Text + ".ttf");
                    File.Delete(GameProject.ProjectPath + "\\" + textBox2.Text);
                    MainWindow.GetInstance().GlobalProject.Textures[_aid].FileName = ProjectPath + "\\" + textBox1.Text + ".ttf";
                }

                DialogResult = DialogResult.OK;
                Close();

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // cancel
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
