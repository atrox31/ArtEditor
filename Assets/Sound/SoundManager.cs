using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace ArtCore_Editor.Assets.Sound
{
    public partial class SoundManager : Form
    {
        private string _aid;
        private bool _canPlay = false;
        readonly SoundPlayer _soundPlayer = new SoundPlayer();
        private string _fileName;
        private const string ProjectPath = "assets\\sound\\";

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
                "In project location:\n" + "assets/music/" + textBox1.Text;
        }


        private void player_LoadCompleted(object sender,
                AsyncCompletedEventArgs e)
        {
            _canPlay = true;
        }
        private void player_LocationChanged(object sender, EventArgs e)
        {
            _canPlay = true;
        }


        public SoundManager(string assetId = null)
        {
            _soundPlayer.LoadCompleted += new AsyncCompletedEventHandler(player_LoadCompleted);
            _soundPlayer.SoundLocationChanged += new EventHandler(player_LocationChanged);
            InitializeComponent(); Program.ApplyTheme(this);
            _aid = assetId;
            if (assetId != null)
            {
                textBox1.Text = MainWindow.GetInstance().GlobalProject.Sounds[assetId].Name;
                _fileName = MainWindow.GetInstance().GlobalProject.Sounds[assetId].FileName;

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
            // play
            if (_canPlay)
                _soundPlayer.Play();
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
                    MainWindow.GetInstance().GlobalProject.Sounds.Add(textBox1.Text, new Asset());
                }
                MainWindow.GetInstance().GlobalProject.Sounds[_aid].Name = textBox1.Text;
                MainWindow.GetInstance().GlobalProject.Sounds[_aid].FileName = _fileName;
                MainWindow.GetInstance().GlobalProject.Sounds[_aid].ProjectPath = ProjectPath;


                if (_aid != MainWindow.GetInstance().GlobalProject.Sounds[_aid].Name)
                {
                    MainWindow.GetInstance().GlobalProject.Sounds.RenameKey(_aid, textBox1.Text);
                }
                if (GameProject.ProjectPath + "\\" + textBox2.Text != GameProject.ProjectPath + "\\assets\\sound\\" + textBox1.Text + ".png")
                {
                    _soundPlayer.Dispose();
                    File.Copy(GameProject.ProjectPath + "\\" + textBox2.Text, GameProject.ProjectPath + "\\assets\\music\\" + _fileName);
                    File.Delete(GameProject.ProjectPath + "\\" + textBox2.Text);
                    MainWindow.GetInstance().GlobalProject.Textures[_aid].FileName = "\\assets\\sound\\" + textBox1.Text + ".png";
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
            openFileDialog.Filter = "WAV|*.wav";
            openFileDialog.Title = "Select file";
            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            string file = openFileDialog.FileName;
            if (textBox1.TextLength == 0)
            {
                textBox1.Text = file.Split('\\').Last().Split('.').First();
                _fileName = textBox1.Text + ".wav";
            }
            File.Copy(file, GameProject.ProjectPath + "\\assets\\sound\\" + _fileName, true);
            textBox2.Text = ProjectPath + "\\" + _fileName;

            SetInfoBox();
        }

        private void MusicManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            _soundPlayer.Stop();
            _soundPlayer.Dispose();
        }
    }
}
