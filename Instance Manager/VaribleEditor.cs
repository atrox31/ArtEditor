using System;
using System.Linq;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    public partial class VaribleEditor : Form
    {
        public static Varible GetValue()
        {
            VaribleEditor v = new VaribleEditor();
            if (v.ShowDialog() == DialogResult.OK)
            {
                return new Varible(v._Type, v._Name, v._Default);
            }
            return null;
        }

        public string _Name;
        public Varible.type _Type;
        public string _Default;
        bool DefaultIsNeeded = false;

        public Varible _var;
        public VaribleEditor(Varible var = null)
        {
            InitializeComponent(); Program.ApplyTheme(this);
            FieldType.DataSource = Enum.GetValues(typeof(Varible.type));
            this._var = var;
            if (var != null)
            {
                FieldName.Text = var.Name;
                FieldDefault.Text = var.Default;
                FieldType.SelectedIndex = (int)var.Type;
            }
        }
        public VaribleEditor(bool GetValueForCodeEditor, string VarType, string Value = "")
        {
            InitializeComponent(); Program.ApplyTheme(this);
            DefaultIsNeeded = true;
            FieldName.Text = "Value";
            FieldName.ReadOnly = true;
            FieldDefault.Text = Value;
            FieldType.DataSource = Enum.GetValues(typeof(Varible.type));
            FieldType.SelectedItem = Enum.Parse(typeof(Varible.type), VarType.ToUpper());
            FieldType.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {// apply
            if (Functions.ErrorCheck(FieldName.TextLength > 3, "Wrong Varible name. Too short.")) return;
            if (Functions.ErrorCheck(FieldName.TextLength < 24, "Wrong Varible name. Too long.")) return;
            _Name = FieldName.Text;

            Varible.type status;
            Enum.TryParse<Varible.type>(FieldType.SelectedValue.ToString(), out status);
            _Type = status;

            if (DefaultIsNeeded)
            {
                if (Functions.ErrorCheck(FieldDefault.Text.Length > 0, "Type varible value in default field.")) return;
            }

            if (FieldDefault.Text.Length > 0)
            {

                switch (status)
                {
                    case Varible.type.INT:
                        {
                            if (Functions.ErrorCheck(int.TryParse(FieldDefault.Text, out int _), "Wrong default value.")) return;
                        }
                        break;
                    case Varible.type.FLOAT:
                        {
                            if (Functions.ErrorCheck(float.TryParse(FieldDefault.Text, out float _), "Wrong default value.")) return;
                        }
                        break;
                    case Varible.type.BOOL:
                        {
                            if (Functions.ErrorCheck(bool.TryParse(FieldDefault.Text, out bool _), "Wrong default value.")) return;
                        }
                        break;
                    case Varible.type.INSTANCE:
                        {
                            if (Functions.ErrorCheck(false, "instance is defined on runtime, not allow to default in editor!")) return;
                        }
                        break;
                    case Varible.type.OBJECT:
                        {
                            if (Functions.ErrorCheck(GameProject.GetInstance().Instances.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                        }
                        break;
                    case Varible.type.SPRITE:
                        {
                            if (Functions.ErrorCheck(GameProject.GetInstance().Sprites.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                        }
                        break;
                    case Varible.type.TEXTURE:
                        {
                            if (Functions.ErrorCheck(GameProject.GetInstance().Textures.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                        }
                        break;
                    case Varible.type.SOUND:
                        {
                            if (Functions.ErrorCheck(GameProject.GetInstance().Sounds.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                        }
                        break;
                    case Varible.type.MUSIC:
                        {
                            if (Functions.ErrorCheck(GameProject.GetInstance().Music.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                        }
                        break;
                    case Varible.type.FONT:
                        {
                            if (Functions.ErrorCheck(GameProject.GetInstance().Fonts.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                        }
                        break;
                    case Varible.type.SCENE:
                        {
                            if (Functions.ErrorCheck(GameProject.GetInstance().Scenes.Keys.Contains(FieldDefault.Text), "Wrong default value.")) return;
                        }
                        break;
                    case Varible.type.POINT:
                        {
                            var point = FieldDefault.Text.Split(':');
                            if (Functions.ErrorCheck(point.Length == 2, "Wrong default value. must be: x:y")) return;
                            if (Functions.ErrorCheck(float.TryParse(point[0], out float _), "Wrong default value. must be: x:y")) return;
                            if (Functions.ErrorCheck(float.TryParse(point[1], out float _), "Wrong default value. must be: x:y")) return;
                        }
                        break;
                    case Varible.type.COLOR:
                        {
                            if (!FieldDefault.Text.StartsWith('#'))
                            {
                                FieldDefault.Text = FieldDefault.Text.Insert(0, "#");
                            }
                            if (Functions.ErrorCheck(Functions.IsHex(FieldDefault.Text), "Wrong default value.")) return;
                        }
                        break;
                    case Varible.type.STRING:
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

                }
            }


            _Default = FieldDefault.Text;
            if (_var == null)
            {
                _var = new Varible(_Type, _Name, _Default);
            }
            else
            {
                _var.Name = _Name;
                _var.Default = _Default;
                _var.Type = _Type;
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
            Varible.type status;
            Enum.TryParse<Varible.type>(FieldType.SelectedValue.ToString(), out status);
            switch (status)
            {
                case Varible.type.INSTANCE:
                case Varible.type.SPRITE:
                case Varible.type.TEXTURE:
                case Varible.type.SOUND:
                case Varible.type.MUSIC:
                case Varible.type.FONT:
                case Varible.type.SCENE:
                    button3.Enabled = true;
                    return;

            }
            button3.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Varible.type status;
            Enum.TryParse<Varible.type>(FieldType.SelectedValue.ToString(), out status);
            switch (status)
            {
                case Varible.type.INSTANCE:
                    {
                        FieldDefault.Text = PicFromList.Get(GameProject.GetInstance().Instances.Keys.ToList());
                    }
                    break;
                case Varible.type.SPRITE:
                    {
                        FieldDefault.Text = PicFromList.Get(GameProject.GetInstance().Sprites.Keys.ToList());
                    }
                    break;
                case Varible.type.TEXTURE:
                    {
                        FieldDefault.Text = PicFromList.Get(GameProject.GetInstance().Textures.Keys.ToList());
                    }
                    break;
                case Varible.type.SOUND:
                    {
                        FieldDefault.Text = PicFromList.Get(GameProject.GetInstance().Sounds.Keys.ToList());
                    }
                    break;
                case Varible.type.MUSIC:
                    {
                        FieldDefault.Text = PicFromList.Get(GameProject.GetInstance().Music.Keys.ToList());
                    }
                    break;
                case Varible.type.FONT:
                    {
                        FieldDefault.Text = PicFromList.Get(GameProject.GetInstance().Fonts.Keys.ToList());
                    }
                    break;

            }
        }
    }
}
