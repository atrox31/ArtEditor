using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    public partial class CodeEditor : Form
    {
        class function
        {
            public Varible return_varible { get; set; }
            public string name { get; set; }
            public List<Varible> arguments { get; set; }
            public function(string name, List<Varible> arguments, Varible return_varible = null)
            {
                this.name = name;
                this.arguments = arguments;
                this.return_varible = return_varible;
            }
        }
        List<string> syntax_functions = new List<string>();
        List<string> syntax_varibles = new List<string>();
        List<string> syntax_operators = new List<string>();

        List<Varible> local_varibles = new List<Varible>();
        List<Varible> instance_varibles = new List<Varible>();
        List<Varible> global_varibles = new List<Varible>();

        public List<string> Code = new List<string>();
        bool changes = false;

        public CodeEditor(List<Varible> instance_varibles, string text = null)
        {
            InitializeComponent(); Program.ApplyTheme(this);
            if (text != null)
            {
                Code = text.Split('\n').ToList();
                richTextBox1.Text = text;
            }
            if (this.instance_varibles.Count > 0)
            {
                instance_varibles.CopyTo(this.instance_varibles.ToArray());
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            changes = true;
        }

        private void CodeEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (changes)
            {
                switch (MessageBox.Show("Do You want to save code?", "Exit", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        Code = richTextBox1.Text.Split('\n').ToList();
                        this.DialogResult = DialogResult.OK;
                        break;
                    case DialogResult.No:
                        this.DialogResult = DialogResult.No;
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                }
            }
        }

        private void CodeEditor_Load(object sender, EventArgs e)
        {
            /*
            string state = "";
            foreach(string line in System.IO.File.ReadAllLines("../../syntax.txt"))
            {
                if (line.StartsWith("//")) continue;
                if (line.StartsWith("["))
                {
                    state = line;
                    continue;
                }
                switch (state)
                {
                    case "[varible]":

                        break;
                    case "[instance_varibles]":

                        break;
                    case "[operators]":

                        break;
                    case "[functions]":
                        //sprite make_sprite_from_texture(texture tex, int frame_width, int frame_height)
                        
                        break;
                    case "[global_varibles]":

                        break;
                }
            }
            */
        }
    }
}
