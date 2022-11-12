using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    public partial class LoadScreen : Form
    {
        bool _fake;
        public LoadScreen(bool fake)
        {
            InitializeComponent();
            _fake = fake;
            progressBar1.Value = 10;
            progressBar1.Visible = true;
            progressBar1.Enabled = true;
        }
        public void SetProgress(int val)
        {
            progressBar1.Value = val;
        }
        public void AddProgress(int val)
        {
            progressBar1.Value += val;
            if (progressBar1.Value > progressBar1.Maximum)
            {
                progressBar1.Value = progressBar1.Maximum;
            }
        }

        private void LoadScreen_FormClosing(object sender, FormClosingEventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i < 100; i++)
            {
                if (backgroundWorker1.CancellationPending == true)
                {
                    e.Cancel = true;
                    return;
                }
                backgroundWorker1.ReportProgress(i);
                System.Threading.Thread.Sleep(60);
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (progressBar1.Value < 100)
            {
                progressBar1.Value++;
            }
        }

        private void LoadScreen_Shown(object sender, EventArgs e)
        {
            if (_fake)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }
    }
}
