using ArtCore_Editor.AdvancedAssets.SceneManager.GuiEditor;
using ArtCore_Editor.Main;
using ArtCore_Editor.Pick_forms;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ArtCore_Editor.AdvancedAssets.SceneManager
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

        private void editSchemaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GuiEditor.GuiEditor guiEditor = new GuiEditor.GuiEditor(_cScene.Name);
            guiEditor.ShowDialog();
        }
        
        private void guiTriggersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string guiSchemaFile =
                GameProject.ProjectPath + "\\" + "scene" + "\\" + _cScene.Name + "\\" + "GuiSchema.json";
            if (!File.Exists(guiSchemaFile)) return;

            // copy schema
            GuiElement rootElement =
                JsonConvert.DeserializeObject<GuiElement>(File.ReadAllText(guiSchemaFile));
            if (rootElement == null) return;

            // must assign parents
            rootElement?.SetAllParents();

            TriggerEditor.TriggerEditor editor = new TriggerEditor.TriggerEditor(_cScene, rootElement);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                // no action
            }
        }
        private void editTriggersToolStripMenuItem_Click(object sender, EventArgs e)
        {

            TriggerEditor.TriggerEditor editor = new TriggerEditor.TriggerEditor(_cScene, null);
            if (editor.ShowDialog() == DialogResult.OK)
            {
                // no action
            }
        }


        private void setStartupTriggerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _cScene.SceneStartingTrigger =
                PicFromList.Get(Directory.GetFiles(GameProject.ProjectPath + _cScene.ProjectPath, "scene&*" + Program.FileExtensions_ArtCode)
                    .Select(Path.GetFileNameWithoutExtension).ToList());

        }
        private void sceneVariablesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            VariablesEditor variablesEditor = new VariablesEditor(_cScene);
            if (variablesEditor.ShowDialog() == DialogResult.OK)
            {
                MakeChange();
            }
        }
    }
}
