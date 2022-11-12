using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    public partial class ValuePicker : Form
{
        static Dictionary<string, List<string>> Functions = new Dictionary<string, List<string>>();
        List<string> usedFunctions = new List<string>();
        List<string> returnedValues = new List<string>();
        string c_Content = "";

        string _value;
        public string Return;
    public ValuePicker(string value)
    {
            Return = "";
            this._value = value;
        InitializeComponent();
            Functions.Add("math",new List<string>()
            {
                "math_min_i;Minimum;Minum value from |value| and |value|.;int",
                "math_max_i;Maximum;Maximum value from |value| and |value|.;int",
                "math_random_i;Random;Random value from |value| to |value|.;int",
                "math_min_f;Minimum;Minum value from |value| and |value|.;float",
                "math_max_f;Maximum;Maximum value from |value| and |value|.;float",
                "math_random_f;Random;Random value from |value| to |value|.;float",

            } );
            Functions.Add("point",new List<string>()
            {
                "point_create_i;Create point;Create int point with value \nx = |value| \ny = |value|.;int",
                "point_create_f;Create point;Create float point with value \nx = |value| \ny = |value|.;float",
                "point_variable;Use variable;Use instance variable |variable|.;point",
            } );
    }

    private void ValuePicker_Load(object sender, EventArgs e)
    {
            foreach (string key in Functions.Keys)
            {
                foreach(string value in Functions[key])
                {
                    if (value.Split(';')[3].Contains(_value.Split(':')[0]))
                    {
                        comboBox1.Items.Add(value.Split(';')[1]);
                        usedFunctions.Add(value);
                    }
                }
            }
            
    }
        public void ContentRefresh()
        {
            string[] tokens = c_Content.Split('|');
            int current_pos = 0;
            int current_link = 0;
            Content.Text = "";
            Content.Links.Clear();

            bool is_picker = false;
            int token_num = 0;

            foreach (string token in tokens)
            {
                if (is_picker) // picker
                {
                    is_picker = !is_picker;

                    string text_to_add = returnedValues[token_num++];


                    Content.Text += text_to_add;
                    Content.Links.Add(current_pos, text_to_add.Length);
                    Content.Links[current_link].Description = token+":"+tokens[3];
                    current_link++;

                    current_pos += text_to_add.Length;
                }
                else
                {   // normal text
                    Content.Text += token;
                    current_pos += token.Length;
                }
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] temp = usedFunctions[comboBox1.SelectedIndex].Split(';');
            c_Content = temp[2];
            bool is_token = false;
            returnedValues.Clear();
            foreach (string _temp in temp[2].Split('|'))
            {
                if (is_token)
                {
                    is_token = !is_token;
                    returnedValues.Add(_temp);
                }
            }

            ContentRefresh();
        }

        private void Content_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string value_pick = e.Link.Description.Split(':')[0];
            string value_type = e.Link.Description.Split(':')[1];

            if(value_pick == "value")
            {
                ValuePicker picker = new ValuePicker(e.Link.Description);
                if(picker.ShowDialog() == DialogResult.OK)
                {
                    
                }
            }
            if(value_pick == "variable")
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked) // value
            {
                Return = textBox1.Text;
            }
            else if (radioButton1.Checked) // maker
            {
                string[] tokens = c_Content.Split('|');
                Return = tokens[0] + "(" + String.Join(",", returnedValues.ToArray()) + ")";
            }
            else
            {
                return;
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
