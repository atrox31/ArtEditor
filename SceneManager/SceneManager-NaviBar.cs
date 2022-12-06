using System;
using System.Diagnostics;
using System.IO;
namespace ArtCore_Editor
{
    public partial class SceneManager
    {    /// <summary>
        /// scene navigation bar actions
        /// </summary>
        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _showGrid = !_showGrid;
            showToolStripMenuItem.Checked = _showGrid;
            RedrawScene();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void dimensionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridControl.GetGridDimensions(ref _grid);
            RedrawScene();
        }

        private void snapToGridToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _snapGrid = !_snapGrid;
            snapToGridToolStripMenuItem.Checked = _snapGrid;
        }
    }
}
