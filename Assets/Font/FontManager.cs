using System.Drawing;
using System.Drawing.Text;

namespace ArtCore_Editor.Assets.Font
{
    public partial class FontManager : AssetManagerTemplate
    {
        public FontManager(string assetId = null)
        {
            RefToGameProjectDictionary = GameProject.GetInstance().Fonts;
            AssetFileExtensionsFilter = "TTF (*.ttf)|*.ttf";
            InitializeComponent(); Program.ApplyTheme(this);
            PrepareManager(assetId, typeof(FontManager));
        }
        
        // prepare asset to preview in editor
        protected override void SetAssetPreview()
        {
            PrivateFontCollection collection = new PrivateFontCollection();
            collection.AddFontFile(CurrentAsset.GetFilePath());
            FontFamily fontFamily = new FontFamily(collection.Families[0].Name, collection);
            _font = new System.Drawing.Font(fontFamily, 8);
            richTextBox1.Font = _font;
        }

        // fill info box with information about this assets
        protected override void SetInfoBox()
        {
            infoBoxLabel.Text = $"Font name: {_font.Name}\n" +
                                $"In project location:\\n{CurrentAsset?.ProjectPath}{CurrentAsset?.FileName}";
        }

        protected override void OnExit()
        {
            _font?.Dispose();
        }

        // custom asset manager members

        // font used to draw font example
        private System.Drawing.Font _font;
    }
}
