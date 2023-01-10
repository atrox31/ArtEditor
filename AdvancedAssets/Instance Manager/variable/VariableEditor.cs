using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ArtCore_Editor.Enums;
using ArtCore_Editor.Functions;
using ArtCore_Editor.Main;
using ArtCore_Editor.Pick_forms;

namespace ArtCore_Editor.AdvancedAssets.Instance_Manager.variable;

public partial class VariableEditor : Form
{
    private Variable.VariableType VariableType { get; }
    public string Default;
    public Variable CurrentVariable { get; private set; }
    private readonly bool _defaultIsNeeded;

    public VariableEditor(Variable currentVariable = null, bool blockName = false)
    {
        InitializeComponent(); Program.ApplyTheme(this);
        FieldType.DataSource = Enum.GetValues(typeof(Variable.VariableType));
        CurrentVariable = currentVariable;
        _defaultIsNeeded = false;

        if (currentVariable == null) return;
        if (blockName)
        {
            FieldName.ReadOnly = true;
            FieldDefault.Focus();
            FieldDefault.SelectAll();
        }
        FieldName.Text = currentVariable.Name;
        FieldDefault.Text = currentVariable.Default;
        FieldType.SelectedIndex = (int)currentVariable.Type;
    }
    public VariableEditor(string variableName, Variable.VariableType varType, string @default = "")
    {
        InitializeComponent(); Program.ApplyTheme(this);
        _defaultIsNeeded = true;
        FieldName.Text = variableName;
        FieldName.ReadOnly = true;
        FieldDefault.Text = @default;
        FieldType.DataSource = Enum.GetValues(typeof(Variable.VariableType));
        VariableType = varType;
        FieldType.SelectedItem = VariableType;
        FieldType.Enabled = false;
    }

    private void button1_Click(object sender, EventArgs e)
    {// apply
        if (Functions.Functions.ErrorCheck(FieldName.TextLength > 2, "Wrong Variable name. Minimum 3 char, maximum 32 char")) return;
        if (Functions.Functions.ErrorCheck(FieldName.TextLength < 32, "WrongVariable name. Minimum 3 char, maximum 32 char.")) return;
        Name = FieldName.Text;

        Enum.TryParse(FieldType.SelectedValue.ToString(), out Variable.VariableType type);

        if (_defaultIsNeeded)
        {
            if (Functions.Functions.ErrorCheck(FieldDefault.Text.Length > 0, "Type Variable value in default field.")) return;
        }

        if (FieldDefault.Text.Length > 0)
        {

            switch (type)
            {
                case Variable.VariableType.VTypeInt:
                {
                    if (Functions.Functions.ErrorCheck(int.TryParse(FieldDefault.Text, out int _), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypeFloat:
                {
                    if (Functions.Functions.ErrorCheck(float.TryParse(FieldDefault.Text, out float _), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypeBool:
                {
                    if (Functions.Functions.ErrorCheck(bool.TryParse(FieldDefault.Text, out bool _), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypeInstance:
                {
                    if (Functions.Functions.ErrorCheck(false, "instance is defined on runtime, not allow to default in editor!")) return;
                }
                    break;
                case Variable.VariableType.VTypeObject:
                {
                    if (Functions.Functions.ErrorCheck(GameProject.GetInstance().Instances.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypeSprite:
                {
                    if (Functions.Functions.ErrorCheck(GameProject.GetInstance().Sprites.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypeTexture:
                {
                    if (Functions.Functions.ErrorCheck(GameProject.GetInstance().Textures.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypeSound:
                {
                    if (Functions.Functions.ErrorCheck(GameProject.GetInstance().Sounds.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypeMusic:
                {
                    if (Functions.Functions.ErrorCheck(GameProject.GetInstance().Music.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypeFont:
                {
                    if (Functions.Functions.ErrorCheck(GameProject.GetInstance().Fonts.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypeScene:
                {
                    if (Functions.Functions.ErrorCheck(GameProject.GetInstance().Scenes.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                }
                    break;
                case Variable.VariableType.VTypePoint:
                {
                    string[] point = FieldDefault.Text.Split(':');
                    if (Functions.Functions.ErrorCheck(point.Length == 2, "Wrong default value. must be: x:y")) return;
                    if (Functions.Functions.ErrorCheck(float.TryParse(point[0], out float _), "Wrong default value. must be: x:y")) return;
                    if (Functions.Functions.ErrorCheck(float.TryParse(point[1], out float _), "Wrong default value. must be: x:y")) return;
                }
                    break;
                case Variable.VariableType.VTypeColor:
                {
                    if (!FieldDefault.Text.StartsWith('#'))
                    {
                        FieldDefault.Text = FieldDefault.Text.Insert(0, "#");
                    }
                    if (Functions.Functions.ErrorCheck(FieldDefault.Text.IsHex(), "Wrong default value.")) return;
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
                    if (Functions.Functions.ErrorCheck(FieldDefault.Text.Count(c => c == '"') == 2, "Wrong default value.")) return;
                }
                    break;

                case Variable.VariableType.VTypeNull:
                    break;
                case Variable.VariableType.VTypeRectangle:
                    string[] rect = FieldDefault.Text.Split(':');
                    if (Functions.Functions.ErrorCheck(rect.Length == 4, "Wrong default value. must be: x:y")) return;
                    if (Functions.Functions.ErrorCheck(float.TryParse(rect[0], out float _), "Wrong default value. must be: x:y:w:h")) return;
                    if (Functions.Functions.ErrorCheck(float.TryParse(rect[1], out float _), "Wrong default value. must be: x:y:w:h")) return;
                    if (Functions.Functions.ErrorCheck(float.TryParse(rect[2], out float _), "Wrong default value. must be: x:y:w:h")) return;
                    if (Functions.Functions.ErrorCheck(float.TryParse(rect[3], out float _), "Wrong default value. must be: x:y:w:h")) return;
                    break;
                case Variable.VariableType.VTypeEnum:
                    if (Functions.Functions.ErrorCheck(CurrentVariable != null && CurrentVariable.EnumValues.Contains(FieldDefault.Text), "Wrong default value.")) return;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


        Default = FieldDefault.Text;
        if (CurrentVariable == null)
        {
            CurrentVariable = new Variable(type, Name, Default);
        }
        else
        {
            CurrentVariable.Name = Name;
            CurrentVariable.Default = Default;
            CurrentVariable.Type = type;
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
        // check if currentVariable exists
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
        Enum.TryParse<Variable.VariableType>(FieldType.SelectedValue.ToString(), out Variable.VariableType status);
        FieldDefault.Text = status switch
        {
            Variable.VariableType.VTypeObject => PicFromList.Get(GameProject.GetInstance().Instances.Keys.ToList()),
            Variable.VariableType.VTypeSprite => PicFromList.Get(GameProject.GetInstance().Sprites.Keys.ToList()),
            Variable.VariableType.VTypeTexture => PicFromList.Get(GameProject.GetInstance().Textures.Keys.ToList()),
            Variable.VariableType.VTypeSound => PicFromList.Get(GameProject.GetInstance().Sounds.Keys.ToList()),
            Variable.VariableType.VTypeMusic => PicFromList.Get(GameProject.GetInstance().Music.Keys.ToList()),
            Variable.VariableType.VTypeFont => PicFromList.Get(GameProject.GetInstance().Fonts.Keys.ToList()),
            Variable.VariableType.VTypeScene => PicFromList.Get(GameProject.GetInstance().Scenes.Keys.ToList()),
            Variable.VariableType.VTypeEnum => PicFromList.Get(CurrentVariable?.EnumValues),
            Variable.VariableType.VTypeBool => PicFromList.Get(new List<string>(){ "True", "False" }),
            _ => "" //default
        };
    }
}