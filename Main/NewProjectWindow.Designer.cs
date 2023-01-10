namespace ArtCore_Editor.Main
{
    partial class NewProjectWindow
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.brn_project_path = new System.Windows.Forms.Button();
            this.tbx_project_path = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chl_project_target_platform = new System.Windows.Forms.CheckedListBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbx_project_name = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chb_import_standard_main_menu = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nud_default_font_size = new System.Windows.Forms.NumericUpDown();
            this.chb_use_default_font = new System.Windows.Forms.CheckBox();
            this.btn_default_font = new System.Windows.Forms.Button();
            this.txb_default_font_path = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cbl_standard_behaviour = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_accept = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_default_font_size)).BeginInit();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.brn_project_path);
            this.groupBox1.Controls.Add(this.tbx_project_path);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(241, 61);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Project path";
            // 
            // brn_project_path
            // 
            this.brn_project_path.BackgroundImage = global::ArtCore_Editor.Properties.Resources.folder;
            this.brn_project_path.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.brn_project_path.Location = new System.Drawing.Point(203, 16);
            this.brn_project_path.Name = "brn_project_path";
            this.brn_project_path.Size = new System.Drawing.Size(32, 32);
            this.brn_project_path.TabIndex = 1;
            this.brn_project_path.UseVisualStyleBackColor = true;
            this.brn_project_path.Click += new System.EventHandler(this.brn_project_path_Click);
            // 
            // tbx_project_path
            // 
            this.tbx_project_path.Location = new System.Drawing.Point(6, 22);
            this.tbx_project_path.Name = "tbx_project_path";
            this.tbx_project_path.Size = new System.Drawing.Size(191, 23);
            this.tbx_project_path.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chl_project_target_platform);
            this.groupBox2.Location = new System.Drawing.Point(259, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(257, 125);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Target platform\'s";
            // 
            // chl_project_target_platform
            // 
            this.chl_project_target_platform.FormattingEnabled = true;
            this.chl_project_target_platform.Items.AddRange(new object[] {
            "Windows (x64) 7, 8, 8.1, 10, 11",
            "Linux (x64)",
            "MacOs (x64) 10 minimum",
            "Android (x64) Android 9 (API level 28)"});
            this.chl_project_target_platform.Location = new System.Drawing.Point(6, 22);
            this.chl_project_target_platform.Name = "chl_project_target_platform";
            this.chl_project_target_platform.Size = new System.Drawing.Size(245, 94);
            this.chl_project_target_platform.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.tbx_project_name);
            this.groupBox3.Location = new System.Drawing.Point(12, 79);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(241, 61);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Project name";
            // 
            // tbx_project_name
            // 
            this.tbx_project_name.Location = new System.Drawing.Point(6, 22);
            this.tbx_project_name.Name = "tbx_project_name";
            this.tbx_project_name.Size = new System.Drawing.Size(229, 23);
            this.tbx_project_name.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.chb_import_standard_main_menu);
            this.groupBox4.Controls.Add(this.groupBox5);
            this.groupBox4.Location = new System.Drawing.Point(12, 146);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(241, 269);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Project properties";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Enabled = false;
            this.label3.ForeColor = System.Drawing.SystemColors.AppWorkspace;
            this.label3.Location = new System.Drawing.Point(53, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "More options after 1.0";
            // 
            // chb_import_standard_main_menu
            // 
            this.chb_import_standard_main_menu.AutoSize = true;
            this.chb_import_standard_main_menu.Location = new System.Drawing.Point(6, 112);
            this.chb_import_standard_main_menu.Name = "chb_import_standard_main_menu";
            this.chb_import_standard_main_menu.Size = new System.Drawing.Size(175, 19);
            this.chb_import_standard_main_menu.TabIndex = 0;
            this.chb_import_standard_main_menu.Text = "Import standard main menu";
            this.chb_import_standard_main_menu.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.nud_default_font_size);
            this.groupBox5.Controls.Add(this.chb_use_default_font);
            this.groupBox5.Controls.Add(this.btn_default_font);
            this.groupBox5.Controls.Add(this.txb_default_font_path);
            this.groupBox5.Location = new System.Drawing.Point(6, 22);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(229, 84);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Default font";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(117, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Font size";
            // 
            // nud_default_font_size
            // 
            this.nud_default_font_size.Location = new System.Drawing.Point(176, 54);
            this.nud_default_font_size.Name = "nud_default_font_size";
            this.nud_default_font_size.Size = new System.Drawing.Size(47, 23);
            this.nud_default_font_size.TabIndex = 3;
            // 
            // chb_use_default_font
            // 
            this.chb_use_default_font.AutoSize = true;
            this.chb_use_default_font.Location = new System.Drawing.Point(6, 55);
            this.chb_use_default_font.Name = "chb_use_default_font";
            this.chb_use_default_font.Size = new System.Drawing.Size(119, 19);
            this.chb_use_default_font.TabIndex = 2;
            this.chb_use_default_font.Text = "Use standard font";
            this.chb_use_default_font.UseVisualStyleBackColor = true;
            this.chb_use_default_font.CheckedChanged += new System.EventHandler(this.chb_use_default_font_CheckedChanged);
            // 
            // btn_default_font
            // 
            this.btn_default_font.BackgroundImage = global::ArtCore_Editor.Properties.Resources.folder;
            this.btn_default_font.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btn_default_font.Location = new System.Drawing.Point(191, 16);
            this.btn_default_font.Name = "btn_default_font";
            this.btn_default_font.Size = new System.Drawing.Size(32, 32);
            this.btn_default_font.TabIndex = 1;
            this.btn_default_font.UseVisualStyleBackColor = true;
            // 
            // txb_default_font_path
            // 
            this.txb_default_font_path.Location = new System.Drawing.Point(6, 22);
            this.txb_default_font_path.Name = "txb_default_font_path";
            this.txb_default_font_path.Size = new System.Drawing.Size(179, 23);
            this.txb_default_font_path.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.cbl_standard_behaviour);
            this.groupBox6.Controls.Add(this.label2);
            this.groupBox6.Location = new System.Drawing.Point(259, 146);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(257, 269);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Standard behaviour";
            // 
            // cbl_standard_behaviour
            // 
            this.cbl_standard_behaviour.FormattingEnabled = true;
            this.cbl_standard_behaviour.Location = new System.Drawing.Point(6, 38);
            this.cbl_standard_behaviour.Name = "cbl_standard_behaviour";
            this.cbl_standard_behaviour.Size = new System.Drawing.Size(245, 220);
            this.cbl_standard_behaviour.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Import";
            // 
            // btn_accept
            // 
            this.btn_accept.Location = new System.Drawing.Point(435, 421);
            this.btn_accept.Name = "btn_accept";
            this.btn_accept.Size = new System.Drawing.Size(75, 24);
            this.btn_accept.TabIndex = 5;
            this.btn_accept.Text = "Create";
            this.btn_accept.UseVisualStyleBackColor = true;
            this.btn_accept.Click += new System.EventHandler(this.btn_accept_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(354, 421);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(75, 24);
            this.btn_cancel.TabIndex = 6;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            // 
            // NewProjectWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 457);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_accept);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NewProjectWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NewProjectWindow";
            this.Load += new System.EventHandler(this.NewProjectWindow_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_default_font_size)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button brn_project_path;
        private System.Windows.Forms.TextBox tbx_project_path;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckedListBox chl_project_target_platform;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tbx_project_name;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nud_default_font_size;
        private System.Windows.Forms.CheckBox chb_use_default_font;
        private System.Windows.Forms.Button btn_default_font;
        private System.Windows.Forms.TextBox txb_default_font_path;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckedListBox cbl_standard_behaviour;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chb_import_standard_main_menu;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_accept;
        private System.Windows.Forms.Button btn_cancel;
    }
}