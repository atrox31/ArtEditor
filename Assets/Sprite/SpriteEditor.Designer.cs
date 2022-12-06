namespace ArtCore_Editor.Assets.Sprite
{
    partial class SpriteEditor
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.s_spritename = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox10 = new System.Windows.Forms.GroupBox();
            this.s_animationSequencePreview = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.s_preview_prev = new System.Windows.Forms.Button();
            this.s_preview_next = new System.Windows.Forms.Button();
            this.s_preview_loop = new System.Windows.Forms.CheckBox();
            this.s_preview_stop = new System.Windows.Forms.Button();
            this.s_preview_play = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.s_preview_fps = new System.Windows.Forms.NumericUpDown();
            this.s_preview = new System.Windows.Forms.PictureBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.s_sprite_center_show = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.s_sprite_center_y = new System.Windows.Forms.NumericUpDown();
            this.s_aprite_center_x = new System.Windows.Forms.NumericUpDown();
            this.s_sprite_center_custom = new System.Windows.Forms.RadioButton();
            this.s_sprite_center_left = new System.Windows.Forms.RadioButton();
            this.s_sprite_center_center = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.s_col_mask_show = new System.Windows.Forms.CheckBox();
            this.s_collision_have_mask = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.s_col_mask_value = new System.Windows.Forms.TrackBar();
            this.s_col_mask_perpixel = new System.Windows.Forms.RadioButton();
            this.s_col_mask_rect = new System.Windows.Forms.RadioButton();
            this.s_col_mask_circle = new System.Windows.Forms.RadioButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox10.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.s_preview_fps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_preview)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.s_sprite_center_y)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_aprite_center_x)).BeginInit();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.s_col_mask_value)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "png";
            this.openFileDialog1.Filter = "PNG|*.png";
            this.openFileDialog1.Multiselect = true;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.LightSteelBlue;
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.groupBox9);
            this.groupBox1.Controls.Add(this.groupBox7);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 377);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Properties";
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.button5);
            this.groupBox9.Controls.Add(this.button6);
            this.groupBox9.Controls.Add(this.button4);
            this.groupBox9.Controls.Add(this.listBox1);
            this.groupBox9.Location = new System.Drawing.Point(6, 144);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(188, 171);
            this.groupBox9.TabIndex = 4;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Animation sequence";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label6.Location = new System.Drawing.Point(9, 312);
            this.label6.MinimumSize = new System.Drawing.Size(180, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(180, 17);
            this.label6.TabIndex = 5;
            this.label6.Text = "not implemented yet!";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(129, 142);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(53, 23);
            this.button5.TabIndex = 4;
            this.button5.Text = "Delete";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(65, 142);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(58, 23);
            this.button6.TabIndex = 3;
            this.button6.Text = "Edit";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(6, 142);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(53, 23);
            this.button4.TabIndex = 1;
            this.button4.Text = "Add";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Location = new System.Drawing.Point(6, 19);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(176, 109);
            this.listBox1.TabIndex = 0;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.button2);
            this.groupBox7.Controls.Add(this.button1);
            this.groupBox7.Controls.Add(this.label4);
            this.groupBox7.Location = new System.Drawing.Point(6, 74);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(188, 64);
            this.groupBox7.TabIndex = 3;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Textures";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(107, 32);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Import";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 32);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Show";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 15);
            this.label4.TabIndex = 0;
            this.label4.Text = "Current: 0";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.s_spritename);
            this.groupBox3.Location = new System.Drawing.Point(6, 19);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(188, 49);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Sprite name";
            // 
            // s_spritename
            // 
            this.s_spritename.Location = new System.Drawing.Point(6, 19);
            this.s_spritename.Name = "s_spritename";
            this.s_spritename.Size = new System.Drawing.Size(176, 23);
            this.s_spritename.TabIndex = 0;
            this.s_spritename.MouseEnter += new System.EventHandler(this.s_spritename_MouseEnter);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox10);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.button3);
            this.groupBox2.Controls.Add(this.s_preview_prev);
            this.groupBox2.Controls.Add(this.s_preview_next);
            this.groupBox2.Controls.Add(this.s_preview_loop);
            this.groupBox2.Controls.Add(this.s_preview_stop);
            this.groupBox2.Controls.Add(this.s_preview_play);
            this.groupBox2.Controls.Add(this.groupBox4);
            this.groupBox2.Controls.Add(this.s_preview);
            this.groupBox2.Location = new System.Drawing.Point(209, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(269, 377);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Preview";
            // 
            // groupBox10
            // 
            this.groupBox10.Controls.Add(this.s_animationSequencePreview);
            this.groupBox10.Location = new System.Drawing.Point(9, 333);
            this.groupBox10.Name = "groupBox10";
            this.groupBox10.Size = new System.Drawing.Size(245, 43);
            this.groupBox10.TabIndex = 10;
            this.groupBox10.TabStop = false;
            this.groupBox10.Text = "Play sequence";
            // 
            // s_animationSequencePreview
            // 
            this.s_animationSequencePreview.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.s_animationSequencePreview.FormattingEnabled = true;
            this.s_animationSequencePreview.Items.AddRange(new object[] {
            "<All frames>"});
            this.s_animationSequencePreview.Location = new System.Drawing.Point(6, 16);
            this.s_animationSequencePreview.Name = "s_animationSequencePreview";
            this.s_animationSequencePreview.Size = new System.Drawing.Size(233, 23);
            this.s_animationSequencePreview.TabIndex = 9;
            this.s_animationSequencePreview.SelectedIndexChanged += new System.EventHandler(this.s_animationSequencePreview_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "0/0";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(126, 304);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(54, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "Reset";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // s_preview_prev
            // 
            this.s_preview_prev.Location = new System.Drawing.Point(66, 304);
            this.s_preview_prev.Name = "s_preview_prev";
            this.s_preview_prev.Size = new System.Drawing.Size(54, 23);
            this.s_preview_prev.TabIndex = 6;
            this.s_preview_prev.Text = "Prev";
            this.s_preview_prev.UseVisualStyleBackColor = true;
            this.s_preview_prev.Click += new System.EventHandler(this.s_preview_prev_Click);
            // 
            // s_preview_next
            // 
            this.s_preview_next.Location = new System.Drawing.Point(66, 275);
            this.s_preview_next.Name = "s_preview_next";
            this.s_preview_next.Size = new System.Drawing.Size(54, 23);
            this.s_preview_next.TabIndex = 5;
            this.s_preview_next.Text = "Next";
            this.s_preview_next.UseVisualStyleBackColor = true;
            this.s_preview_next.Click += new System.EventHandler(this.s_preview_next_Click);
            // 
            // s_preview_loop
            // 
            this.s_preview_loop.AutoSize = true;
            this.s_preview_loop.Location = new System.Drawing.Point(126, 278);
            this.s_preview_loop.Name = "s_preview_loop";
            this.s_preview_loop.Size = new System.Drawing.Size(53, 19);
            this.s_preview_loop.TabIndex = 4;
            this.s_preview_loop.Text = "Loop";
            this.s_preview_loop.UseVisualStyleBackColor = true;
            this.s_preview_loop.CheckedChanged += new System.EventHandler(this.s_preview_loop_CheckedChanged);
            // 
            // s_preview_stop
            // 
            this.s_preview_stop.Enabled = false;
            this.s_preview_stop.Location = new System.Drawing.Point(6, 304);
            this.s_preview_stop.Name = "s_preview_stop";
            this.s_preview_stop.Size = new System.Drawing.Size(54, 23);
            this.s_preview_stop.TabIndex = 3;
            this.s_preview_stop.Text = "Stop";
            this.s_preview_stop.UseVisualStyleBackColor = true;
            this.s_preview_stop.Click += new System.EventHandler(this.s_preview_stop_Click);
            // 
            // s_preview_play
            // 
            this.s_preview_play.Location = new System.Drawing.Point(6, 276);
            this.s_preview_play.Name = "s_preview_play";
            this.s_preview_play.Size = new System.Drawing.Size(54, 23);
            this.s_preview_play.TabIndex = 2;
            this.s_preview_play.Text = "Play";
            this.s_preview_play.UseVisualStyleBackColor = true;
            this.s_preview_play.Click += new System.EventHandler(this.s_preview_play_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.s_preview_fps);
            this.groupBox4.Location = new System.Drawing.Point(190, 278);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(70, 49);
            this.groupBox4.TabIndex = 1;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "FPS";
            // 
            // s_preview_fps
            // 
            this.s_preview_fps.Location = new System.Drawing.Point(7, 20);
            this.s_preview_fps.Maximum = new decimal(new int[] {
            120,
            0,
            0,
            0});
            this.s_preview_fps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.s_preview_fps.Name = "s_preview_fps";
            this.s_preview_fps.Size = new System.Drawing.Size(57, 23);
            this.s_preview_fps.TabIndex = 0;
            this.s_preview_fps.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.s_preview_fps.ValueChanged += new System.EventHandler(this.s_preview_fps_ValueChanged);
            // 
            // s_preview
            // 
            this.s_preview.Location = new System.Drawing.Point(6, 19);
            this.s_preview.Name = "s_preview";
            this.s_preview.Size = new System.Drawing.Size(254, 254);
            this.s_preview.TabIndex = 0;
            this.s_preview.TabStop = false;
            this.s_preview.Paint += new System.Windows.Forms.PaintEventHandler(this.s_preview_Paint);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.s_sprite_center_show);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.s_sprite_center_y);
            this.groupBox5.Controls.Add(this.s_aprite_center_x);
            this.groupBox5.Controls.Add(this.s_sprite_center_custom);
            this.groupBox5.Controls.Add(this.s_sprite_center_left);
            this.groupBox5.Controls.Add(this.s_sprite_center_center);
            this.groupBox5.Location = new System.Drawing.Point(298, 386);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(180, 93);
            this.groupBox5.TabIndex = 2;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Sprite center";
            // 
            // s_sprite_center_show
            // 
            this.s_sprite_center_show.AutoSize = true;
            this.s_sprite_center_show.Location = new System.Drawing.Point(121, 70);
            this.s_sprite_center_show.Name = "s_sprite_center_show";
            this.s_sprite_center_show.Size = new System.Drawing.Size(55, 19);
            this.s_sprite_center_show.TabIndex = 8;
            this.s_sprite_center_show.Text = "Show";
            this.s_sprite_center_show.UseVisualStyleBackColor = true;
            this.s_sprite_center_show.CheckedChanged += new System.EventHandler(this.s_sprite_center_show_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(89, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(19, 18);
            this.label1.TabIndex = 7;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(89, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "Y";
            // 
            // s_sprite_center_y
            // 
            this.s_sprite_center_y.Location = new System.Drawing.Point(114, 46);
            this.s_sprite_center_y.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.s_sprite_center_y.Name = "s_sprite_center_y";
            this.s_sprite_center_y.Size = new System.Drawing.Size(57, 23);
            this.s_sprite_center_y.TabIndex = 4;
            this.s_sprite_center_y.ValueChanged += new System.EventHandler(this.s_sprite_center_y_ValueChanged);
            // 
            // s_aprite_center_x
            // 
            this.s_aprite_center_x.Location = new System.Drawing.Point(114, 20);
            this.s_aprite_center_x.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.s_aprite_center_x.Name = "s_aprite_center_x";
            this.s_aprite_center_x.Size = new System.Drawing.Size(57, 23);
            this.s_aprite_center_x.TabIndex = 3;
            this.s_aprite_center_x.ValueChanged += new System.EventHandler(this.s_sprite_center_x_ValueChanged);
            // 
            // s_sprite_center_custom
            // 
            this.s_sprite_center_custom.AutoSize = true;
            this.s_sprite_center_custom.Location = new System.Drawing.Point(9, 66);
            this.s_sprite_center_custom.Name = "s_sprite_center_custom";
            this.s_sprite_center_custom.Size = new System.Drawing.Size(67, 19);
            this.s_sprite_center_custom.TabIndex = 2;
            this.s_sprite_center_custom.TabStop = true;
            this.s_sprite_center_custom.Text = "Custom";
            this.s_sprite_center_custom.UseVisualStyleBackColor = true;
            this.s_sprite_center_custom.CheckedChanged += new System.EventHandler(this.s_sprite_center_custom_CheckedChanged);
            // 
            // s_sprite_center_left
            // 
            this.s_sprite_center_left.AutoSize = true;
            this.s_sprite_center_left.Location = new System.Drawing.Point(9, 43);
            this.s_sprite_center_left.Name = "s_sprite_center_left";
            this.s_sprite_center_left.Size = new System.Drawing.Size(82, 19);
            this.s_sprite_center_left.TabIndex = 1;
            this.s_sprite_center_left.TabStop = true;
            this.s_sprite_center_left.Text = "Left corner";
            this.s_sprite_center_left.UseVisualStyleBackColor = true;
            this.s_sprite_center_left.CheckedChanged += new System.EventHandler(this.s_sprite_center_left_CheckedChanged);
            // 
            // s_sprite_center_center
            // 
            this.s_sprite_center_center.AutoSize = true;
            this.s_sprite_center_center.Location = new System.Drawing.Point(9, 20);
            this.s_sprite_center_center.Name = "s_sprite_center_center";
            this.s_sprite_center_center.Size = new System.Drawing.Size(60, 19);
            this.s_sprite_center_center.TabIndex = 0;
            this.s_sprite_center_center.TabStop = true;
            this.s_sprite_center_center.Text = "Center";
            this.s_sprite_center_center.UseVisualStyleBackColor = true;
            this.s_sprite_center_center.CheckedChanged += new System.EventHandler(this.s_sprite_center_center_CheckedChanged);
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.s_col_mask_show);
            this.groupBox6.Controls.Add(this.s_collision_have_mask);
            this.groupBox6.Controls.Add(this.label3);
            this.groupBox6.Controls.Add(this.s_col_mask_value);
            this.groupBox6.Controls.Add(this.s_col_mask_perpixel);
            this.groupBox6.Controls.Add(this.s_col_mask_rect);
            this.groupBox6.Controls.Add(this.s_col_mask_circle);
            this.groupBox6.Location = new System.Drawing.Point(3, 385);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(289, 93);
            this.groupBox6.TabIndex = 3;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Collision mask";
            // 
            // s_col_mask_show
            // 
            this.s_col_mask_show.AutoSize = true;
            this.s_col_mask_show.Enabled = false;
            this.s_col_mask_show.Location = new System.Drawing.Point(144, 65);
            this.s_col_mask_show.Name = "s_col_mask_show";
            this.s_col_mask_show.Size = new System.Drawing.Size(55, 19);
            this.s_col_mask_show.TabIndex = 9;
            this.s_col_mask_show.Text = "Show";
            this.s_col_mask_show.UseVisualStyleBackColor = true;
            this.s_col_mask_show.CheckedChanged += new System.EventHandler(this.s_col_mask_show_CheckedChanged);
            // 
            // s_collision_have_mask
            // 
            this.s_collision_have_mask.AutoSize = true;
            this.s_collision_have_mask.Location = new System.Drawing.Point(203, 65);
            this.s_collision_have_mask.Name = "s_collision_have_mask";
            this.s_collision_have_mask.Size = new System.Drawing.Size(84, 19);
            this.s_collision_have_mask.TabIndex = 6;
            this.s_collision_have_mask.Text = "Have mask";
            this.s_collision_have_mask.UseVisualStyleBackColor = true;
            this.s_collision_have_mask.CheckedChanged += new System.EventHandler(this.s_collision_have_mask_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(165, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 15);
            this.label3.TabIndex = 5;
            this.label3.Text = "Value";
            // 
            // s_col_mask_value
            // 
            this.s_col_mask_value.Enabled = false;
            this.s_col_mask_value.LargeChange = 1;
            this.s_col_mask_value.Location = new System.Drawing.Point(86, 37);
            this.s_col_mask_value.Maximum = 100;
            this.s_col_mask_value.Name = "s_col_mask_value";
            this.s_col_mask_value.Size = new System.Drawing.Size(197, 45);
            this.s_col_mask_value.TabIndex = 4;
            this.s_col_mask_value.Scroll += new System.EventHandler(this.s_col_mask_value_Scroll);
            // 
            // s_col_mask_perpixel
            // 
            this.s_col_mask_perpixel.AutoSize = true;
            this.s_col_mask_perpixel.Enabled = false;
            this.s_col_mask_perpixel.Location = new System.Drawing.Point(6, 65);
            this.s_col_mask_perpixel.Name = "s_col_mask_perpixel";
            this.s_col_mask_perpixel.Size = new System.Drawing.Size(67, 19);
            this.s_col_mask_perpixel.TabIndex = 3;
            this.s_col_mask_perpixel.TabStop = true;
            this.s_col_mask_perpixel.Text = "PerPixel";
            this.s_col_mask_perpixel.UseVisualStyleBackColor = true;
            this.s_col_mask_perpixel.CheckedChanged += new System.EventHandler(this.s_col_mask_perpixel_CheckedChanged);
            // 
            // s_col_mask_rect
            // 
            this.s_col_mask_rect.AutoSize = true;
            this.s_col_mask_rect.Enabled = false;
            this.s_col_mask_rect.Location = new System.Drawing.Point(6, 42);
            this.s_col_mask_rect.Name = "s_col_mask_rect";
            this.s_col_mask_rect.Size = new System.Drawing.Size(77, 19);
            this.s_col_mask_rect.TabIndex = 2;
            this.s_col_mask_rect.TabStop = true;
            this.s_col_mask_rect.Text = "Rectangle";
            this.s_col_mask_rect.UseVisualStyleBackColor = true;
            this.s_col_mask_rect.CheckedChanged += new System.EventHandler(this.s_col_mask_rect_CheckedChanged);
            // 
            // s_col_mask_circle
            // 
            this.s_col_mask_circle.AutoSize = true;
            this.s_col_mask_circle.Enabled = false;
            this.s_col_mask_circle.Location = new System.Drawing.Point(6, 19);
            this.s_col_mask_circle.Name = "s_col_mask_circle";
            this.s_col_mask_circle.Size = new System.Drawing.Size(55, 19);
            this.s_col_mask_circle.TabIndex = 1;
            this.s_col_mask_circle.TabStop = true;
            this.s_col_mask_circle.Text = "Circle";
            this.s_col_mask_circle.UseVisualStyleBackColor = true;
            this.s_col_mask_circle.CheckedChanged += new System.EventHandler(this.s_col_mask_circle_CheckedChanged);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(484, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::ArtCore_Editor.Properties.Resources.disk;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox5);
            this.panel1.Controls.Add(this.groupBox6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(484, 482);
            this.panel1.TabIndex = 5;
            // 
            // SpriteEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(484, 507);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SpriteEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sprite";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SpriteAddForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox9.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox10.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.s_preview_fps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_preview)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.s_sprite_center_y)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.s_aprite_center_x)).EndInit();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.s_col_mask_value)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox s_spritename;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button s_preview_prev;
        private System.Windows.Forms.Button s_preview_next;
        private System.Windows.Forms.CheckBox s_preview_loop;
        private System.Windows.Forms.Button s_preview_stop;
        private System.Windows.Forms.Button s_preview_play;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.NumericUpDown s_preview_fps;
        private System.Windows.Forms.PictureBox s_preview;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown s_sprite_center_y;
        private System.Windows.Forms.NumericUpDown s_aprite_center_x;
        private System.Windows.Forms.RadioButton s_sprite_center_custom;
        private System.Windows.Forms.RadioButton s_sprite_center_left;
        private System.Windows.Forms.RadioButton s_sprite_center_center;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar s_col_mask_value;
        private System.Windows.Forms.RadioButton s_col_mask_perpixel;
        private System.Windows.Forms.RadioButton s_col_mask_rect;
        private System.Windows.Forms.RadioButton s_col_mask_circle;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.CheckBox s_sprite_center_show;
        private System.Windows.Forms.CheckBox s_col_mask_show;
        private System.Windows.Forms.CheckBox s_collision_have_mask;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.GroupBox groupBox10;
        private System.Windows.Forms.ComboBox s_animationSequencePreview;
        private System.Windows.Forms.Label label6;
    }
}