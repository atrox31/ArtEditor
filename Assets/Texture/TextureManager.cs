using System;
using System.Windows.Forms;


namespace ArtCore_Editor.Assets.Texture
{
    public partial class TextureManager : AssetManagerTemplate
    {
        public TextureManager(string assetId = null)
        {
            RefToGameProjectDictionary = GameProject.GetInstance().Textures;
            AssetFileExtensionsFilter = "PNG (*.png)|*.png";
            InitializeComponent(); Program.ApplyTheme(this);
            PrepareManager(assetId, typeof(TextureManager));
            this.CancelButton.Click += new System.EventHandler(this.ButtonCancel_click);
            this.ApplyButton.Click += new System.EventHandler(this.ButtonAccept_click);
            this.buttonAddFile.Click += new System.EventHandler(this.ButtonAddFile_click);
        }

        // prepare asset to preview in editor
        protected override void SetAssetPreview()
        {
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = null;
            pictureBox1.ImageLocation = CurrentAsset?.GetFilePath();
            pictureBox1.Load();
        }

        // fill info box with information about this assets
        protected override void SetInfoBox()
        {
            infoBoxLabel.Text = $"Image dimensions: {pictureBox1.Image?.Width}x{pictureBox1.Image?.Height}\n" +
                                $"In project location:\n{CurrentAsset?.ProjectPath}{CurrentAsset?.FileName}";
        }

        protected override void OnExit()
        {
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = null;
        }

        // custom asset manager members
        // null
        
        // view in full dimensions button
        private void button3_Click(object sender, EventArgs e)
        {
            // view in full
            if (pictureBox1.Image == null) return;

            Form temp = new Form();
            temp.Size = pictureBox1.Image.Size;
            temp.BackgroundImage = pictureBox1.Image;
            temp.Text = nameInputBox.Text;
            temp.ShowDialog();
        }
        
    }
    
}
