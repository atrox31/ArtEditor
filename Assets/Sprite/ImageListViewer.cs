﻿using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    public partial class ImageListViewer : Form
    {

        Sprite this_sprite;
        public ImageListViewer(Sprite sprite_data)
        {
            if (sprite_data == null)
            {
                Close();
            }
            this_sprite = sprite_data;
            InitializeComponent();Program.ApplyTheme(this);
        }

        private void ImageListViewer_Load(object sender, EventArgs e)
        {
            if (this_sprite.textures == null)
            {
                Close();
                return;
            }
            foreach (Image img in this_sprite.textures)
            {
                try
                {
                    this.imageList1.Images.Add(img);
                }
                catch
                {
                    Console.WriteLine("This is not an image file");
                }
            }
            this.listView1.View = View.LargeIcon;
            this.imageList1.ImageSize = new Size(64, 64);


            for (int j = 0; j < this.imageList1.Images.Count; j++)
            {
                ListViewItem item = new ListViewItem();
                item.ImageIndex = j;
                this.listView1.Items.Add(item);
            }
            this.listView1.LargeImageList = this.imageList1;

        }
    }

}
