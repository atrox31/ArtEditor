namespace ArtCore_Editor
{
    partial class GameCompiler
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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.OutputLog = new System.Windows.Forms.ListBox();
            this.Bgw = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(14, 492);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(384, 27);
            this.progressBar1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(791, 489);
            this.button1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 27);
            this.button1.TabIndex = 2;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // OutputLog
            // 
            this.OutputLog.FormattingEnabled = true;
            this.OutputLog.ItemHeight = 15;
            this.OutputLog.Location = new System.Drawing.Point(14, 14);
            this.OutputLog.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.OutputLog.Name = "OutputLog";
            this.OutputLog.Size = new System.Drawing.Size(865, 469);
            this.OutputLog.TabIndex = 3;
            this.OutputLog.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OutputLog_MouseDoubleClick);
            // 
            // Bgw
            // 
            this.Bgw.WorkerReportsProgress = true;
            this.Bgw.WorkerSupportsCancellation = true;
            this.Bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.Bgw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.Bgw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // GameCompiler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(892, 532);
            this.ControlBox = false;
            this.Controls.Add(this.OutputLog);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "GameCompiler";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GameCompiler";
            this.Shown += new System.EventHandler(this.GameCompiler_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox OutputLog;
        private System.ComponentModel.BackgroundWorker Bgw;
    }
}