namespace ArtCore_Editor.AdvancedAssets.SpriteManager
{
    partial class SpriteManager
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lb_sprite_info = new System.Windows.Forms.Label();
            this.bt_import = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cb_mask_ratio = new System.Windows.Forms.CheckBox();
            this.nm_sprite_mask_radius = new System.Windows.Forms.NumericUpDown();
            this.lb_sprite_mask_radius = new System.Windows.Forms.Label();
            this.lb_sprite_mask_height = new System.Windows.Forms.Label();
            this.nm_sprite_mask_height = new System.Windows.Forms.NumericUpDown();
            this.cb_show_sprite_mask = new System.Windows.Forms.CheckBox();
            this.lb_sprite_mask_width = new System.Windows.Forms.Label();
            this.nm_sprite_mask_width = new System.Windows.Forms.NumericUpDown();
            this.rb_mask_circle = new System.Windows.Forms.RadioButton();
            this.rb_mask_rect = new System.Windows.Forms.RadioButton();
            this.rb_mask_none = new System.Windows.Forms.RadioButton();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.bt_as_delete = new System.Windows.Forms.Button();
            this.bt_as_edit = new System.Windows.Forms.Button();
            this.bt_as_add = new System.Windows.Forms.Button();
            this.lb_animation_sequence = new System.Windows.Forms.ListBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nm_center_h = new System.Windows.Forms.NumericUpDown();
            this.cb_show_sprite_center = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.nm_center_w = new System.Windows.Forms.NumericUpDown();
            this.rb_center_custom = new System.Windows.Forms.RadioButton();
            this.rb_center_center = new System.Windows.Forms.RadioButton();
            this.rb_center_00 = new System.Windows.Forms.RadioButton();
            this.groupBoxSpriteName = new System.Windows.Forms.GroupBox();
            this.tb_sprite_name = new System.Windows.Forms.TextBox();
            this.sprite_preview = new System.Windows.Forms.PictureBox();
            this.bt_accept = new System.Windows.Forms.Button();
            this.bt_cancel = new System.Windows.Forms.Button();
            this.control_box_region = new System.Windows.Forms.GroupBox();
            this.tb_control_box_info = new System.Windows.Forms.TextBox();
            this.cb_loop = new System.Windows.Forms.CheckBox();
            this.bt_last = new System.Windows.Forms.Button();
            this.bt_next = new System.Windows.Forms.Button();
            this.bt_prev = new System.Windows.Forms.Button();
            this.bt_first = new System.Windows.Forms.Button();
            this.bt_stop = new System.Windows.Forms.Button();
            this.bt_pause = new System.Windows.Forms.Button();
            this.br_play = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.nm_fps = new System.Windows.Forms.NumericUpDown();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.ls_animation_sequence = new System.Windows.Forms.ComboBox();
            this.lb_preview_info = new System.Windows.Forms.Label();
            this.update_preview_timer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nm_sprite_mask_radius)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nm_sprite_mask_height)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nm_sprite_mask_width)).BeginInit();
            this.groupBox9.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nm_center_h)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nm_center_w)).BeginInit();
            this.groupBoxSpriteName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sprite_preview)).BeginInit();
            this.control_box_region.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nm_fps)).BeginInit();
            this.groupBox10.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox9);
            this.groupBox1.Controls.Add(this.groupBox6);
            this.groupBox1.Controls.Add(this.groupBoxSpriteName);
            this.groupBox1.Location = new System.Drawing.Point(13, 12);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new System.Drawing.Size(253, 568);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Properties";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lb_sprite_info);
            this.groupBox2.Controls.Add(this.bt_import);
            this.groupBox2.Location = new System.Drawing.Point(6, 79);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(237, 88);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sprite data";
            // 
            // lb_sprite_info
            // 
            this.lb_sprite_info.AutoSize = true;
            this.lb_sprite_info.Location = new System.Drawing.Point(7, 19);
            this.lb_sprite_info.Name = "lb_sprite_info";
            this.lb_sprite_info.Size = new System.Drawing.Size(38, 15);
            this.lb_sprite_info.TabIndex = 1;
            this.lb_sprite_info.Text = "label5";
            // 
            // bt_import
            // 
            this.bt_import.Location = new System.Drawing.Point(156, 59);
            this.bt_import.Name = "bt_import";
            this.bt_import.Size = new System.Drawing.Size(75, 23);
            this.bt_import.TabIndex = 0;
            this.bt_import.Text = "Import";
            this.bt_import.UseVisualStyleBackColor = true;
            this.bt_import.Click += new System.EventHandler(this.bt_import_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cb_mask_ratio);
            this.groupBox3.Controls.Add(this.nm_sprite_mask_radius);
            this.groupBox3.Controls.Add(this.lb_sprite_mask_radius);
            this.groupBox3.Controls.Add(this.lb_sprite_mask_height);
            this.groupBox3.Controls.Add(this.nm_sprite_mask_height);
            this.groupBox3.Controls.Add(this.cb_show_sprite_mask);
            this.groupBox3.Controls.Add(this.lb_sprite_mask_width);
            this.groupBox3.Controls.Add(this.nm_sprite_mask_width);
            this.groupBox3.Controls.Add(this.rb_mask_circle);
            this.groupBox3.Controls.Add(this.rb_mask_rect);
            this.groupBox3.Controls.Add(this.rb_mask_none);
            this.groupBox3.Location = new System.Drawing.Point(7, 274);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(236, 111);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sprite mask";
            // 
            // cb_mask_ratio
            // 
            this.cb_mask_ratio.AutoSize = true;
            this.cb_mask_ratio.Location = new System.Drawing.Point(150, 82);
            this.cb_mask_ratio.Name = "cb_mask_ratio";
            this.cb_mask_ratio.Size = new System.Drawing.Size(62, 19);
            this.cb_mask_ratio.TabIndex = 10;
            this.cb_mask_ratio.Text = "Square";
            this.cb_mask_ratio.UseVisualStyleBackColor = true;
            // 
            // nm_sprite_mask_radius
            // 
            this.nm_sprite_mask_radius.Location = new System.Drawing.Point(144, 82);
            this.nm_sprite_mask_radius.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.nm_sprite_mask_radius.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nm_sprite_mask_radius.Name = "nm_sprite_mask_radius";
            this.nm_sprite_mask_radius.Size = new System.Drawing.Size(82, 23);
            this.nm_sprite_mask_radius.TabIndex = 9;
            this.nm_sprite_mask_radius.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nm_sprite_mask_radius.ValueChanged += new System.EventHandler(this.nm_sprite_mask_radius_ValueChanged);
            // 
            // lb_sprite_mask_radius
            // 
            this.lb_sprite_mask_radius.AutoSize = true;
            this.lb_sprite_mask_radius.Location = new System.Drawing.Point(99, 84);
            this.lb_sprite_mask_radius.Name = "lb_sprite_mask_radius";
            this.lb_sprite_mask_radius.Size = new System.Drawing.Size(42, 15);
            this.lb_sprite_mask_radius.TabIndex = 8;
            this.lb_sprite_mask_radius.Text = "Radius";
            this.lb_sprite_mask_radius.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lb_sprite_mask_height
            // 
            this.lb_sprite_mask_height.AutoSize = true;
            this.lb_sprite_mask_height.Location = new System.Drawing.Point(95, 55);
            this.lb_sprite_mask_height.Name = "lb_sprite_mask_height";
            this.lb_sprite_mask_height.Size = new System.Drawing.Size(43, 15);
            this.lb_sprite_mask_height.TabIndex = 7;
            this.lb_sprite_mask_height.Text = "Height";
            this.lb_sprite_mask_height.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nm_sprite_mask_height
            // 
            this.nm_sprite_mask_height.Location = new System.Drawing.Point(144, 53);
            this.nm_sprite_mask_height.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.nm_sprite_mask_height.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nm_sprite_mask_height.Name = "nm_sprite_mask_height";
            this.nm_sprite_mask_height.Size = new System.Drawing.Size(82, 23);
            this.nm_sprite_mask_height.TabIndex = 6;
            this.nm_sprite_mask_height.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nm_sprite_mask_height.ValueChanged += new System.EventHandler(this.nm_sprite_mask_h_ValueChanged);
            // 
            // cb_show_sprite_mask
            // 
            this.cb_show_sprite_mask.AutoSize = true;
            this.cb_show_sprite_mask.Location = new System.Drawing.Point(150, 0);
            this.cb_show_sprite_mask.Name = "cb_show_sprite_mask";
            this.cb_show_sprite_mask.Size = new System.Drawing.Size(55, 19);
            this.cb_show_sprite_mask.TabIndex = 5;
            this.cb_show_sprite_mask.Text = "Show";
            this.cb_show_sprite_mask.UseVisualStyleBackColor = true;
            this.cb_show_sprite_mask.CheckedChanged += new System.EventHandler(this.cb_show_sprite_mask_CheckedChanged);
            // 
            // lb_sprite_mask_width
            // 
            this.lb_sprite_mask_width.AutoSize = true;
            this.lb_sprite_mask_width.Location = new System.Drawing.Point(99, 26);
            this.lb_sprite_mask_width.Name = "lb_sprite_mask_width";
            this.lb_sprite_mask_width.Size = new System.Drawing.Size(39, 15);
            this.lb_sprite_mask_width.TabIndex = 4;
            this.lb_sprite_mask_width.Text = "Width";
            // 
            // nm_sprite_mask_width
            // 
            this.nm_sprite_mask_width.Location = new System.Drawing.Point(144, 24);
            this.nm_sprite_mask_width.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.nm_sprite_mask_width.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nm_sprite_mask_width.Name = "nm_sprite_mask_width";
            this.nm_sprite_mask_width.Size = new System.Drawing.Size(82, 23);
            this.nm_sprite_mask_width.TabIndex = 3;
            this.nm_sprite_mask_width.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nm_sprite_mask_width.ValueChanged += new System.EventHandler(this.nm_sprite_mask_w_ValueChanged);
            // 
            // rb_mask_circle
            // 
            this.rb_mask_circle.AutoSize = true;
            this.rb_mask_circle.Location = new System.Drawing.Point(5, 69);
            this.rb_mask_circle.Name = "rb_mask_circle";
            this.rb_mask_circle.Size = new System.Drawing.Size(55, 19);
            this.rb_mask_circle.TabIndex = 2;
            this.rb_mask_circle.TabStop = true;
            this.rb_mask_circle.Text = "Circle";
            this.rb_mask_circle.UseVisualStyleBackColor = true;
            this.rb_mask_circle.CheckedChanged += new System.EventHandler(this.rb_mask_circle_CheckedChanged);
            // 
            // rb_mask_rect
            // 
            this.rb_mask_rect.AutoSize = true;
            this.rb_mask_rect.Location = new System.Drawing.Point(6, 44);
            this.rb_mask_rect.Name = "rb_mask_rect";
            this.rb_mask_rect.Size = new System.Drawing.Size(48, 19);
            this.rb_mask_rect.TabIndex = 1;
            this.rb_mask_rect.TabStop = true;
            this.rb_mask_rect.Text = "Rect";
            this.rb_mask_rect.UseVisualStyleBackColor = true;
            this.rb_mask_rect.CheckedChanged += new System.EventHandler(this.rb_mask_rect_CheckedChanged);
            // 
            // rb_mask_none
            // 
            this.rb_mask_none.AutoSize = true;
            this.rb_mask_none.Location = new System.Drawing.Point(6, 19);
            this.rb_mask_none.Name = "rb_mask_none";
            this.rb_mask_none.Size = new System.Drawing.Size(54, 19);
            this.rb_mask_none.TabIndex = 0;
            this.rb_mask_none.TabStop = true;
            this.rb_mask_none.Text = "None";
            this.rb_mask_none.UseVisualStyleBackColor = true;
            this.rb_mask_none.CheckedChanged += new System.EventHandler(this.rb_mask_none_CheckedChanged);
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.bt_as_delete);
            this.groupBox9.Controls.Add(this.bt_as_edit);
            this.groupBox9.Controls.Add(this.bt_as_add);
            this.groupBox9.Controls.Add(this.lb_animation_sequence);
            this.groupBox9.Location = new System.Drawing.Point(6, 391);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(237, 171);
            this.groupBox9.TabIndex = 9;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Animation sequence";
            // 
            // bt_as_delete
            // 
            this.bt_as_delete.Location = new System.Drawing.Point(178, 142);
            this.bt_as_delete.Name = "bt_as_delete";
            this.bt_as_delete.Size = new System.Drawing.Size(53, 23);
            this.bt_as_delete.TabIndex = 4;
            this.bt_as_delete.Text = "Delete";
            this.bt_as_delete.UseVisualStyleBackColor = true;
            this.bt_as_delete.Click += new System.EventHandler(this.bt_as_delete_Click);
            // 
            // bt_as_edit
            // 
            this.bt_as_edit.Location = new System.Drawing.Point(114, 142);
            this.bt_as_edit.Name = "bt_as_edit";
            this.bt_as_edit.Size = new System.Drawing.Size(58, 23);
            this.bt_as_edit.TabIndex = 3;
            this.bt_as_edit.Text = "Edit";
            this.bt_as_edit.UseVisualStyleBackColor = true;
            this.bt_as_edit.Click += new System.EventHandler(this.bt_as_edit_Click);
            // 
            // bt_as_add
            // 
            this.bt_as_add.Location = new System.Drawing.Point(55, 142);
            this.bt_as_add.Name = "bt_as_add";
            this.bt_as_add.Size = new System.Drawing.Size(53, 23);
            this.bt_as_add.TabIndex = 1;
            this.bt_as_add.Text = "Add";
            this.bt_as_add.UseVisualStyleBackColor = true;
            this.bt_as_add.Click += new System.EventHandler(this.bt_as_add_Click);
            // 
            // lb_animation_sequence
            // 
            this.lb_animation_sequence.FormattingEnabled = true;
            this.lb_animation_sequence.ItemHeight = 15;
            this.lb_animation_sequence.Location = new System.Drawing.Point(8, 19);
            this.lb_animation_sequence.Name = "lb_animation_sequence";
            this.lb_animation_sequence.Size = new System.Drawing.Size(218, 109);
            this.lb_animation_sequence.TabIndex = 0;
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Controls.Add(this.nm_center_h);
            this.groupBox6.Controls.Add(this.cb_show_sprite_center);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.nm_center_w);
            this.groupBox6.Controls.Add(this.rb_center_custom);
            this.groupBox6.Controls.Add(this.rb_center_center);
            this.groupBox6.Controls.Add(this.rb_center_00);
            this.groupBox6.Location = new System.Drawing.Point(6, 173);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(237, 95);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Sprite center";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(96, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(43, 15);
            this.label4.TabIndex = 7;
            this.label4.Text = "Height";
            // 
            // nm_center_h
            // 
            this.nm_center_h.Location = new System.Drawing.Point(145, 65);
            this.nm_center_h.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.nm_center_h.Name = "nm_center_h";
            this.nm_center_h.Size = new System.Drawing.Size(82, 23);
            this.nm_center_h.TabIndex = 6;
            this.nm_center_h.ValueChanged += new System.EventHandler(this.nm_center_h_ValueChanged);
            // 
            // cb_show_sprite_center
            // 
            this.cb_show_sprite_center.AutoSize = true;
            this.cb_show_sprite_center.Location = new System.Drawing.Point(150, 0);
            this.cb_show_sprite_center.Name = "cb_show_sprite_center";
            this.cb_show_sprite_center.Size = new System.Drawing.Size(55, 19);
            this.cb_show_sprite_center.TabIndex = 5;
            this.cb_show_sprite_center.Text = "Show";
            this.cb_show_sprite_center.UseVisualStyleBackColor = true;
            this.cb_show_sprite_center.CheckedChanged += new System.EventHandler(this.cb_show_sprite_center_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(100, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Width";
            // 
            // nm_center_w
            // 
            this.nm_center_w.Location = new System.Drawing.Point(145, 37);
            this.nm_center_w.Maximum = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.nm_center_w.Name = "nm_center_w";
            this.nm_center_w.Size = new System.Drawing.Size(82, 23);
            this.nm_center_w.TabIndex = 3;
            this.nm_center_w.ValueChanged += new System.EventHandler(this.nm_center_w_ValueChanged);
            // 
            // rb_center_custom
            // 
            this.rb_center_custom.AutoSize = true;
            this.rb_center_custom.Location = new System.Drawing.Point(6, 69);
            this.rb_center_custom.Name = "rb_center_custom";
            this.rb_center_custom.Size = new System.Drawing.Size(67, 19);
            this.rb_center_custom.TabIndex = 2;
            this.rb_center_custom.TabStop = true;
            this.rb_center_custom.Text = "Custom";
            this.rb_center_custom.UseVisualStyleBackColor = true;
            this.rb_center_custom.CheckedChanged += new System.EventHandler(this.rb_center_custom_CheckedChanged);
            // 
            // rb_center_center
            // 
            this.rb_center_center.AutoSize = true;
            this.rb_center_center.Location = new System.Drawing.Point(6, 44);
            this.rb_center_center.Name = "rb_center_center";
            this.rb_center_center.Size = new System.Drawing.Size(60, 19);
            this.rb_center_center.TabIndex = 1;
            this.rb_center_center.TabStop = true;
            this.rb_center_center.Text = "Center";
            this.rb_center_center.UseVisualStyleBackColor = true;
            this.rb_center_center.CheckedChanged += new System.EventHandler(this.rb_center_center_CheckedChanged);
            // 
            // rb_center_00
            // 
            this.rb_center_00.AutoSize = true;
            this.rb_center_00.Location = new System.Drawing.Point(6, 19);
            this.rb_center_00.Name = "rb_center_00";
            this.rb_center_00.Size = new System.Drawing.Size(40, 19);
            this.rb_center_00.TabIndex = 0;
            this.rb_center_00.TabStop = true;
            this.rb_center_00.Text = "0,0";
            this.rb_center_00.UseVisualStyleBackColor = true;
            this.rb_center_00.CheckedChanged += new System.EventHandler(this.rb_center_00_CheckedChanged);
            // 
            // groupBoxSpriteName
            // 
            this.groupBoxSpriteName.Controls.Add(this.tb_sprite_name);
            this.groupBoxSpriteName.Location = new System.Drawing.Point(6, 22);
            this.groupBoxSpriteName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxSpriteName.Name = "groupBoxSpriteName";
            this.groupBoxSpriteName.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBoxSpriteName.Size = new System.Drawing.Size(237, 51);
            this.groupBoxSpriteName.TabIndex = 0;
            this.groupBoxSpriteName.TabStop = false;
            this.groupBoxSpriteName.Text = "Sprite name";
            // 
            // tb_sprite_name
            // 
            this.tb_sprite_name.Location = new System.Drawing.Point(7, 22);
            this.tb_sprite_name.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.tb_sprite_name.Name = "tb_sprite_name";
            this.tb_sprite_name.Size = new System.Drawing.Size(219, 23);
            this.tb_sprite_name.TabIndex = 0;
            // 
            // sprite_preview
            // 
            this.sprite_preview.Location = new System.Drawing.Point(273, 18);
            this.sprite_preview.Name = "sprite_preview";
            this.sprite_preview.Size = new System.Drawing.Size(300, 300);
            this.sprite_preview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.sprite_preview.TabIndex = 2;
            this.sprite_preview.TabStop = false;
            this.sprite_preview.Paint += new System.Windows.Forms.PaintEventHandler(this.sprite_preview_Paint);
            // 
            // bt_accept
            // 
            this.bt_accept.Location = new System.Drawing.Point(485, 553);
            this.bt_accept.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bt_accept.Name = "bt_accept";
            this.bt_accept.Size = new System.Drawing.Size(88, 27);
            this.bt_accept.TabIndex = 7;
            this.bt_accept.Text = "Apply";
            this.bt_accept.UseVisualStyleBackColor = true;
            this.bt_accept.Click += new System.EventHandler(this.bt_accept_Click);
            // 
            // bt_cancel
            // 
            this.bt_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bt_cancel.Location = new System.Drawing.Point(389, 553);
            this.bt_cancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.bt_cancel.Name = "bt_cancel";
            this.bt_cancel.Size = new System.Drawing.Size(88, 27);
            this.bt_cancel.TabIndex = 8;
            this.bt_cancel.Text = "Cancel";
            this.bt_cancel.UseVisualStyleBackColor = true;
            this.bt_cancel.Click += new System.EventHandler(this.bt_cancel_Click);
            // 
            // control_box_region
            // 
            this.control_box_region.Controls.Add(this.tb_control_box_info);
            this.control_box_region.Controls.Add(this.cb_loop);
            this.control_box_region.Controls.Add(this.bt_last);
            this.control_box_region.Controls.Add(this.bt_next);
            this.control_box_region.Controls.Add(this.bt_prev);
            this.control_box_region.Controls.Add(this.bt_first);
            this.control_box_region.Controls.Add(this.bt_stop);
            this.control_box_region.Controls.Add(this.bt_pause);
            this.control_box_region.Controls.Add(this.br_play);
            this.control_box_region.Controls.Add(this.groupBox5);
            this.control_box_region.Controls.Add(this.groupBox10);
            this.control_box_region.Enabled = false;
            this.control_box_region.Location = new System.Drawing.Point(273, 324);
            this.control_box_region.Name = "control_box_region";
            this.control_box_region.Size = new System.Drawing.Size(300, 131);
            this.control_box_region.TabIndex = 10;
            this.control_box_region.TabStop = false;
            this.control_box_region.Text = "Control box";
            // 
            // tb_control_box_info
            // 
            this.tb_control_box_info.Location = new System.Drawing.Point(6, 100);
            this.tb_control_box_info.Name = "tb_control_box_info";
            this.tb_control_box_info.ReadOnly = true;
            this.tb_control_box_info.Size = new System.Drawing.Size(288, 23);
            this.tb_control_box_info.TabIndex = 21;
            this.tb_control_box_info.TabStop = false;
            this.tb_control_box_info.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cb_loop
            // 
            this.cb_loop.AutoSize = true;
            this.cb_loop.Location = new System.Drawing.Point(237, 73);
            this.cb_loop.Name = "cb_loop";
            this.cb_loop.Size = new System.Drawing.Size(53, 19);
            this.cb_loop.TabIndex = 20;
            this.cb_loop.Text = "Loop";
            this.cb_loop.UseVisualStyleBackColor = true;
            this.cb_loop.CheckedChanged += new System.EventHandler(this.cb_loop_CheckedChanged);
            // 
            // bt_last
            // 
            this.bt_last.Location = new System.Drawing.Point(198, 68);
            this.bt_last.Name = "bt_last";
            this.bt_last.Size = new System.Drawing.Size(26, 26);
            this.bt_last.TabIndex = 19;
            this.bt_last.UseVisualStyleBackColor = true;
            this.bt_last.Click += new System.EventHandler(this.bt_last_Click);
            this.bt_last.MouseEnter += new System.EventHandler(this.bt_last_MouseEnter);
            // 
            // bt_next
            // 
            this.bt_next.Location = new System.Drawing.Point(166, 68);
            this.bt_next.Name = "bt_next";
            this.bt_next.Size = new System.Drawing.Size(26, 26);
            this.bt_next.TabIndex = 18;
            this.bt_next.UseVisualStyleBackColor = true;
            this.bt_next.Click += new System.EventHandler(this.bt_next_Click);
            this.bt_next.MouseEnter += new System.EventHandler(this.bt_next_MouseEnter);
            // 
            // bt_prev
            // 
            this.bt_prev.Location = new System.Drawing.Point(134, 68);
            this.bt_prev.Name = "bt_prev";
            this.bt_prev.Size = new System.Drawing.Size(26, 26);
            this.bt_prev.TabIndex = 17;
            this.bt_prev.UseVisualStyleBackColor = true;
            this.bt_prev.Click += new System.EventHandler(this.bt_prev_Click);
            this.bt_prev.MouseEnter += new System.EventHandler(this.bt_prev_MouseEnter);
            // 
            // bt_first
            // 
            this.bt_first.Location = new System.Drawing.Point(102, 68);
            this.bt_first.Name = "bt_first";
            this.bt_first.Size = new System.Drawing.Size(26, 26);
            this.bt_first.TabIndex = 16;
            this.bt_first.UseVisualStyleBackColor = true;
            this.bt_first.Click += new System.EventHandler(this.bt_first_Click);
            this.bt_first.MouseEnter += new System.EventHandler(this.bt_first_MouseEnter);
            // 
            // bt_stop
            // 
            this.bt_stop.Location = new System.Drawing.Point(70, 68);
            this.bt_stop.Name = "bt_stop";
            this.bt_stop.Size = new System.Drawing.Size(26, 26);
            this.bt_stop.TabIndex = 15;
            this.bt_stop.UseVisualStyleBackColor = true;
            this.bt_stop.Click += new System.EventHandler(this.bt_stop_Click);
            this.bt_stop.MouseEnter += new System.EventHandler(this.bt_stop_MouseEnter);
            // 
            // bt_pause
            // 
            this.bt_pause.Location = new System.Drawing.Point(38, 68);
            this.bt_pause.Name = "bt_pause";
            this.bt_pause.Size = new System.Drawing.Size(26, 26);
            this.bt_pause.TabIndex = 14;
            this.bt_pause.UseVisualStyleBackColor = true;
            this.bt_pause.Click += new System.EventHandler(this.bt_pause_Click);
            this.bt_pause.MouseEnter += new System.EventHandler(this.bt_pause_MouseEnter);
            // 
            // br_play
            // 
            this.br_play.Location = new System.Drawing.Point(6, 68);
            this.br_play.Name = "br_play";
            this.br_play.Size = new System.Drawing.Size(26, 26);
            this.br_play.TabIndex = 13;
            this.br_play.UseVisualStyleBackColor = true;
            this.br_play.Click += new System.EventHandler(this.br_play_Click);
            this.br_play.MouseEnter += new System.EventHandler(this.br_play_MouseEnter);
            this.br_play.MouseLeave += new System.EventHandler(this.ControlBoxMouseLeave);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.nm_fps);
            this.groupBox5.Location = new System.Drawing.Point(237, 19);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(57, 43);
            this.groupBox5.TabIndex = 12;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "FPS";
            // 
            // nm_fps
            // 
            this.nm_fps.Location = new System.Drawing.Point(6, 16);
            this.nm_fps.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.nm_fps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nm_fps.Name = "nm_fps";
            this.nm_fps.Size = new System.Drawing.Size(43, 23);
            this.nm_fps.TabIndex = 0;
            this.nm_fps.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.nm_fps.ValueChanged += new System.EventHandler(this.nm_fps_ValueChanged);
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.ls_animation_sequence);
            this.groupBox10.Location = new System.Drawing.Point(6, 19);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(225, 43);
            this.groupBox10.TabIndex = 11;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Play sequence";
            // 
            // ls_animation_sequence
            // 
            this.ls_animation_sequence.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ls_animation_sequence.FormattingEnabled = true;
            this.ls_animation_sequence.Items.AddRange(new object[] {
            "<All frames>"});
            this.ls_animation_sequence.Location = new System.Drawing.Point(6, 16);
            this.ls_animation_sequence.Name = "ls_animation_sequence";
            this.ls_animation_sequence.Size = new System.Drawing.Size(213, 23);
            this.ls_animation_sequence.TabIndex = 9;
            this.ls_animation_sequence.SelectedIndexChanged += new System.EventHandler(this.ls_animation_sequence_SelectedIndexChanged);
            // 
            // lb_preview_info
            // 
            this.lb_preview_info.AutoSize = true;
            this.lb_preview_info.Location = new System.Drawing.Point(273, 18);
            this.lb_preview_info.Name = "lb_preview_info";
            this.lb_preview_info.Size = new System.Drawing.Size(0, 15);
            this.lb_preview_info.TabIndex = 11;
            // 
            // update_preview_timer
            // 
            this.update_preview_timer.Tick += new System.EventHandler(this.update_preview_Tick);
            // 
            // SpriteManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 589);
            this.Controls.Add(this.lb_preview_info);
            this.Controls.Add(this.control_box_region);
            this.Controls.Add(this.bt_cancel);
            this.Controls.Add(this.bt_accept);
            this.Controls.Add(this.sprite_preview);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SpriteManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SpriteManager";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpriteManager_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nm_sprite_mask_radius)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nm_sprite_mask_height)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nm_sprite_mask_width)).EndInit();
            this.groupBox9.ResumeLayout(false);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nm_center_h)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nm_center_w)).EndInit();
            this.groupBoxSpriteName.ResumeLayout(false);
            this.groupBoxSpriteName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sprite_preview)).EndInit();
            this.control_box_region.ResumeLayout(false);
            this.control_box_region.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nm_fps)).EndInit();
            this.groupBox10.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lb_sprite_mask_height;
        private System.Windows.Forms.NumericUpDown nm_sprite_mask_height;
        private System.Windows.Forms.CheckBox cb_show_sprite_mask;
        private System.Windows.Forms.Label lb_sprite_mask_width;
        private System.Windows.Forms.NumericUpDown nm_sprite_mask_width;
        private System.Windows.Forms.RadioButton rb_mask_circle;
        private System.Windows.Forms.RadioButton rb_mask_rect;
        private System.Windows.Forms.RadioButton rb_mask_none;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nm_center_h;
        private System.Windows.Forms.CheckBox cb_show_sprite_center;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nm_center_w;
        private System.Windows.Forms.RadioButton rb_center_custom;
        private System.Windows.Forms.RadioButton rb_center_center;
        private System.Windows.Forms.RadioButton rb_center_00;
        private System.Windows.Forms.GroupBox groupBoxSpriteName;
        private System.Windows.Forms.TextBox tb_sprite_name;
        private System.Windows.Forms.PictureBox sprite_preview;
        private System.Windows.Forms.Button bt_accept;
        private System.Windows.Forms.Button bt_cancel;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button bt_as_delete;
        private System.Windows.Forms.Button bt_as_edit;
        private System.Windows.Forms.Button bt_as_add;
        private System.Windows.Forms.ListBox lb_animation_sequence;
        private System.Windows.Forms.GroupBox control_box_region;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.ComboBox ls_animation_sequence;
        private System.Windows.Forms.Button bt_first;
        private System.Windows.Forms.Button bt_stop;
        private System.Windows.Forms.Button bt_pause;
        private System.Windows.Forms.Button br_play;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.NumericUpDown nm_fps;
        private System.Windows.Forms.TextBox tb_control_box_info;
        private System.Windows.Forms.CheckBox cb_loop;
        private System.Windows.Forms.Button bt_last;
        private System.Windows.Forms.Button bt_next;
        private System.Windows.Forms.Button bt_prev;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lb_sprite_info;
        private System.Windows.Forms.Button bt_import;
        private System.Windows.Forms.Label lb_preview_info;
        private System.Windows.Forms.Timer update_preview_timer;
        private System.Windows.Forms.NumericUpDown nm_sprite_mask_radius;
        private System.Windows.Forms.Label lb_sprite_mask_radius;
        private System.Windows.Forms.CheckBox cb_mask_ratio;
    }
}