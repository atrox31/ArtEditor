using System;
using System.Diagnostics;
using System.IO;
namespace ArtCore_Editor
{
    public partial class SceneManager
    {
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowGrid = !ShowGrid;
            showToolStripMenuItem.Checked = ShowGrid;
            RedrawScene();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // save
            Save();
        }

        private void dimensionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridControl.GetGridDimensions(ref Grid);
            RedrawScene();
        }

        private void snapToGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SnapGrid = !SnapGrid;
            snapToGridToolStripMenuItem.Checked = SnapGrid;
        }
        private void gUIEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string guiFile = GameProject.ProjectPath + cScene.ProjectPath + "\\gui\\" + cScene.Name + "_" + "gui.txt";
            Process process = new Process();
            process.StartInfo.FileName = "..\\Core\\gui-bulider\\gui-builder.exe";
            bool fex = File.Exists(guiFile);
            process.StartInfo.Arguments = (fex ? "" : "-new ") + guiFile;
            process.StartInfo.UseShellExecute = false;


            process.Start();
            process.WaitForExit();
            if (File.Exists(guiFile))
            {
                cScene.GuiFile = cScene.ProjectPath + "\\" + cScene.Name + "_" + "gui.txt";
                MakeChange();
            }
        }
    }
}
