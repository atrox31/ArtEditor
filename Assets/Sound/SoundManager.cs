using System;
using System.ComponentModel;
using System.IO;
using System.Media;
using ArtCore_Editor.Functions;

namespace ArtCore_Editor.Assets.Sound
{
    public partial class SoundManager : AssetManagerTemplate
    {
        public SoundManager(string assetId = null)
        {
            RefToGameProjectDictionary = GameProject.GetInstance().Sounds;
            AssetFileExtensionsFilter = "WAV (*.wav)|*.wav";
            InitializeComponent(); Program.ApplyTheme(this);
            PrepareManager(assetId, typeof(SoundManager));
        }

        // prepare asset to preview in editor
        protected override void SetAssetPreview()
        {
            _canPlay = false;
            if (File.Exists(CurrentAsset.GetFilePath()))
            {
                _soundPlayer.Stop();
                _soundPlayer.SoundLocation = CurrentAsset.GetFilePath();
                _soundPlayer.LoadAsync();
            }
        }

        // fill info box with information about this assets
        protected override void SetInfoBox()
        {
            infoBoxLabel.Text = $"Duration: {SoundInfo.GetSoundLength(CurrentAsset?.GetFilePath())}ms" +
                                $" \n" +
                                $"In project location:\n{CurrentAsset?.ProjectPath}{CurrentAsset?.FileName}";
        }

        // custom asset manager members

        // flat to check if music is loaded for playing
        private bool _canPlay = false;
        // player to listen target sound
        private readonly SoundPlayer _soundPlayer = new SoundPlayer();

        protected override void OnLoad()
        {
            _soundPlayer.LoadCompleted += new AsyncCompletedEventHandler(player_LoadCompleted);
            _soundPlayer.SoundLocationChanged += new EventHandler(player_LocationChanged);
        }

        protected override void OnExit()
        {
            _soundPlayer.Stop();
            _soundPlayer.Dispose();
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
    }
}
