using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using ArtCore_Editor.AdvancedAssets.Instance_Manager.Behavior;
using ArtCore_Editor.AdvancedAssets.Instance_Manager.Behavior.pickers;
using ArtCore_Editor.AdvancedAssets.Instance_Manager.code;
using ArtCore_Editor.AdvancedAssets.Instance_Manager.variable;
using ArtCore_Editor.AdvancedAssets.SpriteManager;
using ArtCore_Editor.Enums;
using ArtCore_Editor.Functions;
using ArtCore_Editor.Pick_forms;
using Newtonsoft.Json;
using static ArtCore_Editor.Main.GameProject;
using Path = System.IO.Path;

namespace ArtCore_Editor.AdvancedAssets.Instance_Manager;

public partial class ObjectManager : Form
{
    // id of current edit object
    private string _assetId;
    // ref to current object
    private readonly Instance _currentObject;
    private readonly Dictionary<Event.EventType, string> _eventsData;


    public ObjectManager(string assetId = null, int line = -1, string function = null)
    {
        InitializeComponent(); Program.ApplyTheme(this);

        // fill sprite selection
        foreach (KeyValuePair<string, Sprite> sprite in GetInstance().Sprites)
        {
            comboBox1.Items.Add(sprite.Key);
        }

        _assetId = assetId;
        _eventsData = new Dictionary<Event.EventType, string>();

        if (assetId != null)
        {
            string openingFileName = ProjectPath + "\\" + GetInstance().Instances[assetId].ProjectPath + "\\" + GetInstance().Instances[assetId].FileName;
            if (!File.Exists(openingFileName))
            {
                MessageBox.Show("File: '" + openingFileName + "' not exists", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (StreamReader reader = new StreamReader(File.Open(openingFileName, FileMode.Open)))
            {
                _currentObject = JsonConvert.DeserializeObject<Instance>(reader.ReadToEnd());
            }


            if (_currentObject == null) return;
            textBox1.Text = _currentObject.Name;
            comboBox1.Text = _currentObject.Sprite != null ? _currentObject.Sprite.Name : "<default>";

            if (_currentObject.BodyDataType.Type != Instance.BodyData.BType.None)
            {
                bodyType_mask.Checked = (_currentObject.BodyDataType.Type == Instance.BodyData.BType.Sprite);
                bodyType_rect.Checked = (_currentObject.BodyDataType.Type == Instance.BodyData.BType.Rect);
                bodyType_circle.Checked = (_currentObject.BodyDataType.Type == Instance.BodyData.BType.Circle);
                bodyType_value.Value = _currentObject.BodyDataType.Value;
                bodyType_IsSolid.Checked = true;
            }
            else
            {
                bodyType_IsSolid.Checked = false;
            }

            foreach (Variable item in _currentObject.Variables)
            {
                Varible_listbox.Items.Add(item.Name + '[' + item.Type.ToString() + ']');
            }

            if (_currentObject.Events.Count > 0)
            {
                button5.Enabled = true;
                button8.Enabled = true;
                foreach (KeyValuePair<Event.EventType, string> item in _currentObject.Events)
                {
                    string path = ProjectPath + "\\object\\" + _currentObject.Name + "\\" + item.Value +
                                  "" + Program.FileExtensions_ArtCode;
                    if (File.Exists(path))
                    {
                        Event_listobx.Items.Add(item.Key);
                        _eventsData[item.Key] = File.ReadAllText(path);
                    }
                    else
                    {
                        MessageBox.Show("File '" + _currentObject.Name + "\\" + item.Value + "" + Program.FileExtensions_ArtCode +
                                        "' not found");
                        File.CreateText(path).WriteLine("// file not found");
                    }
                }
            }

            if (line >= 0 && function != null)
            {
                // caller is game compiler, open wrong code
                Event_listobx.SelectedIndex = Event_listobx.Items.IndexOf(function);
                string code = _eventsData[(Event.EventType)Enum.Parse(typeof(Event.EventType), function)];

                CodeEditor codeEditor = new CodeEditor(code, line);
                if (codeEditor.ShowDialog() != DialogResult.OK) return;
                _eventsData[(Event.EventType)Enum.Parse(typeof(Event.EventType), function)] = String.Join("\n", codeEditor.Code);
                Event_treeview.Nodes.Clear();
                foreach (string new_line in codeEditor.Code)
                {
                    Event_treeview.Nodes.Add(new_line);
                }

            }
        }
        else
        {
            _currentObject = new Instance();
        }
    }

    bool AddEvent()
    {
        // add event - bellow Event_listbox
        ObjectEventPicker objectEventPicker = new ObjectEventPicker();
        if (objectEventPicker.ShowDialog() == DialogResult.OK)
        {

            Event.EventType type = objectEventPicker.Type;
            {
                if (Event_listobx.SelectedItem != null)
                {
                    if (Functions.Functions.ErrorCheck(
                            _currentObject.Events.TryGetValue(
                                (Event.EventType)Enum.Parse(
                                    typeof(Event.EventType),
                                    Event_listobx.SelectedItem.ToString() ?? string.Empty
                                ), out _
                            ), "Event exists!"))
                    {
                        Event_listobx.SelectedItem = type.ToString();
                        return false;
                    }
                }

                if (!_currentObject.Events.ContainsKey(type))
                {
                    _currentObject.Events.Add(type, type.ToString());
                    _eventsData.Add(type, "//" + type.ToString());
                    Event_listobx.Items.Add(type.ToString());
                }
            }

            Event_listobx.SelectedIndex = Event_listobx.Items.Count - 1;
            button5.Enabled = true;
            button8.Enabled = true;
            return true;
        }
        return false;
    }

    private void button1_Click(object sender, EventArgs e)
    {
        AddEvent();
    }

    private void button2_Click(object sender, EventArgs e)
    {
        // delete event - bellow Event_listbox
        if (Event_listobx.SelectedItem != null)
        {
            if (MessageBox.Show("Are You sure to delete event '" + Event_listobx.SelectedItem.ToString() + "'", "Delete event", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _currentObject.Events.Remove((Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString()!));
                _eventsData.Remove((Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString()!));
                Event_listobx.Items.Remove(Event_listobx.SelectedItem);
            }
        }
    }

    private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Event_listobx.SelectedItem != null)
        {
            if (_eventsData[((Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString()!))] != null)
            {
                Event_treeview.Nodes.Clear();
                foreach (
                    string line in
                    _eventsData[
                        (Event.EventType)
                        Enum.Parse(
                            typeof(Event.EventType),
                            Event_listobx.SelectedItem.ToString()!
                        )
                    ].Split('\n'))
                {
                    Event_treeview.Nodes.Add(line);
                }
            }
        }
    }

    private void button5_Click(object sender, EventArgs e)
    {
        if (Event_listobx.SelectedItem == null)
        {
            if (!AddEvent()) return;
            if (Event_listobx.Items.Count >= 1) Event_listobx.SelectedItem = Event_listobx.Items[^1];
            else return;
        }

        ScriptEditor scriptEditor = new ScriptEditor(Variable.VariableType.VTypeNull, _currentObject.Variables);
        if (scriptEditor.ShowDialog() == DialogResult.OK)
        {
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
                _eventsData[(Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString()!)] += "\n" + scriptEditor.ReturnValue;
                foreach (string item in _eventsData[(Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString()!)].Split("\n"))
                {
                    Event_treeview.Nodes.Add(item);
                }
            }
            else
            {
                List<string> code = _eventsData[(Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString()!)].Split('\n').ToList();
                code.Insert(selectedNode + 1, scriptEditor.ReturnValue);
                _eventsData[(Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString()!)] = String.Join('\n', code);
                Event_treeview.Nodes.Insert(selectedNode + 1, scriptEditor.ReturnValue);
            }
        }
    }

    private void button7_Click(object sender, EventArgs e)
    {
        VariableEditor variable = new VariableEditor();
        if (variable.ShowDialog() == DialogResult.OK)
        {
            foreach (Variable var in _currentObject.Variables)
            {
                if (var.Name == ((Control)variable).Name)
                {
                    MessageBox.Show("CurrentVariable name exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            _currentObject.Variables.Add(variable.CurrentVariable);
            Varible_listbox.Items.Clear();
            foreach (Variable item in _currentObject.Variables)
            {
                Varible_listbox.Items.Add(item.Name + '[' + item.Type.ToString() + ']');
            }
        }

    }

    private void button3_Click(object sender, EventArgs e)
    {
        // apply
        Save();
        DialogResult = DialogResult.OK;
        Close();
    }

    private void button8_Click(object sender, EventArgs e)
    {
        // code
        if (Event_listobx.SelectedItem == null)
        {
            if (!AddEvent()) return;
            if (Event_listobx.Items.Count >= 1) Event_listobx.SelectedItem = Event_listobx.Items[^1];
            else return;
        }
        string code = _eventsData[(Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString()!)];
        CodeEditor codeEditor = new CodeEditor( code);
        if (codeEditor.ShowDialog() != DialogResult.OK) return;
        _eventsData[(Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString()!)] = String.Join("\n", codeEditor.Code);
        Event_treeview.Nodes.Clear();
        foreach (string line in codeEditor.Code)
        {
            Event_treeview.Nodes.Add(line);
        }

    }

    private void button6_Click(object sender, EventArgs e)
    {
        if (Varible_listbox.SelectedItem == null) return;
        if (MessageBox.Show("Are You sure to delete variable '" + Varible_listbox.SelectedItem.ToString() + "'",
                "Delete variable", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        foreach (Variable var in _currentObject.Variables)
        {
            if (var.Name != Varible_listbox.SelectedItem.ToString()!.Split('[').First()) continue;
            _currentObject.Variables.Remove(var);
            break;
        }
        Varible_listbox.Items.Remove(Varible_listbox.SelectedItem);
    }
    public void Save()
    {
        if (Functions.Functions.ErrorCheck(textBox1.TextLength > 3, "Object name too short")) return;
        if (Functions.Functions.ErrorCheck(textBox1.TextLength < 24, "Object name too long")) return;

        _currentObject.Name = textBox1.Text;
        if (_assetId == null)
        {
            GetInstance().Instances.Add(_currentObject.Name, _currentObject);
            _assetId = _currentObject.Name;
        }
        else
        {
            if (_assetId != _currentObject.Name)
            {
                GetInstance().Instances.RenameKey(_assetId, _currentObject.Name);
            }
        }

        string pathToObjectData = ProjectPath + "\\object\\" + _currentObject.Name;
        if (Directory.Exists(pathToObjectData))
        {
            Directory.Delete(pathToObjectData, true);
        }
        Directory.CreateDirectory(pathToObjectData);

        _currentObject.FileName = _currentObject.Name + "" + Program.FileExtensions_InstanceObject;
        _currentObject.ProjectPath = "\\object\\" + _currentObject.Name + "\\";

        if (bodyType_IsSolid.Checked)
        {
            if (bodyType_mask.Checked) { _currentObject.BodyDataType.Type = Instance.BodyData.BType.Sprite; }
            if (bodyType_rect.Checked) { _currentObject.BodyDataType.Type = Instance.BodyData.BType.Rect; }
            if (bodyType_circle.Checked) { _currentObject.BodyDataType.Type = Instance.BodyData.BType.Circle; }
            _currentObject.BodyDataType.Value = (int)bodyType_value.Value;
        }
        else
        {
            _currentObject.BodyDataType.Value = 0;
            _currentObject.BodyDataType.Type = Instance.BodyData.BType.None;
        }

        GetInstance().Instances[_assetId] = (Instance)_currentObject.Clone();

        using FileStream createStream = File.Create(ProjectPath + "\\" + _currentObject.ProjectPath + "\\" + _currentObject.FileName);
        byte[] buffer = JsonConvert.SerializeObject(_currentObject).Select(c => (byte)c).ToArray();
        createStream.Write(buffer);
        // write all code do different files
        if (_currentObject.Events.Keys.Count > 0)
        {

            foreach (KeyValuePair<Event.EventType, string> item in _currentObject.Events)
            {
                if (_eventsData[item.Key] != null)
                {
                    File.WriteAllText(pathToObjectData + "\\" + item.Value + "" + Program.FileExtensions_ArtCode, _eventsData[item.Key]);

                }// else error
            }
        }
        
    }

    public void WriteObjectCode()
    {
        {
            string instanceMain = "";
            // main
            instanceMain += "object " + _currentObject.Name + "\n";
            foreach (Variable item in _currentObject.Variables)
            {
                instanceMain += "local " + item.Type.ToString().ToLower()["vtype".Length..] + " " + item.Name + "\n";
            }
            foreach (KeyValuePair<Event.EventType, string> item in _currentObject.Events)
            {
                instanceMain += "define " + item.Key + "\n";
            }
            instanceMain += "@end\n";
            // default
            instanceMain += "function " + _currentObject.Name + ":" + "DEF_VALUES" + "\n";
            if (_currentObject.Sprite != null)
            {
                instanceMain += $"set_self_sprite(get_sprite(\"{_currentObject.Sprite.Name}\"))" + "\n";
            }
            instanceMain += $"set_body_type(\"{_currentObject.BodyDataType.Type.ToString()}\", {_currentObject.BodyDataType.Value.ToString()})" + "\n";
            foreach (Variable item in _currentObject.Variables)
            {
                if (item.Default != null && item.Default.Length > 0)
                {
                    switch (item.Type)
                    {
                        case Variable.VariableType.VTypeObject:
                            // can not
                            break;
                        case Variable.VariableType.VTypeScene:
                            // can not
                            break;
                        case Variable.VariableType.VTypeSprite:
                            instanceMain += $"{item.Name} := get_sprite(\"{item.Default}\")\n";
                            break;
                        case Variable.VariableType.VTypeTexture:
                            instanceMain += $"{item.Name} := get_texture(\"{item.Default}\")\n";
                            break;
                        case Variable.VariableType.VTypeSound:
                            instanceMain += $"{item.Name} := get_sound(\"{item.Default}\")\n";
                            break;
                        case Variable.VariableType.VTypeMusic:
                            instanceMain += $"{item.Name} := get_music(\"{item.Default}\")\n";
                            break;
                        case Variable.VariableType.VTypeFont:
                            instanceMain += $"{item.Name} := get_font(\"{item.Default}\")\n";
                            break;
                        case Variable.VariableType.VTypePoint:
                            string[] pt = item.Default.Split(':');
                            instanceMain += $"{item.Name} := new_point( {pt[0]}, {pt[1]})\n";
                            break;
                        case Variable.VariableType.VTypeRectangle:
                            break;
                        default:
                            instanceMain += item.Name + " := " + item.Default + "\n";
                            break;
                    }
                }
            }
            instanceMain += "@end\n";
            // events
            foreach (KeyValuePair<Event.EventType, string> item in _currentObject.Events)
            {
                instanceMain += "function " + _currentObject.Name + ":" + item.Key + "\n";
                instanceMain += _eventsData[item.Key] + "\n";
                instanceMain += "@end\n";
            }
            File.WriteAllText(ProjectPath + "\\object\\" + _currentObject.Name + "\\main" + Program.FileExtensions_ArtCode, instanceMain);
        }
    }

    private void button9_Click(object sender, EventArgs e)
    {
        Save();
    }

    private void button4_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void Varible_listbox_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        if (Varible_listbox.SelectedItem != null)
        {
            Variable varToEdit = _currentObject.Variables.Find(x => x.Name == Varible_listbox.SelectedItem.ToString().Split('[').First());
            //.Split('[').First()
            VariableEditor variable = new VariableEditor(varToEdit);
            if (variable.ShowDialog() == DialogResult.OK)
            {
                Varible_listbox.Items.Clear();
                foreach (Variable item in _currentObject.Variables)
                {
                    Varible_listbox.Items.Add(item.Name + '[' + item.Type.ToString() + ']');
                }
            }
        }
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (GetInstance().Sprites.ContainsKey(comboBox1.Text))
        {
            _currentObject.Sprite = GetInstance().Sprites[comboBox1.Text];
        }
        else
        {
            _currentObject.Sprite = null;
        }
    }


    private void bodyType_IsSolid_CheckedChanged(object sender, EventArgs e)
    {
        if (!bodyType_IsSolid.Checked)
        {
            _currentObject.BodyDataType.Type = Instance.BodyData.BType.None;
        }
        bodyType_circle.Enabled = bodyType_IsSolid.Checked;
        bodyType_mask.Enabled = bodyType_IsSolid.Checked;
        bodyType_rect.Enabled = bodyType_IsSolid.Checked;
        bodyType_value.Enabled = bodyType_IsSolid.Checked;

    }

    private void Event_listobx_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        listBox1_SelectedIndexChanged(sender, null);
        button8_Click(sender, e);
    }
    private void Event_treeview_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        button8_Click(sender, e);
    }

    private void button10_Click(object sender, EventArgs e)
    {
        if (Event_listobx.SelectedItem == null)
        {
            if (!AddEvent()) return;
            if (Event_listobx.Items.Count >= 1) Event_listobx.SelectedItem = Event_listobx.Items[^1];
            else return;
        }

        string sbPath = ProjectPath + "\\object\\StandardBehaviour";
        List<string> list = new List<string>(){ "<new>" };
        if (Directory.Exists(sbPath))
        {
            foreach (string item in Directory.GetFiles(sbPath))
            {
                list.Add(Path.GetFileName(item).Split('.').First());
            }
        }
        else
        {
            Directory.CreateDirectory(sbPath);
        }
        PicFromList picFrom = new PicFromList(list);
        if(picFrom.ShowDialog() == DialogResult.OK) { 
            if(picFrom.Selected == "<new>")
            {
                string behName = GetString.Get("Give name of new Standard Behaviour");
                if(behName == null)
                {
                    MessageBox.Show("Wrong name, can not create new Standard Begaviour", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                CodeEditor codeEditor = new CodeEditor( "//Standard Behaviour " + behName);
                if(codeEditor.ShowDialog() == DialogResult.OK) {
                    File.WriteAllLines(sbPath + "\\" + behName + ".sbh", codeEditor.Code);
                    _eventsData[(Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString())] = String.Join("\n", codeEditor.Code);
                    Event_treeview.Nodes.Clear();
                    foreach (string line in codeEditor.Code)
                    {
                        Event_treeview.Nodes.Add(line);
                    }
                }
                else
                {
                    {
                        MessageBox.Show("Can not create new Standard Begaviour", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            else
            {
                switch (MessageBox.Show("Do You like to insert or edit Standard Behaviour?\nYes to insert.\nNo to edit", "Standard Behaviour", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        if(File.Exists(sbPath + "\\" + picFrom.Selected + ".sbh"))
                        {
                            _eventsData[(Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString())] = File.ReadAllText(sbPath + "\\" + picFrom.Selected + ".sbh");
                            Event_treeview.Nodes.Clear();
                            foreach (string line in _eventsData[(Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString())].Split('\n'))
                            {
                                Event_treeview.Nodes.Add(line);
                            }
                        }
                        break;
                    case DialogResult.No:
                        CodeEditor codeEditor = new CodeEditor( File.ReadAllText(sbPath + "\\" + picFrom.Selected + ".sbh"));
                        if (codeEditor.ShowDialog() == DialogResult.OK)
                        {
                            File.WriteAllLines(sbPath + "\\" + picFrom.Selected + ".sbh", codeEditor.Code);
                            _eventsData[(Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString())] = String.Join("\n", codeEditor.Code);
                            Event_treeview.Nodes.Clear();
                            foreach (string line in codeEditor.Code)
                            {
                                Event_treeview.Nodes.Add(line);
                            }
                        }
                        break;
                    case DialogResult.Cancel:
                        return;
                }
            }
        }
    }
}