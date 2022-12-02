using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static ArtCore_Editor.GameProject;

namespace ArtCore_Editor
{
    public partial class ObjectManager : Form
    {

        string aid;
        GameProject.Instance instance;
        Dictionary<Event.EventType, string> events_data;
        List<Varible> LocalVaribles;

        public ObjectManager(string AssetId = null)
        {
            InitializeComponent(); Program.ApplyTheme(this);

            foreach (var sprite in GameProject.GetInstance().Sprites)
            {
                comboBox1.Items.Add(sprite.Key);
            }

            aid = AssetId;
            events_data = new Dictionary<Event.EventType, string>();
            LocalVaribles = new List<Varible>();

            if (AssetId != null)
            {
                string openingFileName = GameProject.ProjectPath + "\\" + GameProject.GetInstance().Instances[AssetId].ProjectPath + "\\" + GameProject.GetInstance().Instances[AssetId].FileName;
                if (!File.Exists(openingFileName))
                {
                    MessageBox.Show("File: '" + openingFileName + "' not exists", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (StreamReader reader = new StreamReader(File.Open(openingFileName, FileMode.Open)))
                {
                    instance = JsonConvert.DeserializeObject<Instance>(reader.ReadToEnd());
                }


                textBox1.Text = instance.Name;
                if (instance.Sprite != null)
                {
                    comboBox1.Text = instance.Sprite.Name;
                }
                else
                {
                    comboBox1.Text = "<default>";
                    //instance.Sprite = Sprite.Default();
                }
                /*
                if(instance.DBEntry != null)
                {
                    comboBox2.Text = instance.DBEntry.Name;
                }
                */

                if(instance.BodyType.Type != Instance.Body.type.NONE)
                {
                    bodyType_mask.Checked = (instance.BodyType.Type == Instance.Body.type.SPRITE);
                    bodyType_rect.Checked = (instance.BodyType.Type == Instance.Body.type.RECT);
                    bodyType_circle.Checked = (instance.BodyType.Type == Instance.Body.type.CIRCLE);
                    bodyType_value.Value = instance.BodyType.Value;
                    bodyType_IsSolid.Checked = true;
                }
                else
                {
                    bodyType_IsSolid.Checked = false;
                }

                foreach (var item in instance.Varible)
                {
                    Varible_listbox.Items.Add(item.Name + '[' + item.Type.ToString() + ']');
                }
                if (instance.Events.Count > 0)
                {
                    button5.Enabled = true;
                    button8.Enabled = true;
                    foreach (var item in instance.Events)
                    {
                        string path = GameProject.ProjectPath + "\\object\\" + instance.Name + "\\" + item.Value + ".asc";
                        if (File.Exists(path))
                        {
                            Event_listobx.Items.Add(item.Key);
                            events_data[item.Key] = File.ReadAllText(path);
                        }
                        else
                        {
                            MessageBox.Show("File '" + instance.Name + "\\" + item.Value + ".asc" + "' not found");
                            File.CreateText(path).WriteLine("// file not found");
                        }
                    }
                }
            }
            else
            {
                instance = new GameProject.Instance();
            }
        }

        bool AddEvent()
        {
            // add event - bellow Event_listbox
            object_event_picker object_Event_Picker = new object_event_picker();
            if (object_Event_Picker.ShowDialog() == DialogResult.OK)
            {

                Event.EventType type = object_Event_Picker.Type;
                {
                    if (Event_listobx.SelectedItem != null)
                    {
                        if (Functions.ErrorCheck(
                            instance.Events.TryGetValue(
                                (Event.EventType)Enum.Parse(
                                    typeof(Event.EventType),
                                    Event_listobx.SelectedItem.ToString()
                                    ), out _
                                ), "Event exists!"))
                        {
                            Event_listobx.SelectedItem = type.ToString();
                            return false;
                        }
                    }
                    instance.Events.Add(type, type.ToString());
                    events_data.Add(type, "//" + type.ToString());
                    Event_listobx.Items.Add(type.ToString());

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
                    instance.Events.Remove((Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString()));
                    events_data.Remove((Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString()));
                    Event_listobx.Items.Remove(Event_listobx.SelectedItem);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Event_listobx.SelectedItem != null)
            {
                if (events_data[((Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString()))] != null)
                {
                    Event_treeview.Nodes.Clear();
                    foreach (
                        string line in
                        events_data[
                            (Event.EventType)
                                Enum.Parse(
                                    typeof(Event.EventType),
                                    Event_listobx.SelectedItem.ToString()
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
            }
            ScriptEditor.FunctionsList.Clear();
            ScriptEditor scriptEditor = new ScriptEditor(ScriptEditor.Function.type._null, instance);
            if (scriptEditor.ShowDialog() == DialogResult.OK)
            {
                // populate

                // error with function without args
                if (!scriptEditor.ReturnValue.EndsWith(')'))
                {
                    scriptEditor.ReturnValue += "()";
                }

                int SelectedNode = -1;
                if (Event_treeview.SelectedNode != null)
                {
                    SelectedNode = Event_treeview.SelectedNode.Index;
                }
                if (SelectedNode == -1)
                {
                    Event_treeview.Nodes.Clear();
                    events_data[(Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString())] += "\n" + scriptEditor.ReturnValue;
                    foreach (string item in events_data[(Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString())].Split("\n"))
                    {
                        Event_treeview.Nodes.Add(item);
                    }
                }
                else
                {
                    var code = events_data[(Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString())].Split('\n').ToList();
                    code.Insert(SelectedNode + 1, scriptEditor.ReturnValue);
                    events_data[(Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString())] = String.Join('\n', code);
                    Event_treeview.Nodes.Insert(SelectedNode + 1, scriptEditor.ReturnValue);
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            VaribleEditor _varible = new VaribleEditor();
            if (_varible.ShowDialog() == DialogResult.OK)
            {
                foreach (Varible var in instance.Varible)
                {
                    if (var.Name == _varible.Name)
                    {
                        MessageBox.Show("Varible name exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                instance.Varible.Add(_varible._var);
                Varible_listbox.Items.Clear();
                foreach (var item in instance.Varible)
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
            if (Event_listobx.SelectedItem != null)
            {
                string code = events_data[(Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString())];
                CodeEditor codeEditor = new CodeEditor(null, code);
                if (codeEditor.ShowDialog() == DialogResult.OK)
                {
                    events_data[(Event.EventType)Enum.Parse(typeof(Event.EventType), Event_listobx.SelectedItem.ToString())] = String.Join("\n", codeEditor.Code);
                    Event_treeview.Nodes.Clear();
                    foreach (string line in codeEditor.Code)
                    {
                        Event_treeview.Nodes.Add(line);
                    }
                }
            }

        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (Varible_listbox.SelectedItem != null)
            {
                if (MessageBox.Show("Are You sure to delete varible '" + Varible_listbox.SelectedItem.ToString() + "'", "Delete varible", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    foreach (Varible var in instance.Varible)
                    {
                        if (var.Name == Varible_listbox.SelectedItem.ToString().Split('[').First())
                        {
                            instance.Varible.Remove(var);
                            break;
                        }
                    }
                    Varible_listbox.Items.Remove(Varible_listbox.SelectedItem);
                }
            }
        }
        public void Save()
        {
            if (Functions.ErrorCheck(textBox1.TextLength > 3, "Object name too short")) return;
            if (Functions.ErrorCheck(textBox1.TextLength < 24, "Object name too long")) return;

            instance.Name = textBox1.Text;
            /*
            if (GameProject.GetInstance().Sprites.ContainsKey(comboBox1.Text))
            {
                instance.Sprite = GameProject.GetInstance().Sprites[comboBox1.Text];
            }
            else
            {
                instance.Sprite = null;
            }
            */
            //LocalVaribles.Single(n => n.Name == "self_sprite").Default = (instance.Sprite == null?"null":$"get_sprite(\"{instance.Sprite.Name}\")");
            if (aid == null)
            {
                GameProject.GetInstance().Instances.Add(instance.Name, instance);
                aid = instance.Name;
            }
            else
            {
                if (aid != instance.Name)
                {
                    Functions.RenameKey(GameProject.GetInstance().Instances, aid, instance.Name);
                }
            }

            string path_to_object_data = GameProject.ProjectPath + "\\object\\" + instance.Name;
            if (Directory.Exists(path_to_object_data))
            {
                Directory.Delete(path_to_object_data, true);
            }
            Directory.CreateDirectory(path_to_object_data);

            instance.FileName = instance.Name + ".obj";
            instance.ProjectPath = "\\object\\" + instance.Name + "\\";

            if (bodyType_IsSolid.Checked)
            {
                if (bodyType_mask.Checked) { instance.BodyType.Type = Instance.Body.type.SPRITE; }
                if (bodyType_rect.Checked) { instance.BodyType.Type = Instance.Body.type.RECT; }
                if (bodyType_circle.Checked) { instance.BodyType.Type = Instance.Body.type.CIRCLE; }
                instance.BodyType.Value = (int)bodyType_value.Value;
            }
            else
            {
                instance.BodyType.Value = 0;
                instance.BodyType.Type = Instance.Body.type.NONE;
            }

            GameProject.GetInstance().Instances[aid] = (GameProject.Instance)instance.Clone();

            using (FileStream createStream = File.Create(GameProject.ProjectPath + "\\" + instance.ProjectPath + "\\" + instance.FileName))
            {
                byte[] buffer = JsonConvert.SerializeObject(instance).Select(c => (byte)c).ToArray();
                createStream.Write(buffer);
            }

            if (instance.Events.Keys.Count > 0)
            {

                foreach (var item in instance.Events)
                {
                    if (events_data[item.Key] != null)
                    {
                        File.WriteAllText(path_to_object_data + "\\" + item.Value + ".asc", events_data[item.Key]);

                    }// else error
                }
            }
            // instance_main
            {
                string instance_main = "";
                // main
                instance_main += "object " + instance.Name + "\n";
                /*
                foreach (var item in LocalVaribles)
                {
                    instance_main += "local " + (item.ReadOnly ? "READ_ONLY " : "") + item.Type.ToString().ToLower() + " " + item.Name + "\n";
                }
                */
                foreach (var item in instance.Varible)
                {
                    instance_main += "local " + item.Type.ToString().ToLower() + " " + item.Name + "\n";
                }
                foreach (var item in instance.Events)
                {
                    instance_main += "function " + item.Key + "\n";
                }
                instance_main += "@end\n";
                // default
                instance_main += "function " + instance.Name + ":" + "DEF_VALUES" + "\n";
                if (instance.Sprite != null)
                {
                    instance_main += $"set_self_sprite(get_sprite(\"{instance.Sprite.Name}\"))" + "\n";
                }
                instance_main += $"set_body_type(\"{instance.BodyType.Type.ToString()}\", {instance.BodyType.Value.ToString()})" + "\n";
                foreach (var item in instance.Varible)
                {
                    if (item.Default != null && item.Default.Length > 0)
                    {
                        switch (item.Type)
                        {
                            case Varible.type.OBJECT:

                                break;
                            case Varible.type.COLOR:

                                break;
                            case Varible.type.SCENE:

                                break;
                            case Varible.type.SPRITE:
                                instance_main += $"{item.Name} := get_sprite(\"{ item.Default }\")\n";
                                break;
                            case Varible.type.TEXTURE:
                                instance_main += $"{item.Name} := get_texture(\"{item.Default}\")\n";
                                break;
                            case Varible.type.SOUND:
                                instance_main += $"{item.Name} := get_sound(\"{item.Default}\")\n";
                                break;
                            case Varible.type.MUSIC:
                                instance_main += $"{item.Name} := get_music(\"{item.Default}\")\n";
                                break;
                            case Varible.type.FONT:
                                instance_main += $"{item.Name} := get_font(\"{item.Default}\")\n";
                                break;
                            case Varible.type.POINT:
                                var pt = item.Default.Split(':');
                                instance_main += $"{item.Name} := new_point( {pt[0]}, {pt[1]})\n";
                                break;
                            case Varible.type.RECTANGLE:
                                break;
                            default:
                                instance_main += item.Name + " := " + item.Default + "\n";
                                break;
                        }
                    }
                }
                instance_main += "@end\n";
                // events
                foreach (var item in instance.Events)
                {
                    instance_main += "function " + instance.Name + ":" + item.Key + "\n";
                    instance_main += events_data[item.Key] + "\n";
                    instance_main += "@end\n";
                }
                File.WriteAllText(GameProject.ProjectPath + "\\object\\" + instance.Name + "\\main.asc", instance_main);
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
                Varible var_to_edit = instance.Varible.Find(x => x.Name == Varible_listbox.SelectedItem.ToString().Split('[').First());
                //.Split('[').First()
                VaribleEditor _varible = new VaribleEditor(var_to_edit);
                if (_varible.ShowDialog() == DialogResult.OK)
                {
                    Varible_listbox.Items.Clear();
                    foreach (var item in instance.Varible)
                    {
                        Varible_listbox.Items.Add(item.Name + '[' + item.Type.ToString() + ']');
                    }
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GameProject.GetInstance().Sprites.ContainsKey(comboBox1.Text))
            {
                instance.Sprite = GameProject.GetInstance().Sprites[comboBox1.Text];
            }
            else
            {
                instance.Sprite = null;
            }
        }


        private void bodyType_IsSolid_CheckedChanged(object sender, EventArgs e)
        {
            if (!bodyType_IsSolid.Checked)
            {
                instance.BodyType.Type = Instance.Body.type.NONE;
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

    }
}
