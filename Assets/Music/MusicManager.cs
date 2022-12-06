using ArtCore_Editor.Assets;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    public partial class MusicManager : Form
    {
        private string _aid;
        private bool _canPlay = false;
        private readonly SoundPlayer _soundPlayer = new SoundPlayer();
        private string _fileName;
        private const string ProjectPath = "\\assets\\music\\";


        string GetDuration()
        {
            return SoundInfo.GetSoundLength(GameProject.ProjectPath + "\\" + textBox2.Text).ToString();
        }
        private void SetInfoBox()
        {
            _soundPlayer.Stop();
            _soundPlayer.SoundLocation = GameProject.ProjectPath + "\\" + textBox2.Text;
            _canPlay = false;
            _soundPlayer.LoadAsync();
            label1.Text = "Duration: " + GetDuration() + " \n" +
                "In project location:\n" + ProjectPath + textBox1.Text;
        }

        private void player_LoadCompleted(object sender,
                AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
                _canPlay = true;
            else
                MessageBox.Show(e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void player_LocationChanged(object sender, EventArgs e)
        {
            _canPlay = true;
        }

        public MusicManager(string assetId = null)
        {
            _soundPlayer.LoadCompleted += new AsyncCompletedEventHandler(player_LoadCompleted);
            _soundPlayer.SoundLocationChanged += new EventHandler(player_LocationChanged);
            InitializeComponent(); Program.ApplyTheme(this);
            _aid = assetId;
            if (assetId != null)
            {
                textBox1.Text = MainWindow.GetInstance().GlobalProject.Music[assetId].Name;
                _fileName = MainWindow.GetInstance().GlobalProject.Music[assetId].FileName;

                textBox2.Text = ProjectPath + "\\" + _fileName;
                if (!File.Exists(GameProject.ProjectPath + "\\" + textBox2.Text))
                {
                    textBox2.Text = "FILE NOT FOUND";
                }
                else
                {
                    //soundPlayer.
                    SetInfoBox();
                }
            }
        }



        private void button3_Click(object sender, EventArgs e)
        {
            // paly
            if (!_canPlay) return;
            try
            {
                _soundPlayer.Play();
            }
            catch (Exception ee)
            {
                MessageBox.Show("ArtEditor can not play this file but in game its all good...\n" + ee.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // TODO pause

        }

        private void button6_Click(object sender, EventArgs e)
        {
            // stop
            _soundPlayer.Stop();
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
                    MainWindow.GetInstance().GlobalProject.Music.Add(textBox1.Text, new Asset());
                }
                MainWindow.GetInstance().GlobalProject.Music[_aid].Name = textBox1.Text;
                MainWindow.GetInstance().GlobalProject.Music[_aid].FileName = _fileName;
                MainWindow.GetInstance().GlobalProject.Music[_aid].ProjectPath = ProjectPath;

                if (_aid != MainWindow.GetInstance().GlobalProject.Music[_aid].Name)
                {
                    MainWindow.GetInstance().GlobalProject.Music.RenameKey(_aid, textBox1.Text);
                }
                if (GameProject.ProjectPath + "\\" + textBox2.Text != GameProject.ProjectPath + "\\assets\\music\\" + _fileName)
                {
                    _soundPlayer.Dispose();
                    File.Copy(GameProject.ProjectPath + "\\" + textBox2.Text, GameProject.ProjectPath + "\\assets\\music\\" + _fileName);
                    File.Delete(GameProject.ProjectPath + "\\" + textBox2.Text);
                    MainWindow.GetInstance().GlobalProject.Textures[_aid].FileName = "\\assets\\music\\" + textBox1.Text + ".png";
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

        private void button1_Click(object sender, EventArgs e)
        {
            // open
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "WAV|*.wav|OGG|*.ogg";
            openFileDialog.Title = "Select file";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string ofile = openFileDialog.FileName;
                if (textBox1.TextLength == 0)
                {
                    textBox1.Text = ofile.Split('\\').Last().Split('.').First();
                    _fileName = textBox1.Text + "." + ofile.Split('\\').Last().Split('.').Last();
                }
                File.Copy(ofile, GameProject.ProjectPath + "\\assets\\music\\" + _fileName, true);
                textBox2.Text = ProjectPath + "\\" + _fileName;

                SetInfoBox();
            }
        }

        private void MusicManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            _soundPlayer.Stop();
            _soundPlayer.Dispose();
        }
    }
}
