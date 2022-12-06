using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArtCore_Editor.Assets.Sprite
{
    public partial class ImageListViewer : Form
    {
        private readonly Sprite _thisSprite;
        public ImageListViewer(Sprite spriteData)
        {
            if (spriteData == null)
            {
                Close();
            }
            _thisSprite = spriteData;
            InitializeComponent(); Program.ApplyTheme(this);
        }

        private void ImageListViewer_Load(object sender, EventArgs e)
        {
            if (_thisSprite.Textures == null)
            {
                Close();
                return;
            }
            foreach (Image img in _thisSprite.Textures)
            {
                try
                {
                    imageList1.Images.Add(img);
                }
                catch
                {
                    Console.WriteLine("This is not an image file");
                }
            }
            listView1.View = View.LargeIcon;
            imageList1.ImageSize = new Size(64, 64);


            for (int j = 0; j < imageList1.Images.Count; j++)
            {
                ListViewItem item = new ListViewItem
                {
                    ImageIndex = j
                };
                listView1.Items.Add(item);
            }
            listView1.LargeImageList = imageList1;

        }
    }

}
