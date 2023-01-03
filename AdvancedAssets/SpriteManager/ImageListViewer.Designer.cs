namespace ArtCore_Editor.AdvancedAssets.SpriteManager { 
    partial class ImageListViewer
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.listView1 = new System.Windows.Forms.ListView();
        this.imageList1 = new System.Windows.Forms.ImageList(this.components);
        this.SuspendLayout();
        // 
        // listView1
        // 
        this.listView1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
        this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
        this.listView1.HideSelection = false;
        this.listView1.Location = new System.Drawing.Point(0, 0);
        this.listView1.Name = "listView1";
        this.listView1.Size = new System.Drawing.Size(800, 450);
        this.listView1.TabIndex = 0;
        this.listView1.UseCompatibleStateImageBehavior = false;
        // 
        // imageList1
        // 
        this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
        this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
        this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
        // 
        // ImageListViewer
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.BackColor = System.Drawing.SystemColors.ActiveCaption;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Controls.Add(this.listView1);
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "ImageListViewer";
        this.ShowIcon = false;
        this.ShowInTaskbar = false;
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "Textures Preview";
        this.Load += new System.EventHandler(this.ImageListViewer_Load);
        this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.ListView listView1;
    private System.Windows.Forms.ImageList imageList1;
}
}