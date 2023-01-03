using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ArtCore_Editor.Enums;

namespace ArtCore_Editor.AdvancedAssets.Instance_Manager.Behavior;

// class represent functions given in script editor
public class Function
{
    public string Name { get; }
    // function return type
    public  Variable.VariableType ReturnType { get; }
    // function additional text that contains info. last argument in lib
    private readonly string _additionalText;
    // main text that contains info and links to click
    private readonly string _mainText;
    // arguments that must be fill before exit
    public readonly List<Variable.VariableType> Arguments;

    private string TranslateEnumToValidString(Variable.VariableType input)
    {
        return input.ToString().Substring("VType".Length).ToLower();
    }
    private Variable.VariableType TranslateStringToValidEnum(string input)
    {
        if (Enum.TryParse(typeof(Variable.VariableType), "VType" + input,true, out _))
        {
            return (Variable.VariableType)Enum.Parse(typeof(Variable.VariableType), "VType" + input, true);
        }
        else
        {
            return Variable.VariableType.VTypeNull;
        }
    }

    // Second list box, get all text with normal spelling
    public string GetNormalName()
    {
        string tmp = Name.Replace('_', ' ');
        return char.ToUpper(tmp[0]) + tmp.Substring(1);
    }
    // first listbox only category
    public string GetCategory()
    {
        string tmp = Name.Split('_')[0];
        return char.ToUpper(tmp[0]) + tmp.Substring(1);
    }

    public Function(string line)
    {
        ReturnType = Variable.VariableType.VTypeNull;
        _additionalText = "";
        _mainText = "";
        Name = "<error>";
        Arguments = new List<Variable.VariableType>();
        ReturnedArguments = new Dictionary<int, string>();

        string[] segment = line.Split(';');

        string[] variableType = segment[0].Split(' ');
        ReturnType = TranslateStringToValidEnum(variableType[0]);

        string[] tmp2 = variableType[1].Split('(');
        Name = tmp2[0];

        string tmpArguments = segment[0].Substring(segment[0].IndexOf('('));
        tmpArguments = tmpArguments.Substring(1, tmpArguments.Length - 2);

        if (tmpArguments.Length > 0)
        {

            foreach (string item in tmpArguments.Split(','))
            {
                string tmp = item;
                if (item[0] == ' ')
                {
                    tmp = tmp.Substring(1);
                }
                Arguments.Add(TranslateStringToValidEnum(tmp.Split(' ')[0]));
                
            }
        }
        // copy string
        _mainText = segment[1].Substring(0);

        if (segment.Length > 2)
            _additionalText += segment[2];

        //point new_point(float x, float y);Make point <float>, <float>.; New point from value or other.
        // from libray
    }

    public readonly Dictionary<int, string> ReturnedArguments;

    public void MakeLinkText(ref LinkLabel linkLabel)
    {
        linkLabel.Links.Clear();
        linkLabel.Text = "";

        int lStart = 0;
        int fno = 0;

        // copy string
        string mainTextCopy = _mainText.Substring(0);

        for (int i = 0; i < mainTextCopy.Length; i++)
        {
            if (mainTextCopy[i] == '<')
            {
                lStart = i + 1;
                continue;
            }
            if (mainTextCopy[i] == '>')
            {
                int oldValueLen = i - lStart;
                //string value = mainTextCopy.Substring(lStart, oldValueLen);
                string newValue = null;
                if (ReturnedArguments.Keys.Contains(fno))
                {
                    newValue = ReturnedArguments[fno];
                    mainTextCopy = mainTextCopy.Remove(i - oldValueLen, oldValueLen);
                    mainTextCopy = mainTextCopy.Insert(i - oldValueLen, newValue);
                    int difference = newValue.Length - oldValueLen;
                    i += difference;
                    oldValueLen += difference;
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
        linkLabel.Text = mainTextCopy;

    }
    public void MakeAditionalText(ref Label label)
    {
        label.Text = _additionalText;
    }

}