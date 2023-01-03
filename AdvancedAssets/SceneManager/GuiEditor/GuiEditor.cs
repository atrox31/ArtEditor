using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ArtCore_Editor.AdvancedAssets.Instance_Manager.variable;
using ArtCore_Editor.Enums;
using ArtCore_Editor.Functions;
using Newtonsoft.Json;

namespace ArtCore_Editor.AdvancedAssets.SceneManager.GuiEditor
{
    public partial class GuiEditor : Form
    {
        private GuiElement _currentGuiElement = null;

        // root element that contain all children`s
        [System.Text.Json.Serialization.JsonIgnore]
        private readonly GuiElement _rootElement;

        private readonly string _sceneName;
        private readonly string _guiSchemaFile;

        public string GetSceneName()
        {
            return _sceneName;
        }

        public GuiEditor(string sceneName)
        {
            InitializeComponent();
            Program.ApplyTheme(this);

            this._sceneName = sceneName;
            _guiSchemaFile =
                GameProject.ProjectPath + "\\" + "scene" + "\\" + _sceneName + "\\" + "GuiSchema.json";
            if (File.Exists(_guiSchemaFile))
            {
                // copy schema
                _rootElement =
                    JsonConvert.DeserializeObject<GuiElement>(File.ReadAllText(_guiSchemaFile));
                // must assign parents
                _rootElement?.SetAllParents();
            }
            else
            {
                // prepare new 
                _rootElement = new GuiElement("root", null);
            }

            UpdateTreeView();
        }
        
        private void GuiElementList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (GuiElementList.SelectedNode == null) return;

            _currentGuiElement = _rootElement.FindGuiElement(GuiElementList.SelectedNode.FullPath);
            if (_currentGuiElement == null) return;

            GuiElementProperties.Rows.Clear();
            foreach (Variable rowProperty in _currentGuiElement.GetAllProperties())
            {
                GuiElementProperties.Rows.Add(rowProperty.Name, rowProperty.Default);
            }
        }

        private void GuiElementList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            if (GuiElementList.SelectedNode == null) return;

            // construct context menu strip
            ToolStripMenuItem itemAdd = new ToolStripMenuItem("Add");
            foreach (string templateName in GuiElement.GetTemplateNames())
            {
                itemAdd.DropDownItems.Add(templateName, null, (o, args) =>
                {
                    // check if target exists
                    GuiElement target = _rootElement.FindGuiElement(GuiElementList.SelectedNode.FullPath);
                    if (target == null) return;

                    // add new element and select it
                    _currentGuiElement = target.AddChild(templateName);
                    // update tree view
                    UpdateTreeView();
                });
            }

            ContextMenuStrip contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add(itemAdd);
            if (GuiElementList.SelectedNode.Parent != null)
            {
                contextMenu.Items.Add("Delete", null, (o, args) =>
                {
                    // remove element
                    GuiElement target = _rootElement.FindGuiElement(GuiElementList.SelectedNode.FullPath);
                    if (target == null) return;
                    // we must get parent of selected element to remove all children`s
                    // for deleteChildren we need only tag because its unique
                    target.GetParent().DeleteChildren(GuiElementList.SelectedNode.ToString().Split('#').Last());

                    // refresh view
                    _currentGuiElement = null;
                    GuiElementProperties.Rows.Clear();
                    UpdateTreeView();
                });
            }

            contextMenu.Show(MousePosition);

        }

        private void UpdateTreeView()
        {
            List<string> savedExpansionState = GuiElementList.Nodes.GetExpansionState();
            GuiElementList.BeginUpdate();

            GuiElementList.SelectedNode = null;
            GuiElementList.Nodes.Clear();
            GuiElementList.PopulateTreeView(_rootElement.GetAllChildrenPaths(), '/');

            GuiElementList.Nodes.SetExpansionState(savedExpansionState);
            GuiElementList.EndUpdate();
        }

        private void GuiElementProperties_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string fieldName = GuiElementProperties.Rows[e.RowIndex].Cells[0].Value.ToString();
            _currentGuiElement?.SetValue(
                fieldName,
                GuiElementProperties.Rows[e.RowIndex].Cells[1].Value.ToString());
            // if changed value is tag, refresh list view to get fresh view
            if (fieldName == "Tag") UpdateTreeView();
        }

        private void GuiElementProperties_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            Variable editedVariable =
                _currentGuiElement.GetVariable(GuiElementProperties.Rows[e.RowIndex].Cells[0].Value.ToString());

            VariableEditor variableEditor = new VariableEditor(editedVariable, true);
            if (variableEditor.ShowDialog() == DialogResult.OK)
            {
                GuiElementProperties.Rows[e.RowIndex].Cells[1].Value = editedVariable.Default.Replace("\"", "");
            }
            else
            {
                e.Cancel = true;
            }

        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            string guiElementSerializeObject = JsonConvert.SerializeObject(_rootElement);
            File.WriteAllText(_guiSchemaFile, guiElementSerializeObject);
            DialogResult = DialogResult.OK;
            Close();
        }

    }
}
