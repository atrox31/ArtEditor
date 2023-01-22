using ArtCore_Editor.Functions;
using ArtCore_Editor.Main;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms;

namespace ArtCore_Editor.AdvancedAssets.SceneManager.LevelEditor
{
    public partial class LevelListSelect : Form
    {
        private bool _changes = false;

        private readonly Scene  _scene;
        private readonly string _name;
        private readonly string _levelPath;
        private readonly Point  _grid;
        public LevelListSelect(Scene scene, string name, Point grid)
        {
            InitializeComponent(); Program.ApplyTheme(this);
            _scene = scene;
            _name = name;
            _grid = grid;
            _levelPath = StringExtensions.Combine(GameProject.ProjectPath, scene.ProjectPath, "levels");
        }

        private void Save()
        {
            _changes = false;
            if (listBox1.Items.Count == 0)
            {
                // no data or delete all data
                Functions.Functions.CleanDirectory(_levelPath);
                return;
            }

            // read all level data 
            List<byte[]> filesContent = new List<byte[]>();
            foreach (object listBox1Item in listBox1.Items)
            {
                string filePath = StringExtensions.Combine(_levelPath, listBox1Item.ToString() + Program.FileExtensions_SceneLevel);
                if (File.Exists(filePath))
                {
                    filesContent.Add(File.ReadAllBytes(filePath));
                }
            }

            // write all level data in target order
            if (filesContent.Count == 0) return;
            Functions.Functions.CleanDirectory(_levelPath);
            for (int i = 0; i < filesContent.Count; i++)
            {
                string filePath = StringExtensions.Combine(_levelPath, _name + i.ToString() + Program.FileExtensions_SceneLevel);
                File.WriteAllBytes(filePath, filesContent[i]);
            }
        }

        private void Btn_apply_Click(object sender, EventArgs e)
        {
            if (_changes) Save();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            if (!_changes)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
            switch (MessageBox.Show("You make changes, do You want to save?", "Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    Save();
                    DialogResult = DialogResult.OK;
                    Close();
                    break;
                case DialogResult.No:
                    DialogResult = DialogResult.Cancel;
                    Close();
                    break;
                case DialogResult.Cancel:
                    // do nothing
                    break;
            }
        }

        private void Btn_edit_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0) return; // not selected

            LevelEditorMain levelEditor = new LevelEditorMain(
                _scene, listBox1.SelectedItem.ToString() + Program.FileExtensions_SceneLevel,
                _grid, _levelPath);
            if (levelEditor.ShowDialog() != DialogResult.OK) return;

            RefreshPreview();
            _changes = true;
        }

        private void Btn_down_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0) return; // not selected
            if (listBox1.SelectedIndex >= listBox1.Items.Count - 1) return; // index must be less than size

            // swap
            (listBox1.Items[listBox1.SelectedIndex + 1], listBox1.Items[listBox1.SelectedIndex]) = (listBox1.Items[listBox1.SelectedIndex], listBox1.Items[listBox1.SelectedIndex + 1]);
            listBox1.SelectedIndex++;
            _changes = true;
        }

        private void Btn_up_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0) return; // not selected
            if (listBox1.SelectedIndex < 1) return; // index must be more than 1

            // swap
            (listBox1.Items[listBox1.SelectedIndex - 1], listBox1.Items[listBox1.SelectedIndex]) = (listBox1.Items[listBox1.SelectedIndex], listBox1.Items[listBox1.SelectedIndex - 1]);
            listBox1.SelectedIndex--;
            _changes = true;
        }

        private void Btn_new_Click(object sender, EventArgs e)
        {
            LevelEditorMain levelEditor = new LevelEditorMain(_scene, _name, _grid, _levelPath);
            if (levelEditor.ShowDialog() != DialogResult.OK) return;

            listBox1.Items.Add(levelEditor.GetName());
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
            _changes = true;
            //RefreshPreview();
        }

        private void Btn_delete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0) return;
            listBox1.Items.RemoveAt(listBox1.SelectedIndex);
            _changes = true;
        }

        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            RefreshPreview();
        }

        private void RefreshPreview()
        {
            if (listBox1.SelectedIndex < 0) return; // not selected
            pictureBox1.Image?.Dispose();
            pictureBox1.Image = null;

            LevelEditorMain levelEditor = new LevelEditorMain(
                _scene, listBox1.SelectedItem.ToString() + Program.FileExtensions_SceneLevel,
                _grid, _levelPath);
            pictureBox1.Image = levelEditor.GetPreview(pictureBox1.Width, pictureBox1.Height);
            //pictureBox1.Refresh();
        }

        private void LevelListSelect_Load(object sender, EventArgs e)
        {
            string levelPath = StringExtensions.Combine(GameProject.ProjectPath, _scene.ProjectPath, "levels");
            listBox1.Items.AddRange(Directory.GetFiles(levelPath)
                                        .Select(item => item.WithoutExtension())
                                        .ToArray());
        }
    }
}
