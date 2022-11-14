using ArtCore_Editor.Assets;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    public partial class SoundManager : Form
    {
        string aid;
        bool can_play = false;
        string GetDuration(bool formated = false)
        {
            return SoundInfo.GetSoundLength(GameProject.ProjectPath + "\\" + textBox2.Text).ToString();
        }
        private void SetInfoBox()
        {
            soundPlayer.Stop();
            soundPlayer.SoundLocation = GameProject.ProjectPath + "\\" + textBox2.Text;
            can_play = false;
            soundPlayer.LoadAsync();
            label1.Text = "Duration: " + GetDuration(true) + " \n" +
                "In project location:\n" + "assets/music/" + textBox1.Text;
        }

        SoundPlayer soundPlayer = new SoundPlayer();

        private void player_LoadCompleted(object sender,
                AsyncCompletedEventArgs e)
        {
            can_play = true;
        }
        private void player_LocationChanged(object sender, EventArgs e)
        {
            can_play = true;
        }

        string FileName;
        string ProjectPath= "assets\\sound\\";

        public SoundManager(string AssetId = null)
        {
            soundPlayer.LoadCompleted += new AsyncCompletedEventHandler(player_LoadCompleted);
            soundPlayer.SoundLocationChanged += new EventHandler(player_LocationChanged);
            InitializeComponent();Program.ApplyTheme(this);
            aid = AssetId;
            if (AssetId != null)
            {
                textBox1.Text = MainWindow.GetInstance().Game_Project.Sounds[AssetId].Name;
                FileName = MainWindow.GetInstance().Game_Project.Sounds[AssetId].FileName;

                textBox2.Text = ProjectPath + "\\" + FileName;
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
            if (can_play)
                soundPlayer.Play();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // stop
            soundPlayer.Stop();
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
                    MainWindow.GetInstance().Game_Project.Sounds.Add(textBox1.Text, new Asset());
                }
                MainWindow.GetInstance().Game_Project.Sounds[aid].Name = textBox1.Text;
                MainWindow.GetInstance().Game_Project.Sounds[aid].FileName = FileName;
                MainWindow.GetInstance().Game_Project.Sounds[aid].ProjectPath = ProjectPath;


                if (aid != MainWindow.GetInstance().Game_Project.Sounds[aid].Name)
                {
                    Functions.RenameKey(MainWindow.GetInstance().Game_Project.Sounds, aid, textBox1.Text);
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
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string ofile = openFileDialog.FileName;
                if (textBox1.TextLength == 0)
                {
                    textBox1.Text = ofile.Split('\\').Last().Split('.').First();
                    FileName = textBox1.Text + ".wav";
                }
                File.Copy(ofile, GameProject.ProjectPath + "\\assets\\sound\\" + FileName, true);
                textBox2.Text = ProjectPath + "\\" + FileName;

                SetInfoBox();
            }
        }

        private void MusicManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            soundPlayer.Stop();
            soundPlayer.Dispose();
        }
    }
}
