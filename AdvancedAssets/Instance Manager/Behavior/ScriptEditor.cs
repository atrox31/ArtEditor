using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ArtCore_Editor.AdvancedAssets.Instance_Manager.variable;
using ArtCore_Editor.Enums;
using ArtCore_Editor.Pick_forms;

namespace ArtCore_Editor.AdvancedAssets.Instance_Manager.Behavior;

public partial class ScriptEditor : Form
{
    // functions that are returned from child forms
    private readonly List<Function> _functions = new List<Function>();
    // what this form need to get
    private readonly Variable.VariableType _requiredType;
    
    private readonly List<Variable> _variables;
    // static ArtCode.lib content
    private static List<Function> _functionsList = null;
    // what is this form returning (include child returns)
    public string ReturnValue = "";

    public static void UpdateFunctionList()
    {
        _functionsList?.Clear();
        _functionsList = new List<Function>();
        foreach (string line in System.IO.File.ReadAllLines(Program.ProgramDirectory + "\\" + "Core\\AScript.lib"))
        {
            if (line.Length == 0) continue;
            if (line.StartsWith("//")) continue;
            if (string.Concat(line.Where(c => !char.IsWhiteSpace(c))).Length == 0) continue;
            _functionsList.Add(new Function(line));
        }
    }

    public ScriptEditor(Variable.VariableType requiredType, List<Variable> variables)
    {
        InitializeComponent(); Program.ApplyTheme(this);
        _requiredType = requiredType;
        _variables = variables;


        if (_functionsList == null)
        {
            UpdateFunctionList();
        }

        if (_functionsList != null)
        {
            foreach (Function fun in _functionsList)
            {
                if (fun.ReturnType == requiredType)
                {
                    _functions.Add(fun);
                }
            }
        }

        if (requiredType != Variable.VariableType.VTypeNull)
        {
            comboBox1.Items.Add("Value");
        }

    }

    private void ScriptEditor_Load(object sender, EventArgs e)
    {
        foreach (Function function in _functions)
        {
            string category = function.GetCategory();
            if (!comboBox1.Items.Contains(category))
            {
                comboBox1.Items.Add(category);
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
            comboBox2.Items.Add("Variable");
        }
        else
        {
            foreach (Function function in _functions)
            {
                if (function.GetCategory() == comboBox1.SelectedItem.ToString())
                {
                    comboBox2.Items.Add(function.GetNormalName());
                }
            }
        }
        button1.Enabled = false;
    }

    private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (comboBox2.SelectedItem.ToString() == "New Value")
        {
            VariableEditor variableEditor = new VariableEditor("Value", _requiredType);
            if (variableEditor.ShowDialog() != DialogResult.OK) return;
            // populate
            linkLabel1.Text = variableEditor.Default;
            linkLabel1.Links.Clear();
            button1.Enabled = true;

            ReturnValue = variableEditor.Default;
            return;
        }
        if (comboBox2.SelectedItem.ToString() == "Variable" && _variables.Count > 0) // scene not have variable
        {
            Variable.VariableType convertedEnum = _requiredType;

            List<Variable> varList = _variables.FindAll(obj => obj.Type == convertedEnum);
            string answer = PicFromList.Get(((IEnumerable)varList).Cast<Variable>()
                .Select(x => x.Name)
                .ToList());
            if (answer == null) return;
            linkLabel1.Text = answer;
            linkLabel1.Links.Clear();
            button1.Enabled = true;
            ReturnValue = answer;
            return;
        }

        foreach (Function function in _functions)
        {
            if (function.GetNormalName() == comboBox2.SelectedItem.ToString())
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
        string[] target = (e.Link.LinkData as string)?.Split(':');
        if (target != null && target.Length == 2 && target[1] == "[ERR]") return;
        ScriptEditor scriptEditor = new ScriptEditor((Variable.VariableType)Enum.Parse(typeof(Variable.VariableType), target[1]), _variables);
        if (scriptEditor.ShowDialog() == DialogResult.OK)
        {
            // populate
            int linkNo = Convert.ToInt32(target[0]);
            Function answer = _functionsList.Find(x => x.GetNormalName() == comboBox2.Text);
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
                //ReturnValue = ReturnValue.Substring(0, ReturnValue.Length - 2);
                ReturnValue = ReturnValue[..^2];
                ReturnValue += ")";
            }
        }

    }

    private void button2_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.No;
    }
}