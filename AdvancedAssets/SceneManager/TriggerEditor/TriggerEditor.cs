using ArtCore_Editor.AdvancedAssets.Instance_Manager.Behavior;
using ArtCore_Editor.AdvancedAssets.Instance_Manager.code;
using ArtCore_Editor.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ArtCore_Editor.AdvancedAssets.SceneManager.GuiEditor;
using ArtCore_Editor.AdvancedAssets.SceneManager.TriggerEditor.pickers;
using ArtCore_Editor.Pick_forms;
using ArtCore_Editor.Main;

namespace ArtCore_Editor.AdvancedAssets.SceneManager.TriggerEditor
{
    public partial class TriggerEditor : Form
    {
        private readonly GuiElement _guiElement;
        private readonly Scene _scene;
        private readonly Dictionary<string, string> _eventsData = new Dictionary<string, string>();
        public TriggerEditor(Scene scene, GuiElement guiElement)
        {
            InitializeComponent(); Program.ApplyTheme(this);
            _scene = scene;

            _guiElement = guiElement;
            if (_guiElement != null)
            {
                // gui triggers
                foreach (string enumerateFile in Directory.EnumerateFiles(
                             GameProject.ProjectPath + "\\" + "scene" + "\\" + scene.Name + "\\", "gui_*" + Program.FileExtensions_ArtCode))
                {
                    if (!Path.GetFileNameWithoutExtension(enumerateFile).StartsWith("gui_")) continue;
                    string[] evName = Path.GetFileNameWithoutExtension(enumerateFile).Substring("gui_".Length)
                        .Split('&');
                    if (evName.Length != 2) continue; // error
                    if (!Enum.TryParse(
                            evName[1],
                            out Event.TriggerType _
                        )) continue; // error
                    _eventsData.Add(evName[0] + "&" + evName[1], File.ReadAllText(enumerateFile));
                    Event_listobx.Items.Add(evName[0] + "&" + evName[1]);
                    //                             name#tag & Event.TriggerType
                }
            }
            else
            {
                // scene triggers
                foreach (string enumerateFile in Directory.EnumerateFiles(
                             GameProject.ProjectPath + "\\" + "scene" + "\\" + scene.Name + "\\", "scene&*" + Program.FileExtensions_ArtCode))
                {
                    if (!Path.GetFileNameWithoutExtension(enumerateFile).StartsWith("scene&")) continue;
                    string evName = Path.GetFileNameWithoutExtension(enumerateFile);
                    _eventsData.Add(evName, File.ReadAllText(enumerateFile));
                    Event_listobx.Items.Add(evName);
                    //                             name#tag & Event.TriggerType
                }
            }
        }

        private void Event_listbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Event_listobx.SelectedItem == null) return;

            if (_eventsData[Event_listobx.SelectedItem.ToString()!] == null) return;
            Event_treeview.Nodes.Clear();
            foreach (string line in _eventsData[Event_listobx.SelectedItem.ToString()!].Split(('\n')))
            {
                Event_treeview.Nodes.Add(line);
            }
        }

        private void Event_treeView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btn_code_Click(sender, e);
        }

        private void btn_script_Click(object sender, EventArgs e)
        {
            if (Event_listobx.SelectedItem == null)
            {
                if (!AddEvent()) return;
                if (Event_listobx.Items.Count >= 1) Event_listobx.SelectedItem = Event_listobx.Items[^1];
                else return;
            }

            ScriptEditor scriptEditor = new ScriptEditor(Variable.VariableType.VTypeNull, _scene.SceneVariables);
            if (scriptEditor.ShowDialog() != DialogResult.OK) return;
            // populate

            // error with function without args
            if (!scriptEditor.ReturnValue.EndsWith(')'))
            {
                scriptEditor.ReturnValue += "()";
            }

            int selectedNode = -1;
            if (Event_treeview.SelectedNode != null)
            {
                selectedNode = Event_treeview.SelectedNode.Index;
            }
            if (selectedNode == -1)
            {
                Event_treeview.Nodes.Clear();
                _eventsData[Event_listobx.SelectedItem.ToString()!] += "\n" + scriptEditor.ReturnValue;
                foreach (string item in _eventsData[Event_listobx.SelectedItem.ToString()!].Split("\n"))
                {
                    Event_treeview.Nodes.Add(item);
                }
            }
            else
            {
                List<string> code = _eventsData[Event_listobx.SelectedItem.ToString()!].Split('\n').ToList();
                code.Insert(selectedNode + 1, scriptEditor.ReturnValue);
                _eventsData[Event_listobx.SelectedItem.ToString()!] = String.Join('\n', code);
                Event_treeview.Nodes.Insert(selectedNode + 1, scriptEditor.ReturnValue);
            }
        }

        private void btn_code_Click(object sender, EventArgs e)
        {
            // code
            if (Event_listobx.SelectedItem == null)
            {
                if (!AddEvent()) return;
                if (Event_listobx.Items.Count >= 1) Event_listobx.SelectedItem = Event_listobx.Items[^1];
                else return;
            }

            string code = _eventsData[Event_listobx.SelectedItem.ToString()!];
            CodeEditor codeEditor = new CodeEditor(code, _scene.SceneVariables);

            if (codeEditor.ShowDialog() != DialogResult.OK) return;
            _eventsData[Event_listobx.SelectedItem.ToString()!] = String.Join("\n", codeEditor.Code);
            Event_treeview.Nodes.Clear();
            foreach (string line in codeEditor.Code)
            {
                Event_treeview.Nodes.Add(line);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddEvent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // delete event - bellow Event_listbox
            if (Event_listobx.SelectedItem == null) return;
            if (MessageBox.Show("Are You sure to delete event '" + Event_listobx.SelectedItem.ToString() + "'",
                    "Delete event", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // mark file to delete on save exit
                string prefix = Event_listobx.SelectedItem.ToString()!.Contains('#') ? "gui_" : "";

                string oldFile = GameProject.ProjectPath + _scene.ProjectPath + "\\" + prefix +
                                 Event_listobx.SelectedItem + "" + Program.FileExtensions_ArtCode;
                string newFile = GameProject.ProjectPath + _scene.ProjectPath + "\\!" + prefix +
                                 Event_listobx.SelectedItem + "" + Program.FileExtensions_ArtCode;
                if (File.Exists(oldFile))
                {
                    File.Move(oldFile, newFile);
                }

                //File.Move();
                _eventsData.Remove(Event_listobx.SelectedItem.ToString()!);
                Event_listobx.Items.Remove(Event_listobx.SelectedItem);
            }
        }

        bool AddEvent()
        {
            string guiElementName;
            string triggerEventName;
            if (_guiElement != null)
            {
                Pick_forms.PicFromList picFrom = new PicFromList(_guiElement.GetAllChildrenNamesList());
                if (picFrom.ShowDialog() != DialogResult.OK) return false;
                guiElementName = picFrom.Selected;

                TriggerEventPicker objectEventPicker = new TriggerEventPicker();
                if (objectEventPicker.ShowDialog() != DialogResult.OK) return false;
                triggerEventName = objectEventPicker.Get().ToString();

            if (Functions.Functions.ErrorCheck(
                    !_eventsData.ContainsKey(guiElementName + "&" + triggerEventName)
                    , "Event exists!")) return false;

            _eventsData.Add(guiElementName + "&" + triggerEventName, 
                "// " + _guiElement.GetPath());

            }
            else
            {
                guiElementName = "scene";
                triggerEventName = GetString.Get("Trigger name");
                if (triggerEventName == null)
                {
                    return false;
                }
                if (Functions.Functions.ErrorCheck(
                        !_eventsData.ContainsKey(guiElementName + "&" + triggerEventName)
                        , "Event exists!")) return false;
                _eventsData.Add(guiElementName + "&" + triggerEventName,
                    "// " + triggerEventName);
            }

            Event_listobx.Items.Add(guiElementName + "&" + triggerEventName);
            Event_listobx.SelectedIndex = Event_listobx.Items.Count - 1;

            btn_code.Enabled = true;
            btn_script.Enabled = true;
            return true;
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            // apply
            Save();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            // restore all files that is marked to delete
            foreach (string enumerateFile in Directory.EnumerateFiles(
                         GameProject.ProjectPath + "\\" + "scene" + "\\" + _scene.Name + "\\", "!*" + Program.FileExtensions_ArtCode))
            {
                File.Move(enumerateFile, enumerateFile.Replace("!", ""));
            }
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void Save()
        {
            List<string> fileToSave = new List<string>();
            foreach (KeyValuePair<string, string> eventData in _eventsData)
            {
                if (eventData.Key.Contains("scene&"))
                { // scene trigger
                    fileToSave.Add(GameProject.ProjectPath + "\\" + "scene" + "\\" + _scene.Name + "\\" +
                                   eventData.Key + "" + Program.FileExtensions_ArtCode);
                    File.WriteAllText(
                        fileToSave.Last(),
                        eventData.Value
                    );

                }
                else
                { // gui trigger
                    fileToSave.Add(GameProject.ProjectPath + "\\" + "scene" + "\\" + _scene.Name + "\\" + "gui_" +
                                   eventData.Key + "" + Program.FileExtensions_ArtCode);
                    File.WriteAllText(
                        fileToSave.Last(),
                        eventData.Value
                    );
                }

            }
            // delete unused files
            foreach (string enumerateFile in Directory.EnumerateFiles(
                         GameProject.ProjectPath + "\\" + "scene" + "\\" + _scene.Name + "\\", "*" + Program.FileExtensions_ArtCode))
            {
                if (Path.GetFileName(enumerateFile).StartsWith('!'))
                {
                    File.Delete(enumerateFile);
                }
            }
            
        }
    }
}
