using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArtCore_Editor.Enums;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace ArtCore_Editor
{
    public partial class GuiEditor : Form
    {
        public GuiEditor()
        {
            InitializeComponent(); Program.ApplyTheme(this);
        }



        class GuiElement
        {
            private static List<Variable> StandardValues = new List<Variable>()
            {

                    new Variable(Variable.VariableType.VTypeString, "Tag", "undefined"),
                    new Variable(Variable.VariableType.VTypeInt, "Position_x", "undefined"),
                    new Variable(Variable.VariableType.VTypeInt, "Position_y", "undefined"),
                    new Variable(Variable.VariableType.VTypeInt, "Width", "undefined"),
                    new Variable(Variable.VariableType.VTypeInt, "Height", "undefined"),
                    new Variable(Variable.VariableType.VTypeBool, "Enabled", "undefined"),
                    new Variable(Variable.VariableType.VTypeBool, "Visible", "undefined"),
                    new Variable(Variable.VariableType.VTypeString, "Style", "ALIGN_LEFT"),
                    //"string Tag='undefined'", "float Position_y=0", "float Position_x=0", "float Width=128", "float Height=128", "bool Enabled=true",
                    //"bool Visible=true", "enum[ALIGN_LEFT,ALIGN_RIGHT,ALIGN_CENTER,FILL_CENTER,RELATIVE_PARENT,ABSOLUTE] Style=ALIGN_LEFT"
                
            };

            private List<string> Fields = new List<string>();

            public void SetValue(string field, string value)
            {
                for(var i=0; i<Fields.Count; i++){
                    // get field name from variable list type
                    string fieldName = Fields[i].Split('=').First().Split(' ').Last();
                    if (fieldName == field)
                    {
                        string returnString = Fields[i].Split('=').First();
                        Fields[i] = returnString + '=' + value;
                        return;
                    }
                }
            }

            public string GetValue(string field)
            {
                return "";
            }
           
            public GuiElement( List<string> CustomComponents)
            {
                

            }

        };
        Dictionary<string, GuiElement> ElementTemplates = new Dictionary<string, GuiElement>();

        private void GuiEditor_Load(object sender, EventArgs e)
        {
            // const values from ArtCore
            ElementTemplates.Add(
                "Button",
                new GuiElement(
                    new List<string>()
                    {
                        "string Text=''"
                    }
                )
            );

            ElementTemplates.Add(
                "Grid",
                new GuiElement(
                    new List<string>()
                    {
                        "int Columns=3", "int Rows=3", "int ElementWidth=64", "int ElementHeight=64",
                        "int ElementSpacing=8",
                    }
                )
            );

            ElementTemplates.Add(
                "Image",
                new GuiElement(
                    new List<string>()
                    {
                        "sprite image=null"
                    }
                )
            );

            ElementTemplates.Add(
                "Label",
                new GuiElement(
                    new List<string>()
                    {
                        "string Text=''"
                    }
                )
            );

            ElementTemplates.Add(
                "Panel",
                new GuiElement(
                    new List<string>()
                    {
                        // null
                    }
                )
            );

            ElementTemplates.Add("ProgressBar",
                new GuiElement(

                    new List<string>()
                    {
                        "int Minimum=0", "int Maximum=100", "Enum[STEP,SOLID,STEP_BLEND,SOLID_BLEND] DrawingStyle=STEP",
                        "int height=32"
                    }
                )
            );
            /* TODO: implement later
            ElementTemplates.Add(
                    "TabPanel",
                new GuiElement(
                    new List<string>()
                    {
                        
                    }
                )
            );
            */
            foreach (var element in ElementTemplates)
            {
                list_items.Items.Add(element.Key);
            }

        }
    }
}
