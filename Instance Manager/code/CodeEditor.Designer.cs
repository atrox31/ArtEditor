namespace ArtCore_Editor.Instance_Manager.code
{
    partial class CodeEditor
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
            this.CodeHelper = new System.Windows.Forms.StatusStrip();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.AcceptsTab = true;
            this.richTextBox1.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Consolas", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(1007, 725);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // CodeHelper
            // 
            this.CodeHelper.Location = new System.Drawing.Point(0, 725);
            this.CodeHelper.Name = "CodeHelper";
            this.CodeHelper.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.CodeHelper.Size = new System.Drawing.Size(1007, 22);
            this.CodeHelper.TabIndex = 2;
            this.CodeHelper.Text = "statusStrip1";
            // 
            // CodeEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1007, 747);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.CodeHelper);
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "CodeEditor";
            this.Text = "CodeEditor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CodeEditor_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.StatusStrip CodeHelper;
    }
}