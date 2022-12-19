using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using ArtCore_Editor.Enums;
using ArtCore_Editor.Instance_Manager;
using ArtCore_Editor.Instance_Manager.code;
using ArtCore_Editor.Instance_Manager.varible;
using static ArtCore_Editor.GameProject;

namespace ArtCore_Editor;

public partial class ScriptEditor : Form
{
    // functions that are returned from child forms
    private readonly List<Function> _functions = new List<Function>();
    // what this form need to get
    private readonly Variable.VariableType _requiredType;
    // ref to child
    private readonly Instance _instance;
    // static ArtCode.lib content
    public static List<Function> FunctionsList = null;
    // what is this form returning (include child returns)
    public string ReturnValue = "";

    public static void ClearFunctionList()
    {
        FunctionsList?.Clear();
    }

    public ScriptEditor(Variable.VariableType requiredType, Instance instance)
    {
        InitializeComponent(); Program.ApplyTheme(this);
        _requiredType = requiredType;
        _instance = instance;


        if (FunctionsList == null)
        {
            FunctionsList = new List<Function>();
            foreach (string line in System.IO.File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "\\" + "Core\\AScript.lib"))
            {
                if (line.Length == 0) continue;
                if (line.StartsWith("//")) continue;
                if (string.Concat(line.Where(c => !char.IsWhiteSpace(c))).Length == 0) continue;
                FunctionsList.Add(new Function(line));
            }
        }

        foreach (var fun in FunctionsList)
        {
            if (fun._returnType == requiredType)
            {
                _functions.Add(fun);
            }

        }
        if (requiredType != Variable.VariableType.VTypeNull)
        {
            comboBox1.Items.Add("Value");
        }

    }

    private void ScriptEditor_Load(object sender, EventArgs e)
    {
        foreach (var function in _functions)
        {
            var category = function.GetCategory();
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
            foreach (var function in _functions)
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
            VariableEditor variableEditor = new VariableEditor(true, _requiredType.ToString());
            if (variableEditor.ShowDialog() == DialogResult.OK)
            {
                // populate
                linkLabel1.Text = variableEditor.Default;
                linkLabel1.Links.Clear();
                button1.Enabled = true;

                ReturnValue = variableEditor.Default;
            }
            return;
        }
        if (comboBox2.SelectedItem.ToString() == "Variable")
        {
            Variable.VariableType convertedEnum = (Variable.VariableType)Enum.Parse(typeof(Variable.VariableType), _requiredType.ToString().Substring(1).ToUpper());

            List<Variable> varList = _instance.Variables.FindAll(obj => obj.Type == convertedEnum);
            string answer = PicFromList.Get(((IEnumerable)varList).Cast<Variable>()
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

        foreach (var function in _functions)
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
        string[] target = (e.Link.LinkData as string).Split(':');
        if (target.Length == 2 && target[1] == "[ERR]") return;
        ScriptEditor scriptEditor = new ScriptEditor((Variable.VariableType)Enum.Parse(typeof(Variable.VariableType), target[1]), _instance);
        if (scriptEditor.ShowDialog() == DialogResult.OK)
        {
            // populate
            int linkNo = Convert.ToInt32(target[0]);
            Function answer = FunctionsList.Find(x => x.GetNormalName() == comboBox2.Text);
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