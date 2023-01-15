using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ArtCore_Editor.AdvancedAssets.SpriteManager
{
    public partial class ImageListViewer : Form
    {
        public ImageListViewer()
        {
            InitializeComponent(); Program.ApplyTheme(this);

            listView1.View = View.LargeIcon;
            imageList1.ImageSize = new Size(64, 64);

        }

        public static void Show(List<Image> Textures)
        {
            ImageListViewer instance = new ImageListViewer();
            if (Textures == null || Textures.Count == 0)
            {
                return;
            }
            instance.imageList1.Images.AddRange(Textures.ToArray());
            instance.ShowDialog();
        }

        private void ImageListViewer_Load(object sender, EventArgs e)
        {
            
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
