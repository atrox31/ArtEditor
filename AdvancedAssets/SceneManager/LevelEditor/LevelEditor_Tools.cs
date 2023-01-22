using ArtCore_Editor.AdvancedAssets.Instance_Manager;
using ArtCore_Editor.AdvancedAssets.Instance_Manager.code;
using ArtCore_Editor.Functions;
using ArtCore_Editor.Main;
using ArtCore_Editor.Pick_forms;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;


namespace ArtCore_Editor.AdvancedAssets.SceneManager.LevelEditor
{
    public partial class LevelEditorTools : Form
    {
        private readonly Scene _scene;          // parent scene of level
        private readonly string _triggersPath; // const path to trigger data
        private readonly Dictionary<string, string> _triggerData; // triggers code
        public SceneManager.SceneInstance SelectedInstance { get; set; }
        public LevelEditorTools(Scene scene, string path)
        {
            InitializeComponent(); Program.ApplyTheme(this);
            SelectedInstance = null;
            _scene = scene;
            _triggersPath = path;
            _triggerData = new Dictionary<string, string>();
        }

        private void LevelEditor_Tools_Load(object sender, EventArgs e)
        {
            instance_list_view.Items.Add("null", 0);
            // load all assets
            foreach (KeyValuePair<string, Instance> item in GameProject.GetInstance().Instances)
            {
                // check if can be placed in scene
                if (!item.Value.EditorShowInLevel) continue;

                if (item.Value.Sprite == null)
                {
                    instance_list_view.Items.Add(item.Key, 0);
                }
                else
                {
                    Bitmap image = ZipIO.ReadImageFromArchive(
                        StringExtensions.Combine(GameProject.ProjectPath, item.Value.Sprite.DataPath),
                        "0.png");
                    if (image != null)
                    {
                        Instance_imagelist.Images.Add(image.GetThumbnailImage(64, 64, null, IntPtr.Zero));
                    }

                    instance_list_view.Items.Add(item.Key,
                        image == null ? 0 : Instance_imagelist.Images.Count - 1
                        );
                }
            }

            // load all triggers
            foreach (string trigger in ZipIO.ReadFromZip(
                                        _triggersPath,
                                "triggers.txt"
                                    ).Split('\n'))
            {
                string triggerData = ZipIO.ReadFromZip(
                                        _triggersPath,
                                trigger, true
                                    );
                if (triggerData == null) continue;
                _triggerData.Add(trigger.WithoutExtension(),
                    triggerData);
            }

            RefreshTriggerList();
        }

        private void RefreshTriggerList()
        {
            trigger_listbox.Items.Clear();
            foreach (string triggerDataKey in _triggerData.Keys)
            {
                trigger_listbox.Items.Add(triggerDataKey);
            }
        }

        public void SaveTriggers()
        {
            List<string> triggers = new List<string>();
            foreach (KeyValuePair<string, string> item in _triggerData)
            {
                string triggerFileName = item.Key + Program.FileExtensions_ArtCode;
                triggers.Add(triggerFileName);

                ZipIO.WriteLineToArchive(
                    _triggersPath,
                    triggerFileName,
                    item.Value,
                    true
                );
            }

            ZipIO.WriteListToArchive(
                _triggersPath,
                "triggers.txt",
                triggers,
                true);
        }

        private void instance_list_view_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(instance_list_view.SelectedItems.Count == 0 
            || instance_list_view.SelectedItems[0].Text == "null")
            {
                SelectedInstance = null;
                return;
            }
            SelectedInstance = new SceneManager.SceneInstance(0,0,
                        GameProject.GetInstance().Instances.GetValueOrDefault(
                                instance_list_view.SelectedItems[0].Text
                        )       
                      );
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedInstance = null;
        }

        private void btn_trigger_new_Click(object sender, EventArgs e)
        {
            string triggerName = GetString.Get("Trigger name");
            if (triggerName == null) return;
            if (Functions.Functions.ErrorCheck(!_triggerData.ContainsKey(triggerName),
                "Trigger with that name exists!"
                )) return;

            EditTrigger(triggerName);
        }

        private void btn_trigger_edit_Click(object sender, EventArgs e)
        {
            if (trigger_listbox.SelectedItem == null) return;
            EditTrigger(trigger_listbox.SelectedItem.ToString());
        }
        private void trigger_listbox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (trigger_listbox.SelectedItem == null) return;
            EditTrigger(trigger_listbox.SelectedItem.ToString());
        }

        private void EditTrigger(string trigger)
        {
            if(_triggerData.ContainsKey(trigger))
            {
                CodeEditor codeEditor = new CodeEditor(_triggerData[trigger],_scene.SceneVariables);
                if(codeEditor.ShowDialog() == DialogResult.OK)
                {
                    _triggerData[trigger] = String.Join("\n", codeEditor.Code);
                }
            }
            else
            {
                CodeEditor codeEditor = new CodeEditor("// " + trigger, _scene.SceneVariables);
                if (codeEditor.ShowDialog() == DialogResult.OK)
                {
                    _triggerData.Add(trigger, String.Join("\n", codeEditor.Code));
                }
            }
            RefreshTriggerList();

        }

        private void btn_trigger_delete_Click(object sender, EventArgs e)
        {
            if (trigger_listbox.SelectedItem == null) return;
            if (MessageBox.Show(
                    "Delete '" + trigger_listbox.SelectedItem + "' ?", "Delete trigger?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;

            _triggerData.Remove(trigger_listbox.SelectedItem.ToString()!);
            trigger_listbox.Items.Remove(trigger_listbox.SelectedItem.ToString()!);
        }

    }
}
