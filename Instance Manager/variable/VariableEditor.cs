using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Forms;
using ArtCore_Editor.Enums;

namespace ArtCore_Editor.Instance_Manager.varible;

public partial class VariableEditor : Form
{
    public static Variable GetValue()
    {
        VariableEditor v = new VariableEditor();
        return v.ShowDialog() == DialogResult.OK ? new Variable(v.VariableType, v.Name, v.Default) : null;
    }

    private Variable.VariableType VariableType { get; }
    public string Default;
    public Variable Variable { get; private set; }

    // enum can be edited only if editor wants to, there is no possibility to create custom enum
    private bool _isEnum;
    private List<string> _enumValues;

    private readonly bool _defaultIsNeeded = false;

    public VariableEditor(Variable variable = null)
    {
        InitializeComponent(); Program.ApplyTheme(this);
        FieldType.DataSource = Enum.GetValues(typeof(Variable.VariableType));
        Variable = variable;
        _defaultIsNeeded = false;

        if (variable == null) return;
        FieldName.Text = variable.Name;
        FieldDefault.Text = variable.Default;
        FieldType.SelectedIndex = (int)variable.Type;
        _isEnum = false;
    }
    public VariableEditor(bool getValueForCodeEditor, string varType, string value = "", List<string> enumValues = null)
    {
        InitializeComponent(); Program.ApplyTheme(this);
        _defaultIsNeeded = true;
        FieldName.Text = "Value";
        FieldName.ReadOnly = true;
        FieldDefault.Text = value;
        FieldType.DataSource = Enum.GetValues(typeof(Variable.VariableType));
        VariableType = (Variable.VariableType)Enum.Parse(typeof(Variable.VariableType), varType.ToUpper());
        FieldType.SelectedItem = VariableType;
        FieldType.Enabled = false;


        if (VariableType == Variable.VariableType.VTypeEnum)
        {
            _isEnum = true;
            _enumValues = enumValues;
        }
        else
        {
            _isEnum = false;
        }
    }

    private void button1_Click(object sender, EventArgs e)
    {// apply
        if (Functions.ErrorCheck(FieldName.TextLength > 3, "Wrong Variable name. Minimum 3 char, maximum 24 char")) return;
        if (Functions.ErrorCheck(FieldName.TextLength < 24, "Wrong Variable name. Minimum 3 char, maximum 24 char.")) return;
        Name = FieldName.Text;

        Enum.TryParse(FieldType.SelectedValue.ToString(), out Variable.VariableType type);

        if (_defaultIsNeeded)
        {
            if (Functions.ErrorCheck(FieldDefault.Text.Length > 0, "Type variable value in default field.")) return;
        }

        if (FieldDefault.Text.Length > 0)
        {

            switch (type)
            {
                case Variable.VariableType.VTypeInt:
                {
                    if (Functions.ErrorCheck(int.TryParse(FieldDefault.Text, out int _), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypeFloat:
                {
                    if (Functions.ErrorCheck(float.TryParse(FieldDefault.Text, out float _), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypeBool:
                {
                    if (Functions.ErrorCheck(bool.TryParse(FieldDefault.Text, out bool _), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypeInstance:
                {
                    if (Functions.ErrorCheck(false, "instance is defined on runtime, not allow to default in editor!")) return;
                }
                    break;
                case Variable.VariableType.VTypeObject:
                {
                    if (Functions.ErrorCheck(GameProject.GetInstance().Instances.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypeSprite:
                {
                    if (Functions.ErrorCheck(GameProject.GetInstance().Sprites.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypeTexture:
                {
                    if (Functions.ErrorCheck(GameProject.GetInstance().Textures.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypeSound:
                {
                    if (Functions.ErrorCheck(GameProject.GetInstance().Sounds.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypeMusic:
                {
                    if (Functions.ErrorCheck(GameProject.GetInstance().Music.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypeFont:
                {
                    if (Functions.ErrorCheck(GameProject.GetInstance().Fonts.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypeScene:
                {
                    if (Functions.ErrorCheck(GameProject.GetInstance().Scenes.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypePoint:
                {
                    var point = FieldDefault.Text.Split(':');
                    if (Functions.ErrorCheck(point.Length == 2, "Wrong default value. must be: x:y")) return;
                    if (Functions.ErrorCheck(float.TryParse(point[0], out float _), "Wrong default value. must be: x:y")) return;
                    if (Functions.ErrorCheck(float.TryParse(point[1], out float _), "Wrong default value. must be: x:y")) return;
                }
                    break;
                case Variable.VariableType.VTypeColor:
                {
                    if (!FieldDefault.Text.StartsWith('#'))
                    {
                        FieldDefault.Text = FieldDefault.Text.Insert(0, "#");
                    }
                    if (Functions.ErrorCheck(Functions.IsHex(FieldDefault.Text), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypeString:
                {
                    if (!FieldDefault.Text.StartsWith('"'))
                    {
                        FieldDefault.Text = FieldDefault.Text.Insert(0, "\"");
                    }
                    if (!FieldDefault.Text.EndsWith('"'))
                    {
                        FieldDefault.Text += "\"";
                    }
                    if (Functions.ErrorCheck(FieldDefault.Text.Count(c => c == '"') == 2, "Wrong default value.")) return;
                }
                    break;

                case Variable.VariableType.VTypeNull:
                    break;
                case Variable.VariableType.VTypeRectangle:
                    var rect = FieldDefault.Text.Split(':');
                    if (Functions.ErrorCheck(rect.Length == 4, "Wrong default value. must be: x:y")) return;
                    if (Functions.ErrorCheck(float.TryParse(rect[0], out float _), "Wrong default value. must be: x:y:w:h")) return;
                    if (Functions.ErrorCheck(float.TryParse(rect[1], out float _), "Wrong default value. must be: x:y:w:h")) return;
                    if (Functions.ErrorCheck(float.TryParse(rect[2], out float _), "Wrong default value. must be: x:y:w:h")) return;
                    if (Functions.ErrorCheck(float.TryParse(rect[3], out float _), "Wrong default value. must be: x:y:w:h")) return;
                    break;
                case Variable.VariableType.VTypeEnum:
                    if (Functions.ErrorCheck(_enumValues != null, "Can not create custom enums.")) return;
                    if (Functions.ErrorCheck(_enumValues.Contains(FieldDefault.Text), "Wrong default value.")) return;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        Default = FieldDefault.Text;
        if (Variable == null)
        {
            Variable = new Variable(type, Name, Default);
        }
        else
        {
            Variable.Name = Name;
            Variable.Default = Default;
            Variable.Type = type;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void button2_Click(object sender, EventArgs e)
    {// cancel
        DialogResult = DialogResult.Cancel;
        Close();
    }

    private void FieldType_SelectedIndexChanged(object sender, EventArgs e)
    {
        // check if variable exists
        try
        {
            Enum.TryParse<Variable.VariableType>(FieldType.SelectedValue.ToString(), out Variable.VariableType status);
        }
        catch (ArgumentException)
        {
            button3.Enabled = false;
        }
        button3.Enabled = true;

    }

    private void button3_Click(object sender, EventArgs e)
    {
        Enum.TryParse<Variable.VariableType>(FieldType.SelectedValue.ToString(), out var status);
        // must check if enum is user defined or ArtCore defined
        if (status == Variable.VariableType.VTypeEnum && _enumValues == null) return;
        FieldDefault.Text = status switch
        {
            Variable.VariableType.VTypeObject => PicFromList.Get(GameProject.GetInstance().Instances.Keys.ToList()),
            Variable.VariableType.VTypeSprite => PicFromList.Get(GameProject.GetInstance().Sprites.Keys.ToList()),
            Variable.VariableType.VTypeTexture => PicFromList.Get(GameProject.GetInstance().Textures.Keys.ToList()),
            Variable.VariableType.VTypeSound => PicFromList.Get(GameProject.GetInstance().Sounds.Keys.ToList()),
            Variable.VariableType.VTypeMusic => PicFromList.Get(GameProject.GetInstance().Music.Keys.ToList()),
            Variable.VariableType.VTypeFont => PicFromList.Get(GameProject.GetInstance().Fonts.Keys.ToList()),
            Variable.VariableType.VTypeScene => PicFromList.Get(GameProject.GetInstance().Scenes.Keys.ToList()),
            Variable.VariableType.VTypeEnum => PicFromList.Get(_enumValues),
            _ => "" //default
        };
    }
}