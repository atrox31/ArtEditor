namespace ArtCore_Editor.AdvancedAssets.SceneManager.TriggerEditor
{
    partial class TriggerEditor
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
            this.Event_listobx = new System.Windows.Forms.ListBox();
            this.Event_treeview = new System.Windows.Forms.TreeView();
            this.btn_code = new System.Windows.Forms.Button();
            this.btn_script = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_apply = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Event_listobx
            // 
            this.Event_listobx.FormattingEnabled = true;
            this.Event_listobx.ItemHeight = 15;
            this.Event_listobx.Location = new System.Drawing.Point(13, 3);
            this.Event_listobx.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Event_listobx.Name = "Event_listobx";
            this.Event_listobx.Size = new System.Drawing.Size(351, 394);
            this.Event_listobx.TabIndex = 5;
            this.Event_listobx.SelectedIndexChanged += new System.EventHandler(this.Event_listbox_SelectedIndexChanged);
            // 
            // Event_treeview
            // 
            this.Event_treeview.Location = new System.Drawing.Point(372, 3);
            this.Event_treeview.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Event_treeview.Name = "Event_treeview";
            this.Event_treeview.Size = new System.Drawing.Size(582, 394);
            this.Event_treeview.TabIndex = 12;
            this.Event_treeview.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Event_treeView_MouseDoubleClick);
            // 
            // btn_code
            // 
            this.btn_code.Location = new System.Drawing.Point(468, 403);
            this.btn_code.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_code.Name = "btn_code";
            this.btn_code.Size = new System.Drawing.Size(88, 27);
            this.btn_code.TabIndex = 14;
            this.btn_code.Text = "Code";
            this.btn_code.UseVisualStyleBackColor = true;
            this.btn_code.Click += new System.EventHandler(this.btn_code_Click);
            // 
            // btn_script
            // 
            this.btn_script.Location = new System.Drawing.Point(372, 403);
            this.btn_script.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_script.Name = "btn_script";
            this.btn_script.Size = new System.Drawing.Size(88, 27);
            this.btn_script.TabIndex = 13;
            this.btn_script.Text = "Script";
            this.btn_script.UseVisualStyleBackColor = true;
            this.btn_script.Click += new System.EventHandler(this.btn_script_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(770, 403);
            this.btn_cancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(88, 27);
            this.btn_cancel.TabIndex = 16;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_apply
            // 
            this.btn_apply.Location = new System.Drawing.Point(866, 403);
            this.btn_apply.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.Size = new System.Drawing.Size(88, 27);
            this.btn_apply.TabIndex = 15;
            this.btn_apply.Text = "Apply";
            this.btn_apply.UseVisualStyleBackColor = true;
            this.btn_apply.Click += new System.EventHandler(this.btn_apply_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(109, 403);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 27);
            this.button2.TabIndex = 18;
            this.button2.Text = "Delete";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 403);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 27);
            this.button1.TabIndex = 17;
            this.button1.Text = "Add Event";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // TriggerEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(967, 439);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_apply);
            this.Controls.Add(this.btn_code);
            this.Controls.Add(this.btn_script);
            this.Controls.Add(this.Event_treeview);
            this.Controls.Add(this.Event_listobx);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TriggerEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TriggerEditor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox Event_listobx;
        private System.Windows.Forms.TreeView Event_treeview;
        private System.Windows.Forms.Button btn_code;
        private System.Windows.Forms.Button btn_script;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_apply;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
    }
}