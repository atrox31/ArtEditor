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
            this.chb_target_4 = new System.Windows.Forms.CheckBox();
            this.chb_target_2 = new System.Windows.Forms.CheckBox();
            this.chb_target_1 = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tbx_project_name = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chb_import_standard_main_menu = new System.Windows.Forms.CheckBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.cbl_standard_behaviour = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_accept = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
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
            this.tbx_project_path.ReadOnly = true;
            this.tbx_project_path.Size = new System.Drawing.Size(191, 23);
            this.tbx_project_path.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chb_target_4);
            this.groupBox2.Controls.Add(this.chb_target_2);
            this.groupBox2.Controls.Add(this.chb_target_1);
            this.groupBox2.Location = new System.Drawing.Point(259, 15);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(257, 125);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Target platform\'s";
            // 
            // chb_target_4
            // 
            this.chb_target_4.AutoSize = true;
            this.chb_target_4.Location = new System.Drawing.Point(6, 76);
            this.chb_target_4.Name = "chb_target_4";
            this.chb_target_4.Size = new System.Drawing.Size(83, 19);
            this.chb_target_4.TabIndex = 3;
            this.chb_target_4.Text = "checkBox4";
            this.chb_target_4.UseVisualStyleBackColor = true;
            // 
            // chb_target_2
            // 
            this.chb_target_2.AutoSize = true;
            this.chb_target_2.Location = new System.Drawing.Point(6, 51);
            this.chb_target_2.Name = "chb_target_2";
            this.chb_target_2.Size = new System.Drawing.Size(83, 19);
            this.chb_target_2.TabIndex = 1;
            this.chb_target_2.Text = "checkBox2";
            this.chb_target_2.UseVisualStyleBackColor = true;
            // 
            // chb_target_1
            // 
            this.chb_target_1.AutoSize = true;
            this.chb_target_1.Location = new System.Drawing.Point(6, 26);
            this.chb_target_1.Name = "chb_target_1";
            this.chb_target_1.Size = new System.Drawing.Size(83, 19);
            this.chb_target_1.TabIndex = 0;
            this.chb_target_1.Text = "checkBox1";
            this.chb_target_1.UseVisualStyleBackColor = true;
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
            this.tbx_project_name.ReadOnly = true;
            this.tbx_project_name.Size = new System.Drawing.Size(229, 23);
            this.tbx_project_name.TabIndex = 0;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label3);
            this.groupBox4.Controls.Add(this.chb_import_standard_main_menu);
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
            this.label3.Location = new System.Drawing.Point(58, 141);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 15);
            this.label3.TabIndex = 2;
            this.label3.Text = "More options after 1.0";
            // 
            // chb_import_standard_main_menu
            // 
            this.chb_import_standard_main_menu.AutoSize = true;
            this.chb_import_standard_main_menu.Location = new System.Drawing.Point(6, 22);
            this.chb_import_standard_main_menu.Name = "chb_import_standard_main_menu";
            this.chb_import_standard_main_menu.Size = new System.Drawing.Size(175, 19);
            this.chb_import_standard_main_menu.TabIndex = 0;
            this.chb_import_standard_main_menu.Text = "Import standard main menu";
            this.chb_import_standard_main_menu.UseVisualStyleBackColor = true;
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
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
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
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button brn_project_path;
        private System.Windows.Forms.TextBox tbx_project_path;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox tbx_project_name;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckedListBox cbl_standard_behaviour;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chb_import_standard_main_menu;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btn_accept;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.CheckBox chb_target_4;
        private System.Windows.Forms.CheckBox chb_target_2;
        private System.Windows.Forms.CheckBox chb_target_1;
    }
}