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
        string aid;
        Font font;


        string FileName;
        string ProjectPath = "assets\\font\\";

        public FontManager(string AssetId = null)
        {
            InitializeComponent(); Program.ApplyTheme(this);
            aid = AssetId;
            if (AssetId != null)
            {
                textBox1.Text = MainWindow.GetInstance().Game_Project.Fonts[AssetId].Name;
                FileName = MainWindow.GetInstance().Game_Project.Fonts[AssetId].FileName;

                textBox2.Text = ProjectPath + "\\" + FileName;
                if (!File.Exists(GameProject.ProjectPath + "\\" + textBox2.Text))
                {
                    textBox2.Text = "FILE NOT FOUND";
                }
                else
                {
                    PrivateFontCollection collection = new PrivateFontCollection();
                    collection.AddFontFile(GameProject.ProjectPath + "\\" + textBox2.Text);
                    FontFamily fontFamily = new FontFamily(collection.Families[0].Name, collection);
                    font = new Font(fontFamily, 8);
                    richTextBox1.Font = font;
                }
            }
            else
            {
                font = new Font("Arial", 8);
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
                File.Copy(ofile, GameProject.ProjectPath + "\\assets\\font\\" + textBox1.Text + ".ttf", true);
                textBox2.Text = "assets\\font\\" + textBox1.Text + ".ttf";

                PrivateFontCollection collection = new PrivateFontCollection();
                collection.AddFontFile(GameProject.ProjectPath + "\\assets\\font\\" + textBox1.Text + ".ttf");
                FontFamily fontFamily = new FontFamily(collection.Families[0].Name, collection);
                font = new Font(fontFamily, 8);
                richTextBox1.Font = font;
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
                if (aid == null)
                {
                    // add new
                    aid = textBox1.Text;
                    MainWindow.GetInstance().Game_Project.Fonts.Add(textBox1.Text, new Asset());
                }
                MainWindow.GetInstance().Game_Project.Fonts[aid].Name = textBox1.Text;
                MainWindow.GetInstance().Game_Project.Fonts[aid].FileName = FileName;
                MainWindow.GetInstance().Game_Project.Fonts[aid].ProjectPath = ProjectPath;

                if (aid != MainWindow.GetInstance().Game_Project.Fonts[aid].Name)
                {
                    Functions.RenameKey(MainWindow.GetInstance().Game_Project.Fonts, aid, textBox1.Text);
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
