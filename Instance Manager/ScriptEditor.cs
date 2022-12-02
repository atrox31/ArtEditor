using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using static ArtCore_Editor.GameProject;

namespace ArtCore_Editor
{
    public partial class ScriptEditor : Form
    {
        public class Function
        {
            public enum type
            {
                _null, _int, _float, _bool, _instance, _sprite, _texture, _sound, _music, _font, _point, _rectangle, _string, _color
            };
            type ReturnType { get; }
            string AditionalText { get; }
            public string Name { get; }
            string MainText;
            public List<type> Arguments;
            public bool IsType(string ThisType)
            {
                return Enum.TryParse(typeof(type), '_' + ThisType, out _);
            }
            public bool IsType(type ThisType)
            {
                return ThisType == ReturnType;
            }
            public string GetNormalname()
            {
                string tmp = Name.Replace('_', ' ');
                return char.ToUpper(tmp[0]) + tmp.Substring(1);
            }
            public string GetCategory()
            {
                string tmp = Name.Split('_')[0];
                return char.ToUpper(tmp[0]) + tmp.Substring(1);
            }

            public Function(string line)
            {

                ReturnType = type._null;
                AditionalText = "";
                MainText = "";
                Name = "<error>";
                Arguments = new List<type>();
                ReturnedArguments = new Dictionary<int, string>();

                var segment = line.Split(';');

                var tmp_1 = segment[0].Split(' ');
                //ReturnType = (type)Enum.Parse(typeof(type), '_' + tmp_1[0]);
                if(Enum.TryParse(typeof(type), '_' + tmp_1[0], true,out _))
                {
                    ReturnType = (type)Enum.Parse(typeof(type), '_' + tmp_1[0]);
                }
                else
                {
                    AditionalText += "[ERROR] Function return value type is invalid, do not use this script until update! ";
                }

                string[] tmp_2 = tmp_1[1].Split('(');
                Name = tmp_2[0];

                string tmp_arguments = segment[0].Substring(segment[0].IndexOf('('));
                tmp_arguments = tmp_arguments.Substring(1, tmp_arguments.Length - 2);

                if (tmp_arguments.Length > 0)
                {

                    foreach (var item in tmp_arguments.Split(','))
                    {
                        var tmp = item;
                        if (item[0] == ' ')
                        {
                            tmp = tmp.Substring(1);
                        }
                        if(Enum.TryParse(typeof(type), "_" + tmp.Split(' ')[0], true, out _))
                        {
                            Arguments.Add((type)Enum.Parse(typeof(type), "_" + tmp.Split(' ')[0]));
                        }
                        else
                        {
                            Arguments.Add(type._null);
                        }
                    }
                }

                MainText = segment[1];

                if (segment.Length > 2)
                    AditionalText += segment[2];

                //point new_point(float x, float y);Make point <float>, <float>.; New point from value or other.

            }

            public Dictionary<int, string> ReturnedArguments;

            public void MakeLinkText(ref LinkLabel linkLabel)
            {
                linkLabel.Links.Clear();
                linkLabel.Text = "";

                int lStart = 0;
                int fno = 0;

                var MainText_copy = String.Copy(MainText);

                for (int i = 0; i < MainText_copy.Length; i++)
                {
                    if (MainText_copy[i] == '<')
                    {
                        lStart = i + 1;
                        continue;
                    }
                    if (MainText_copy[i] == '>')
                    {
                        int oldValueLen = i - lStart;
                        string value = MainText_copy.Substring(lStart, oldValueLen);
                        string newValue = null;
                        if (ReturnedArguments.Keys.Contains(fno))
                        {
                            newValue = ReturnedArguments[fno];
                            MainText_copy = MainText_copy.Remove(i - oldValueLen, oldValueLen);
                            MainText_copy = MainText_copy.Insert(i - oldValueLen, newValue);
                            int diffrence = newValue.Length - oldValueLen;
                            i += diffrence;
                            oldValueLen += diffrence;
                        }

                        // link no + varible + selected_value
                        if (fno >= Arguments.Count)
                        {
                            linkLabel.Links.Add(lStart, oldValueLen, linkLabel.Links.Count.ToString() + ":" + "[ERR]" + (newValue != null ? ":" + newValue : ""));
                        }
                        else
                        {
                            linkLabel.Links.Add(lStart, oldValueLen, linkLabel.Links.Count.ToString() + ":" + Arguments[fno].ToString() + (newValue != null ? ":" + newValue : ""));
                        }

                        fno++;
                    }

                }
                linkLabel.Text = MainText_copy;

            }
            public void MakeAditionalText(ref Label label)
            {
                label.Text = AditionalText;
            }

        }
        List<Function> functions = new List<Function>();
        Function.type RequiredType;
        Instance instance;
        public static List<Function> FunctionsList = null;
        public string ReturnValue = "";

        public ScriptEditor(Function.type RequiredType, Instance instance)
        {
            InitializeComponent(); Program.ApplyTheme(this);
            this.RequiredType = RequiredType;
            this.instance = instance;


            if (FunctionsList == null)
            {
                FunctionsList = new List<Function>();
                foreach (var line in System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\" + "Core\\AScript.lib"))
                {
                    if (line.Length == 0) continue;
                    if (line.StartsWith("//")) continue;
                    if (line == null) continue;
                    string copy = line;
                    if (String.Concat(copy.Where(c => !Char.IsWhiteSpace(c))).Length == 0) continue;
                    FunctionsList.Add(new Function(line));
                }
            }

            foreach (var fun in FunctionsList)
            {
                if (fun.IsType(RequiredType))
                {
                    functions.Add(fun);
                }

            }
            if (RequiredType != Function.type._null)
            {
                comboBox1.Items.Add("Value");
            }

        }

        private void ScriptEditor_Load(object sender, EventArgs e)
        {
            foreach (var function in functions)
            {
                var Category = function.GetCategory();
                if (!comboBox1.Items.Contains(Category))
                {
                    comboBox1.Items.Add(Category);
                }
            }
            button1.Enabled = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            if (comboBox1.SelectedItem.ToString() == "Value")
            {
                comboBox2.Items.Add("New Value");
                comboBox2.Items.Add("Varible");
            }
            else
            {
                foreach (var function in functions)
                {
                    if (function.GetCategory() == comboBox1.SelectedItem.ToString())
                    {
                        comboBox2.Items.Add(function.GetNormalname());
                    }
                }
            }
            button1.Enabled = false;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "New Value")
            {
                VaribleEditor varibleEditor = new VaribleEditor(true, RequiredType.ToString().Substring(1));
                if (varibleEditor.ShowDialog() == DialogResult.OK)
                {
                    // populate
                    linkLabel1.Text = varibleEditor._Default;
                    linkLabel1.Links.Clear();
                    button1.Enabled = true;

                    ReturnValue = varibleEditor._Default;
                }
                return;
            }
            if (comboBox2.SelectedItem.ToString() == "Varible")
            {
                Varible.type ConvertedEnum = (Varible.type)Enum.Parse(typeof(Varible.type), RequiredType.ToString().Substring(1).ToUpper());

                List<Varible> VarList = instance.Varible.FindAll(obj => obj.Type == ConvertedEnum);
                string answer = PicFromList.Get(((IEnumerable)VarList).Cast<Varible>()
                                 .Select(x => x.Name)
                                 .ToList());
                if (answer != null)
                {
                    linkLabel1.Text = answer;
                    linkLabel1.Links.Clear();
                    button1.Enabled = true;
                    ReturnValue = answer;
                }


                return;
            }

            foreach (var function in functions)
            {
                if (function.GetNormalname() == comboBox2.SelectedItem.ToString())
                {
                    function.MakeLinkText(ref linkLabel1);
                    function.MakeAditionalText(ref label1);
                    button1.Enabled = false;

                    if (function.Arguments.Count == 0)
                    {
                        ReturnValue = function.Name + "()";
                        button1.Enabled = true;
                    }

                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DialogResult = DialogResult.OK;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string[] target = (e.Link.LinkData as string).Split(':');
            if (target.Length == 2 && target[1] == "[ERR]") return;
            ScriptEditor scriptEditor = new ScriptEditor((Function.type)Enum.Parse(typeof(Function.type), target[1]), instance);
            if (scriptEditor.ShowDialog() == DialogResult.OK)
            {
                // populate
                int linkNo = Convert.ToInt32(target[0]);
                Function answer = FunctionsList.Find(x => x.GetNormalname() == comboBox2.Text);
                answer.ReturnedArguments[linkNo] = scriptEditor.ReturnValue;

                answer.MakeLinkText(ref linkLabel1);

                if (answer.ReturnedArguments.Count == answer.Arguments.Count)
                {
                    button1.Enabled = true;

                    ReturnValue = answer.Name + "(";
                    for (int i = 0; i < answer.ReturnedArguments.Count; i++)
                    {
                        ReturnValue += answer.ReturnedArguments[i] + ", ";
                    }
                    ReturnValue = ReturnValue.Substring(0, ReturnValue.Length - 2);
                    ReturnValue += ")";
                }
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }
    }
}
