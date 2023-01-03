namespace ArtCore_Editor.Assets.Font
{
    partial class FontManager : AssetManagerTemplate
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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonAddFile = new System.Windows.Forms.Button();
            this.fileBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nameInputBox = new System.Windows.Forms.TextBox();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.infoBoxLabel = new System.Windows.Forms.Label();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(12, 256);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(202, 110);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.buttonAddFile);
            this.groupBox2.Controls.Add(this.fileBox);
            this.groupBox2.Location = new System.Drawing.Point(14, 75);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Size = new System.Drawing.Size(201, 54);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "File";
            // 
            // buttonAddFile
            // 
            this.buttonAddFile.Location = new System.Drawing.Point(167, 21);
            this.buttonAddFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.buttonAddFile.Name = "buttonAddFile";
            this.buttonAddFile.Size = new System.Drawing.Size(27, 23);
            this.buttonAddFile.TabIndex = 1;
            this.buttonAddFile.Text = "+";
            this.buttonAddFile.UseVisualStyleBackColor = true;
            this.buttonAddFile.Click += new System.EventHandler(this.ButtonAddFile_click);
            // 
            // fileBox
            // 
            this.fileBox.Location = new System.Drawing.Point(7, 22);
            this.fileBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.fileBox.Name = "fileBox";
            this.fileBox.ReadOnly = true;
            this.fileBox.Size = new System.Drawing.Size(152, 23);
            this.fileBox.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nameInputBox);
            this.groupBox1.Location = new System.Drawing.Point(14, 14);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new System.Drawing.Size(201, 54);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Name";
            // 
            // nameInputBox
            // 
            this.nameInputBox.Location = new System.Drawing.Point(7, 22);
            this.nameInputBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.nameInputBox.Name = "nameInputBox";
            this.nameInputBox.Size = new System.Drawing.Size(186, 23);
            this.nameInputBox.TabIndex = 0;
            // 
            // ApplyButton
            // 
            this.ApplyButton.Location = new System.Drawing.Point(127, 382);
            this.ApplyButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(88, 27);
            this.ApplyButton.TabIndex = 9;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ButtonAccept_click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(12, 382);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(88, 27);
            this.CancelButton.TabIndex = 10;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.ButtonCancel_click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.infoBoxLabel);
            this.groupBox3.Location = new System.Drawing.Point(14, 135);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Size = new System.Drawing.Size(201, 115);
            this.groupBox3.TabIndex = 11;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Info";
            // 
            // infoBoxLabel
            // 
            this.infoBoxLabel.AutoSize = true;
            this.infoBoxLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoBoxLabel.Location = new System.Drawing.Point(4, 19);
            this.infoBoxLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.infoBoxLabel.Name = "infoBoxLabel";
            this.infoBoxLabel.Size = new System.Drawing.Size(0, 15);
            this.infoBoxLabel.TabIndex = 0;
            // 
            // FontManager
            // 
            this.AcceptButton = this.ApplyButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 418);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.richTextBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "FontManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FontManager";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button buttonAddFile;
        private System.Windows.Forms.TextBox fileBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox nameInputBox;
        private System.Windows.Forms.Button ApplyButton;
        private new System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label infoBoxLabel;
    }
}