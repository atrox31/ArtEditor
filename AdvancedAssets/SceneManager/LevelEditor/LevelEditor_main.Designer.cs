namespace ArtCore_Editor.AdvancedAssets.SceneManager.LevelEditor
{
    partial class LevelEditorMain
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
            this.Content = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Content)).BeginInit();
            this.SuspendLayout();
            // 
            // Content
            // 
            this.Content.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Content.Location = new System.Drawing.Point(0, 0);
            this.Content.Name = "Content";
            this.Content.Size = new System.Drawing.Size(800, 450);
            this.Content.TabIndex = 0;
            this.Content.TabStop = false;
            this.Content.Paint += new System.Windows.Forms.PaintEventHandler(this.PictureBox1_Paint);
            this.Content.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseClick);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 34;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // LevelEditor_main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Content);
            this.Name = "LevelEditorMain";
            this.Text = "LevelEditor_main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LevelEditor_main_FormClosing);
            this.Load += new System.EventHandler(this.LevelEditor_main_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Content)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox Content;
        private System.Windows.Forms.Timer timer1;
    }
}