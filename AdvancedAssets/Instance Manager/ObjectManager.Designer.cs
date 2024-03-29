﻿namespace ArtCore_Editor.AdvancedAssets.Instance_Manager
{
    partial class ObjectManager
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
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.lb_body_value_2 = new System.Windows.Forms.Label();
            this.bodyType_value_2 = new System.Windows.Forms.NumericUpDown();
            this.bodyType_IsSolid = new System.Windows.Forms.CheckBox();
            this.lb_body_value = new System.Windows.Forms.Label();
            this.bodyType_value_1 = new System.Windows.Forms.NumericUpDown();
            this.bodyType_circle = new System.Windows.Forms.RadioButton();
            this.bodyType_rect = new System.Windows.Forms.RadioButton();
            this.bodyType_mask = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.button7 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.Varible_listbox = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.Event_listobx = new System.Windows.Forms.ListBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.Event_treeview = new System.Windows.Forms.TreeView();
            this.button9 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button10 = new System.Windows.Forms.Button();
            this.chb_show_in_scene = new System.Windows.Forms.CheckBox();
            this.chb_show_in_level = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bodyType_value_2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bodyType_value_1)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox6);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new System.Drawing.Size(253, 538);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Properties";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.lb_body_value_2);
            this.groupBox6.Controls.Add(this.bodyType_value_2);
            this.groupBox6.Controls.Add(this.bodyType_IsSolid);
            this.groupBox6.Controls.Add(this.lb_body_value);
            this.groupBox6.Controls.Add(this.bodyType_value_1);
            this.groupBox6.Controls.Add(this.bodyType_circle);
            this.groupBox6.Controls.Add(this.bodyType_rect);
            this.groupBox6.Controls.Add(this.bodyType_mask);
            this.groupBox6.Location = new System.Drawing.Point(7, 194);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(239, 95);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "BodyData type";
            // 
            // lb_body_value_2
            // 
            this.lb_body_value_2.AutoSize = true;
            this.lb_body_value_2.Location = new System.Drawing.Point(96, 67);
            this.lb_body_value_2.Name = "lb_body_value_2";
            this.lb_body_value_2.Size = new System.Drawing.Size(43, 15);
            this.lb_body_value_2.TabIndex = 7;
            this.lb_body_value_2.Text = "Height";
            // 
            // bodyType_value_2
            // 
            this.bodyType_value_2.Location = new System.Drawing.Point(145, 65);
            this.bodyType_value_2.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.bodyType_value_2.Name = "bodyType_value_2";
            this.bodyType_value_2.Size = new System.Drawing.Size(82, 23);
            this.bodyType_value_2.TabIndex = 6;
            // 
            // bodyType_IsSolid
            // 
            this.bodyType_IsSolid.AutoSize = true;
            this.bodyType_IsSolid.Location = new System.Drawing.Point(150, 0);
            this.bodyType_IsSolid.Name = "bodyType_IsSolid";
            this.bodyType_IsSolid.Size = new System.Drawing.Size(83, 19);
            this.bodyType_IsSolid.TabIndex = 5;
            this.bodyType_IsSolid.Text = "Have body";
            this.bodyType_IsSolid.UseVisualStyleBackColor = true;
            this.bodyType_IsSolid.CheckedChanged += new System.EventHandler(this.bodyType_IsSolid_CheckedChanged);
            // 
            // lb_body_value
            // 
            this.lb_body_value.AutoSize = true;
            this.lb_body_value.Location = new System.Drawing.Point(100, 39);
            this.lb_body_value.Name = "lb_body_value";
            this.lb_body_value.Size = new System.Drawing.Size(39, 15);
            this.lb_body_value.TabIndex = 4;
            this.lb_body_value.Text = "Width";
            // 
            // bodyType_value_1
            // 
            this.bodyType_value_1.Location = new System.Drawing.Point(145, 37);
            this.bodyType_value_1.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.bodyType_value_1.Name = "bodyType_value_1";
            this.bodyType_value_1.Size = new System.Drawing.Size(82, 23);
            this.bodyType_value_1.TabIndex = 3;
            // 
            // bodyType_circle
            // 
            this.bodyType_circle.AutoSize = true;
            this.bodyType_circle.Location = new System.Drawing.Point(6, 69);
            this.bodyType_circle.Name = "bodyType_circle";
            this.bodyType_circle.Size = new System.Drawing.Size(55, 19);
            this.bodyType_circle.TabIndex = 2;
            this.bodyType_circle.TabStop = true;
            this.bodyType_circle.Text = "Circle";
            this.bodyType_circle.UseVisualStyleBackColor = true;
            this.bodyType_circle.CheckedChanged += new System.EventHandler(this.bodyType_circle_CheckedChanged);
            // 
            // bodyType_rect
            // 
            this.bodyType_rect.AutoSize = true;
            this.bodyType_rect.Location = new System.Drawing.Point(6, 44);
            this.bodyType_rect.Name = "bodyType_rect";
            this.bodyType_rect.Size = new System.Drawing.Size(48, 19);
            this.bodyType_rect.TabIndex = 1;
            this.bodyType_rect.TabStop = true;
            this.bodyType_rect.Text = "Rect";
            this.bodyType_rect.UseVisualStyleBackColor = true;
            this.bodyType_rect.CheckedChanged += new System.EventHandler(this.bodyType_rect_CheckedChanged);
            // 
            // bodyType_mask
            // 
            this.bodyType_mask.AutoSize = true;
            this.bodyType_mask.Location = new System.Drawing.Point(6, 19);
            this.bodyType_mask.Name = "bodyType_mask";
            this.bodyType_mask.Size = new System.Drawing.Size(86, 19);
            this.bodyType_mask.TabIndex = 0;
            this.bodyType_mask.TabStop = true;
            this.bodyType_mask.Text = "Sprite mask";
            this.bodyType_mask.UseVisualStyleBackColor = true;
            this.bodyType_mask.CheckedChanged += new System.EventHandler(this.bodyType_mask_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.button7);
            this.groupBox5.Controls.Add(this.button6);
            this.groupBox5.Controls.Add(this.Varible_listbox);
            this.groupBox5.Location = new System.Drawing.Point(7, 291);
            this.groupBox5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox5.Size = new System.Drawing.Size(239, 241);
            this.groupBox5.TabIndex = 3;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Vars";
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(145, 210);
            this.button7.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(88, 27);
            this.button7.TabIndex = 2;
            this.button7.Text = "Add";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(7, 210);
            this.button6.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(88, 27);
            this.button6.TabIndex = 1;
            this.button6.Text = "Delete";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // Varible_listbox
            // 
            this.Varible_listbox.FormattingEnabled = true;
            this.Varible_listbox.ItemHeight = 15;
            this.Varible_listbox.Location = new System.Drawing.Point(7, 22);
            this.Varible_listbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Varible_listbox.Name = "Varible_listbox";
            this.Varible_listbox.Size = new System.Drawing.Size(224, 184);
            this.Varible_listbox.TabIndex = 0;
            this.Varible_listbox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Varible_listbox_MouseDoubleClick);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chb_show_in_level);
            this.groupBox4.Controls.Add(this.chb_show_in_scene);
            this.groupBox4.Location = new System.Drawing.Point(7, 137);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox4.Size = new System.Drawing.Size(239, 51);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Editor placement";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.comboBox1);
            this.groupBox3.Location = new System.Drawing.Point(7, 80);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox3.Size = new System.Drawing.Size(239, 51);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sprite";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "<default>"});
            this.comboBox1.Location = new System.Drawing.Point(7, 20);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(224, 23);
            this.comboBox1.TabIndex = 0;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox1);
            this.groupBox2.Location = new System.Drawing.Point(7, 22);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox2.Size = new System.Drawing.Size(239, 51);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Obiect Name";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(7, 22);
            this.textBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(224, 23);
            this.textBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(425, 525);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 27);
            this.button1.TabIndex = 2;
            this.button1.Text = "Add Event";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(275, 525);
            this.button2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 27);
            this.button2.TabIndex = 3;
            this.button2.Text = "Delete";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Event_listobx
            // 
            this.Event_listobx.FormattingEnabled = true;
            this.Event_listobx.ItemHeight = 15;
            this.Event_listobx.Location = new System.Drawing.Point(276, 12);
            this.Event_listobx.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Event_listobx.Name = "Event_listobx";
            this.Event_listobx.Size = new System.Drawing.Size(237, 499);
            this.Event_listobx.TabIndex = 4;
            this.Event_listobx.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.Event_listobx.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Event_listbox_MouseDoubleClick);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1055, 525);
            this.button3.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(88, 27);
            this.button3.TabIndex = 6;
            this.button3.Text = "Apply";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button4.Location = new System.Drawing.Point(959, 525);
            this.button4.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(88, 27);
            this.button4.TabIndex = 7;
            this.button4.Text = "Cancel";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Enabled = false;
            this.button5.Location = new System.Drawing.Point(519, 12);
            this.button5.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(88, 27);
            this.button5.TabIndex = 8;
            this.button5.Text = "Script";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button8
            // 
            this.button8.Enabled = false;
            this.button8.Location = new System.Drawing.Point(615, 12);
            this.button8.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(88, 27);
            this.button8.TabIndex = 10;
            this.button8.Text = "Code";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // Event_treeview
            // 
            this.Event_treeview.Location = new System.Drawing.Point(519, 45);
            this.Event_treeview.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Event_treeview.Name = "Event_treeview";
            this.Event_treeview.Size = new System.Drawing.Size(622, 466);
            this.Event_treeview.TabIndex = 11;
            this.Event_treeview.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.Event_treeview_MouseDoubleClick);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(863, 525);
            this.button9.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(88, 27);
            this.button9.TabIndex = 12;
            this.button9.Text = "Save";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.label2.Location = new System.Drawing.Point(696, 531);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "*Only save, not close window.";
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(710, 12);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(89, 27);
            this.button10.TabIndex = 14;
            this.button10.Text = "Behaviour";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // chb_show_in_scene
            // 
            this.chb_show_in_scene.AutoSize = true;
            this.chb_show_in_scene.Location = new System.Drawing.Point(6, 22);
            this.chb_show_in_scene.Name = "chb_show_in_scene";
            this.chb_show_in_scene.Size = new System.Drawing.Size(101, 19);
            this.chb_show_in_scene.TabIndex = 0;
            this.chb_show_in_scene.Text = "Show in scene";
            this.chb_show_in_scene.UseVisualStyleBackColor = true;
            // 
            // chb_show_in_level
            // 
            this.chb_show_in_level.AutoSize = true;
            this.chb_show_in_level.Location = new System.Drawing.Point(113, 22);
            this.chb_show_in_level.Name = "chb_show_in_level";
            this.chb_show_in_level.Size = new System.Drawing.Size(95, 19);
            this.chb_show_in_level.TabIndex = 1;
            this.chb_show_in_level.Text = "Show in level";
            this.chb_show_in_level.UseVisualStyleBackColor = true;
            // 
            // ObjectManager
            // 
            this.AcceptButton = this.button3;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button4;
            this.ClientSize = new System.Drawing.Size(1156, 564);
            this.ControlBox = false;
            this.Controls.Add(this.button10);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.Event_treeview);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.Event_listobx);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ObjectManager";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "ObjectManager";
            this.groupBox1.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bodyType_value_2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bodyType_value_1)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox Event_listobx;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.ListBox Varible_listbox;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TreeView Event_treeview;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox bodyType_IsSolid;
        private System.Windows.Forms.Label lb_body_value;
        private System.Windows.Forms.NumericUpDown bodyType_value_1;
        private System.Windows.Forms.RadioButton bodyType_circle;
        private System.Windows.Forms.RadioButton bodyType_rect;
        private System.Windows.Forms.RadioButton bodyType_mask;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Label lb_body_value_2;
        private System.Windows.Forms.NumericUpDown bodyType_value_2;
        private System.Windows.Forms.CheckBox chb_show_in_level;
        private System.Windows.Forms.CheckBox chb_show_in_scene;
    }
}