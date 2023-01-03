using ArtCore_Editor.AdvancedAssets.Instance_Manager.variable;
using ArtCore_Editor.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ArtCore_Editor.AdvancedAssets.SceneManager
{
    public partial class VariablesEditor : Form
    {
        private List<Variable> _sceneVariables = new List<Variable>();
        private readonly Scene _scene;
        private bool _changes = false;

        public VariablesEditor(Scene scene)
        {
            InitializeComponent();Program.ApplyTheme(this);

            _scene = scene;
            // copy variables to not apply changes before save
            foreach (Variable item in _scene.SceneVariables)
            {
                _sceneVariables.Add(item);
                Varible_listbox.Items.Add(item.Name + '[' + item.Type.ToString() + ']');
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            VariableEditor variable = new VariableEditor();
            if (variable.ShowDialog() != DialogResult.OK) return;

            foreach (Variable var in _sceneVariables)
            {
                if (var.Name == ((Control)variable).Name)
                {
                    MessageBox.Show("CurrentVariable name exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            _sceneVariables.Add(variable.CurrentVariable);
            Varible_listbox.Items.Clear();
            foreach (Variable item in _sceneVariables)
            {
                Varible_listbox.Items.Add(item.Name + '[' + item.Type.ToString() + ']');
            }
            _changes = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Varible_listbox.SelectedItem == null) return;
            if (MessageBox.Show("Are You sure to delete variable '" + Varible_listbox.SelectedItem.ToString() + "'",
                    "Delete variable", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            foreach (Variable var in _sceneVariables)
            {
                if (var.Name != Varible_listbox.SelectedItem.ToString()!.Split('[').First()) continue;
                _sceneVariables.Remove(var);
                break;
            }
            Varible_listbox.Items.Remove(Varible_listbox.SelectedItem);
            _changes = true;
        }

        private void VariablesEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_changes) return;
            switch (MessageBox.Show("You made some changes, save?", "Save variables?",
                        MessageBoxButtons.YesNoCancel,
                        MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    _scene.SceneVariables = _sceneVariables;
                    this.DialogResult = DialogResult.OK;
                    break;
                case DialogResult.No:
                    this.DialogResult = DialogResult.Cancel;
                    break;
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
            }
        }
    }
}
