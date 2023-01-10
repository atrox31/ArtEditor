using ArtCore_Editor.Main;
using LibVLCSharp.Shared;
using System;
using System.IO;
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;

namespace ArtCore_Editor.Assets.Music
{
    public partial class MusicManager : AssetManagerTemplate
    {
        public MusicManager(string assetId = null)
        {
            RefToGameProjectDictionary = GameProject.GetInstance().Music;
            AssetFileExtensionsFilter = "OGG (preferred) (*.ogg)|*.ogg|WAV (*.wav)|*.wav|MP3 (*.mp3)|*.mp3";
            InitializeComponent(); Program.ApplyTheme(this);
            PrepareManager(assetId, typeof(MusicManager));
        }

        // prepare asset to preview in editor
        protected override void SetAssetPreview()
        {
            if (!File.Exists(CurrentAsset.GetFilePath())) return;

            // setup track bar to change music position
            trackBar1.Value = 0;

            _mediaPlayer.Media?.Dispose();

            _mediaPlayer.Media = new Media(_libVlc, CurrentAsset.GetFilePath() );
            _mediaPlayer.Media.AddOption(":no-video");
            // .wait is for load music, else we have not can get duration
            _mediaPlayer.Media.Parse(MediaParseOptions.FetchLocal).Wait();
        }

        // fill info box with information about this assets
        protected override void SetInfoBox()
        {
            long? duration = _mediaPlayer.Media?.Duration;
            
            infoBoxLabel.Text = $"Duration: {(duration ?? 0)}ms" +
                                $" ({TimeSpan.FromMilliseconds((double)(duration ?? 0)).ToString(@"hh\:mm\:ss")})" +
                                $" \n" +
                                $"In project location:\n{CurrentAsset?.ProjectPath}{CurrentAsset?.FileName}";
        }
        // custom asset manager members

        // player to listen target sound
        private LibVLC _libVlc;
        private MediaPlayer _mediaPlayer;
        protected override void OnLoad()
        {
            _libVlc = new LibVLC();
            _mediaPlayer = new MediaPlayer(_libVlc);
            
            _mediaPlayer.LengthChanged += (object sender, MediaPlayerLengthChangedEventArgs e) =>
            {
                trackBar1.Invoke(delegate { trackBar1.Maximum = (int)(_mediaPlayer.Media.Duration / 1000); });
                Invoke(SetInfoBox);
            };

            _mediaPlayer.TimeChanged += (object sender, MediaPlayerTimeChangedEventArgs e) =>
            {
                // try because  sometimes throws error, not important to handle
                try
                {
                    trackBar1.Invoke(delegate { trackBar1.Value = (int)(e.Time / 1000); });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            };

            trackBar1.MouseClick += (sender, args) =>
            {
                // try because  sometimes throws error, not important to handle
                try
                {
                    _mediaPlayer.SeekTo(TimeSpan.FromMilliseconds(trackBar1.Value * 1000));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            };
        }

        protected override void OnExit()
        {
            _mediaPlayer.Stop();
            _mediaPlayer.Dispose();
            _libVlc.Dispose();
        }
        private void button3_Click(object sender, EventArgs e)
        {// play button

            _mediaPlayer.Play();
        }

        private void button5_Click(object sender, EventArgs e)
        { // pause button

            _mediaPlayer.Pause();
        }

        private void button6_Click(object sender, EventArgs e)
        { // stop button

            _mediaPlayer.Stop();
        }
    }
}
