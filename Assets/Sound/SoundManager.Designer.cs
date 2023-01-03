namespace ArtCore_Editor.Assets.Sound { 
    partial class SoundManager : AssetManagerTemplate
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.buttonAddFile = new System.Windows.Forms.Button();
            this.fileBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nameInputBox = new System.Windows.Forms.TextBox();
            this.CancelButton = new System.Windows.Forms.Button();
            this.ApplyButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.infoBoxLabel = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
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
            this.groupBox2.TabIndex = 4;
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
            this.groupBox1.TabIndex = 3;
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
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(14, 253);
            this.CancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(88, 27);
            this.CancelButton.TabIndex = 9;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.ButtonCancel_click);
            // 
            // ApplyButton
            // 
            this.ApplyButton.Location = new System.Drawing.Point(120, 253);
            this.ApplyButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ApplyButton.Name = "ApplyButton";
            this.ApplyButton.Size = new System.Drawing.Size(88, 27);
            this.ApplyButton.TabIndex = 8;
            this.ApplyButton.Text = "Apply";
            this.ApplyButton.UseVisualStyleBackColor = true;
            this.ApplyButton.Click += new System.EventHandler(this.ButtonAccept_click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.infoBoxLabel);
            this.groupBox3.Location = new System.Drawing.Point(14, 136);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Size = new System.Drawing.Size(201, 75);
            this.groupBox3.TabIndex = 10;
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
            // button3
            // 
            this.button3.BackgroundImage = global::ArtCore_Editor.Properties.Resources.L1_R1_C1_Tlo_visible_normal_255;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button3.Location = new System.Drawing.Point(14, 218);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(28, 28);
            this.button3.TabIndex = 12;
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button6
            // 
            this.button6.BackgroundImage = global::ArtCore_Editor.Properties.Resources.L3_R1_C1_Warstwa_3_visible_normal_255;
            this.button6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button6.Location = new System.Drawing.Point(49, 218);
            this.button6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(28, 28);
            this.button6.TabIndex = 13;
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // SoundManager
            // 
            this.AcceptButton = this.ApplyButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(229, 293);
            this.ControlBox = false;
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.ApplyButton);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "SoundManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SoundManager";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Button buttonAddFile;
    private System.Windows.Forms.TextBox fileBox;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TextBox nameInputBox;
    private System.Windows.Forms.Button ApplyButton;
    private new System.Windows.Forms.Button CancelButton;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.Label infoBoxLabel;
    private System.Windows.Forms.Button button3;
    private System.Windows.Forms.Button button6;
}
}