namespace ArtCore_Editor
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.spriteRenderer = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ok_button = new System.Windows.Forms.Button();
            this.cancel_button = new System.Windows.Forms.Button();
            this.btn_play = new System.Windows.Forms.Button();
            this.btn_prev = new System.Windows.Forms.Button();
            this.btn_next = new System.Windows.Forms.Button();
            this.btn_stop = new System.Windows.Forms.Button();
            this.btn_first = new System.Windows.Forms.Button();
            this.btn_last = new System.Windows.Forms.Button();
            this.chk_loop = new System.Windows.Forms.CheckBox();
            this.fps_value = new System.Windows.Forms.NumericUpDown();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spriteRenderer)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fps_value)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.spriteRenderer, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 67.16918F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 32.83082F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1118, 735);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // spriteRenderer
            // 
            this.spriteRenderer.BackColor = System.Drawing.Color.White;
            this.spriteRenderer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spriteRenderer.Location = new System.Drawing.Point(562, 3);
            this.spriteRenderer.Name = "spriteRenderer";
            this.spriteRenderer.Size = new System.Drawing.Size(553, 487);
            this.spriteRenderer.TabIndex = 0;
            this.spriteRenderer.TabStop = false;
            this.spriteRenderer.DoubleClick += new System.EventHandler(this.spriteRenderer_DoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cancel_button);
            this.panel1.Controls.Add(this.ok_button);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(562, 496);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(553, 236);
            this.panel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(541, 77);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Controls";
            // 
            // ok_button
            // 
            this.ok_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ok_button.Location = new System.Drawing.Point(469, 204);
            this.ok_button.Name = "ok_button";
            this.ok_button.Size = new System.Drawing.Size(75, 23);
            this.ok_button.TabIndex = 1;
            this.ok_button.Text = "Apply";
            this.ok_button.UseVisualStyleBackColor = true;
            // 
            // cancel_button
            // 
            this.cancel_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancel_button.Location = new System.Drawing.Point(388, 204);
            this.cancel_button.Name = "cancel_button";
            this.cancel_button.Size = new System.Drawing.Size(75, 23);
            this.cancel_button.TabIndex = 2;
            this.cancel_button.Text = "Cancel";
            this.cancel_button.UseVisualStyleBackColor = true;
            // 
            // btn_play
            // 
            this.btn_play.Location = new System.Drawing.Point(3, 3);
            this.btn_play.Name = "btn_play";
            this.btn_play.Size = new System.Drawing.Size(75, 23);
            this.btn_play.TabIndex = 0;
            this.btn_play.Text = "Play";
            this.btn_play.UseVisualStyleBackColor = true;
            // 
            // btn_prev
            // 
            this.btn_prev.Location = new System.Drawing.Point(84, 3);
            this.btn_prev.Name = "btn_prev";
            this.btn_prev.Size = new System.Drawing.Size(75, 23);
            this.btn_prev.TabIndex = 1;
            this.btn_prev.Text = "Prev";
            this.btn_prev.UseVisualStyleBackColor = true;
            // 
            // btn_next
            // 
            this.btn_next.Location = new System.Drawing.Point(165, 3);
            this.btn_next.Name = "btn_next";
            this.btn_next.Size = new System.Drawing.Size(75, 23);
            this.btn_next.TabIndex = 2;
            this.btn_next.Text = "Next";
            this.btn_next.UseVisualStyleBackColor = true;
            // 
            // btn_stop
            // 
            this.btn_stop.Location = new System.Drawing.Point(3, 32);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(75, 23);
            this.btn_stop.TabIndex = 3;
            this.btn_stop.Text = "Stop";
            this.btn_stop.UseVisualStyleBackColor = true;
            // 
            // btn_first
            // 
            this.btn_first.Location = new System.Drawing.Point(84, 32);
            this.btn_first.Name = "btn_first";
            this.btn_first.Size = new System.Drawing.Size(75, 23);
            this.btn_first.TabIndex = 4;
            this.btn_first.Text = "First";
            this.btn_first.UseVisualStyleBackColor = true;
            // 
            // btn_last
            // 
            this.btn_last.Location = new System.Drawing.Point(165, 32);
            this.btn_last.Name = "btn_last";
            this.btn_last.Size = new System.Drawing.Size(75, 23);
            this.btn_last.TabIndex = 5;
            this.btn_last.Text = "Last";
            this.btn_last.UseVisualStyleBackColor = true;
            // 
            // chk_loop
            // 
            this.chk_loop.AutoSize = true;
            this.chk_loop.Checked = true;
            this.chk_loop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk_loop.Location = new System.Drawing.Point(246, 3);
            this.chk_loop.Name = "chk_loop";
            this.chk_loop.Size = new System.Drawing.Size(50, 17);
            this.chk_loop.TabIndex = 6;
            this.chk_loop.Text = "Loop";
            this.chk_loop.UseVisualStyleBackColor = true;
            // 
            // fps_value
            // 
            this.fps_value.Location = new System.Drawing.Point(246, 32);
            this.fps_value.Maximum = new decimal(new int[] {
            144,
            0,
            0,
            0});
            this.fps_value.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.fps_value.Name = "fps_value";
            this.fps_value.Size = new System.Drawing.Size(72, 20);
            this.fps_value.TabIndex = 7;
            this.fps_value.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.flowLayoutPanel1.Controls.Add(this.btn_play);
            this.flowLayoutPanel1.Controls.Add(this.btn_prev);
            this.flowLayoutPanel1.Controls.Add(this.btn_next);
            this.flowLayoutPanel1.Controls.Add(this.chk_loop);
            this.flowLayoutPanel1.Controls.Add(this.btn_stop);
            this.flowLayoutPanel1.Controls.Add(this.btn_first);
            this.flowLayoutPanel1.Controls.Add(this.btn_last);
            this.flowLayoutPanel1.Controls.Add(this.fps_value);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(105, 11);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(325, 60);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.groupBox2);
            this.flowLayoutPanel2.Controls.Add(this.groupBox3);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel2.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(553, 487);
            this.flowLayoutPanel2.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(231, 52);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "groupBox2";
            // 
            // groupBox3
            // 
            this.groupBox3.Location = new System.Drawing.Point(3, 61);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(231, 52);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "groupBox3";
            // 
            // SpriteManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1118, 735);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(735, 455);
            this.Name = "SpriteManager";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SpriteManager";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spriteRenderer)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fps_value)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox spriteRenderer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button cancel_button;
        private System.Windows.Forms.Button ok_button;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown fps_value;
        private System.Windows.Forms.CheckBox chk_loop;
        private System.Windows.Forms.Button btn_last;
        private System.Windows.Forms.Button btn_first;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.Button btn_next;
        private System.Windows.Forms.Button btn_prev;
        private System.Windows.Forms.Button btn_play;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ColorDialog colorDialog1;
    }
}