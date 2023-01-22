namespace ArtCore_Editor.AdvancedAssets.SceneManager.LevelEditor
{
    partial class LevelEditorTools
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LevelEditorTools));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.instance_list_view = new System.Windows.Forms.ListView();
            this.Instance_imagelist = new System.Windows.Forms.ImageList(this.components);
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btn_trigger_delete = new System.Windows.Forms.Button();
            this.btn_trigger_edit = new System.Windows.Forms.Button();
            this.btn_trigger_new = new System.Windows.Forms.Button();
            this.trigger_listbox = new System.Windows.Forms.ListBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(325, 613);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.instance_list_view);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(317, 585);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Tag = "object_tab";
            this.tabPage1.Text = "Objects";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // instance_list_view
            // 
            this.instance_list_view.Dock = System.Windows.Forms.DockStyle.Fill;
            this.instance_list_view.LargeImageList = this.Instance_imagelist;
            this.instance_list_view.Location = new System.Drawing.Point(3, 3);
            this.instance_list_view.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.instance_list_view.MultiSelect = false;
            this.instance_list_view.Name = "instance_list_view";
            this.instance_list_view.Size = new System.Drawing.Size(311, 579);
            this.instance_list_view.TabIndex = 1;
            this.instance_list_view.UseCompatibleStateImageBehavior = false;
            this.instance_list_view.SelectedIndexChanged += new System.EventHandler(this.instance_list_view_SelectedIndexChanged);
            // 
            // Instance_imagelist
            // 
            this.Instance_imagelist.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.Instance_imagelist.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("Instance_imagelist.ImageStream")));
            this.Instance_imagelist.TransparentColor = System.Drawing.Color.Transparent;
            this.Instance_imagelist.Images.SetKeyName(0, "interrogation.png");
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btn_trigger_delete);
            this.tabPage2.Controls.Add(this.btn_trigger_edit);
            this.tabPage2.Controls.Add(this.btn_trigger_new);
            this.tabPage2.Controls.Add(this.trigger_listbox);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(317, 585);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Tag = "triggers_tab";
            this.tabPage2.Text = "Triggers";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btn_trigger_delete
            // 
            this.btn_trigger_delete.Location = new System.Drawing.Point(44, 554);
            this.btn_trigger_delete.Name = "btn_trigger_delete";
            this.btn_trigger_delete.Size = new System.Drawing.Size(75, 23);
            this.btn_trigger_delete.TabIndex = 3;
            this.btn_trigger_delete.Text = "Delete";
            this.btn_trigger_delete.UseVisualStyleBackColor = true;
            this.btn_trigger_delete.Click += new System.EventHandler(this.btn_trigger_delete_Click);
            // 
            // btn_trigger_edit
            // 
            this.btn_trigger_edit.Location = new System.Drawing.Point(125, 554);
            this.btn_trigger_edit.Name = "btn_trigger_edit";
            this.btn_trigger_edit.Size = new System.Drawing.Size(75, 23);
            this.btn_trigger_edit.TabIndex = 2;
            this.btn_trigger_edit.Text = "Edit";
            this.btn_trigger_edit.UseVisualStyleBackColor = true;
            this.btn_trigger_edit.Click += new System.EventHandler(this.btn_trigger_edit_Click);
            // 
            // btn_trigger_new
            // 
            this.btn_trigger_new.Location = new System.Drawing.Point(206, 554);
            this.btn_trigger_new.Name = "btn_trigger_new";
            this.btn_trigger_new.Size = new System.Drawing.Size(75, 23);
            this.btn_trigger_new.TabIndex = 1;
            this.btn_trigger_new.Text = "New";
            this.btn_trigger_new.UseVisualStyleBackColor = true;
            this.btn_trigger_new.Click += new System.EventHandler(this.btn_trigger_new_Click);
            // 
            // trigger_listbox
            // 
            this.trigger_listbox.FormattingEnabled = true;
            this.trigger_listbox.ItemHeight = 15;
            this.trigger_listbox.Location = new System.Drawing.Point(6, 6);
            this.trigger_listbox.Name = "trigger_listbox";
            this.trigger_listbox.Size = new System.Drawing.Size(275, 544);
            this.trigger_listbox.TabIndex = 0;
            this.trigger_listbox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.trigger_listbox_MouseDoubleClick);
            // 
            // LevelEditorTools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(325, 613);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "LevelEditorTools";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "LevelEditor_Tools";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.LevelEditor_Tools_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView instance_list_view;
        private System.Windows.Forms.ImageList Instance_imagelist;
        private System.Windows.Forms.Button btn_trigger_delete;
        private System.Windows.Forms.Button btn_trigger_edit;
        private System.Windows.Forms.Button btn_trigger_new;
        private System.Windows.Forms.ListBox trigger_listbox;
    }
}