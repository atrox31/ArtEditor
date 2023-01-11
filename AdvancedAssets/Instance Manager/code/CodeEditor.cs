using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ArtCore_Editor.AdvancedAssets.Instance_Manager.Behavior;
using ArtCore_Editor.Enums;
using ArtCore_Editor.Functions;

namespace ArtCore_Editor.AdvancedAssets.Instance_Manager.code;

public partial class CodeEditor : Form
{
    [DllImport("user32", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);


    public List<string> Code = new();
    private bool _changes;
    // static ArtCode.lib content
    private static List<Function> _functionsList = null;
    private readonly List<Variable> _variablesList = null;

    // color pallet
    private static class ColorPallet
    {
        public static readonly Color Error    = Color.OrangeRed;
        public static readonly Color Comment  = Color.GreenYellow;
        public static readonly Color Function = Color.Yellow;
        public static readonly Color Variable = Color.DodgerBlue;
        public static readonly Color Numeric  = Color.DeepSkyBlue;
        public static readonly Color Token    = Color.PapayaWhip;
        public static readonly Color Neutral  = Color.GhostWhite;
        public static readonly Color Text     = Color.Orange;
        public static readonly Color Boolean  = Color.CornflowerBlue;
    }
    public CodeEditor(string text = null, List<Variable> variables = null, int selectedLine = -1)
    {
        InitializeComponent(); Program.ApplyTheme(this);
        if (text == null) return;
        Code = text.Split('\n').ToList();
        richTextBox1.Text = text;
        _changes = false;

        _variablesList = variables;

        if (_functionsList == null)
        {
            _functionsList = new List<Function>();
            foreach (string line in System.IO.File.ReadAllLines(Program.ProgramDirectory + "\\" + "Core\\AScript.lib"))
            {
                if (line.Length == 0) continue;
                if (line.StartsWith("//")) continue;
                if (string.Concat(line.Where(c => !char.IsWhiteSpace(c))).Length == 0) continue;
                _functionsList.Add(new Function(line));
            }
        }

        ColorTheCode();
        if (selectedLine > -1)
        {
            int start = richTextBox1.GetFirstCharIndexFromLine(selectedLine);
            int length = richTextBox1.Lines[selectedLine].Length;
            richTextBox1.Select(start, length);
        }
    }

    private void richTextBox1_TextChanged(object sender, EventArgs e)
    {
        ColorTheCode();
    }

    private void ColorTheCode()
    {
        const int WM_SETREDRAW = 0x000B;
        const int WM_USER = 0x400;
        const int EM_GETEVENTMASK = (WM_USER + 59);
        const int EM_SETEVENTMASK = (WM_USER + 69);
        IntPtr eventMask = IntPtr.Zero;
        try
        {
            // Stop redrawing:
            SendMessage(richTextBox1.Handle, WM_SETREDRAW, 0, IntPtr.Zero);
            // Stop sending of events:
            eventMask = SendMessage(richTextBox1.Handle, EM_GETEVENTMASK, 0, IntPtr.Zero);

            // change colors and stuff in the RichTextBox
            _changes = true;
            string[] tokens =
            {
                "if", "end", "other", "else",
                "+=", "-=", "*=", "/=", "!=", 
                "<>", "==", "<=", ">=", ":="
            };

            int oldSelectionStart = richTextBox1.SelectionStart;
            int oldSelectionLength = richTextBox1.SelectionLength;

            // restart parameters
            int pos = 0;
            int len = 0;
            richTextBox1.SelectionStart = 0;
            richTextBox1.SelectionLength = richTextBox1.Text.Length;
            richTextBox1.SelectionColor = ColorPallet.Neutral;
            string text = "";

            while (pos < richTextBox1.Text.Length - 1)
            {
                text += richTextBox1.Text[pos];
                pos++;
                len++;

                if (text.Last() == ' ' || text.Last() == ')' 
                ||  text.Last() == '(' || text.Last() == '\n' 
                || text.Last() == '\"'|| text.Last() == ',')
                {
                    bool found = false;
                    // text
                    if (!found && (text.Length > 0 && text.Last() == '\"'))
                    {
                        found = true;
                        int altPos = text.IndexOf('\"', StringComparison.Ordinal);
                        if (pos >= richTextBox1.Text.Length - 1) break;
                        // seek to end of line
                        while (pos < richTextBox1.Text.Length)
                        {
                            text += richTextBox1.Text[pos];
                            pos++;
                            len++;
                            if (text.Last() == '\"')
                            {
                                richTextBox1.SelectionStart = pos + altPos - len;
                                richTextBox1.SelectionLength = len - altPos;
                                richTextBox1.SelectionColor = ColorPallet.Text;
                                break;
                            }
                        }
                    }

                    // comment
                    if (!found && (text.Contains("//")))
                    {
                        found = true;
                        int altPos = text.IndexOf("//", StringComparison.Ordinal);
                        // seek to end of line
                        while(text.IndexOf('\n', StringComparison.Ordinal) < 0)
                        {
                            if (pos >= richTextBox1.Text.Length - 1) break;
                            text += richTextBox1.Text[pos];
                            pos++;
                            len++;
                        }
                        richTextBox1.SelectionStart = pos + altPos - len;
                        richTextBox1.SelectionLength = len - altPos;
                        richTextBox1.SelectionColor = ColorPallet.Comment;
                    }

                    // last char is special, delete it
                    text = text.Remove(text.Length - 1);
                    // token
                    if (!found && (tokens.Contains(text.RemoveWhitespace())))
                    {
                        found = true;
                        richTextBox1.SelectionLength = len;
                        richTextBox1.SelectionColor = ColorPallet.Token;
                    }

                    if (!found && (text.ToLower() == "true" || text.ToLower() == "false"))
                    {
                        found = true;
                        richTextBox1.SelectionLength = len - 1;
                        richTextBox1.SelectionColor = ColorPallet.Boolean;
                    }

                    // function
                    if (!found && (_functionsList.Any(f => f.Name == text)))
                    {
                        found = true;
                        richTextBox1.SelectionLength = len -1;
                        richTextBox1.SelectionColor = ColorPallet.Function;

                    }

                    if (!found && (_variablesList != null && _variablesList.Count > 0))
                    {
                        if (_variablesList.Any(v => v.Name == text))
                        {
                            found = true;
                            richTextBox1.SelectionLength = len - 1;
                            richTextBox1.SelectionColor = ColorPallet.Variable;
                        }
                    }

                    if (!found && (text.Length > 0 && decimal.TryParse(text, out _)))
                    {
                        found = true;
                        richTextBox1.SelectionLength = len - 1;
                        richTextBox1.SelectionColor = ColorPallet.Numeric;
                    }

                    if (text.Length > 0 && !found)
                    {
                        richTextBox1.SelectionLength = len - 1;
                        richTextBox1.SelectionColor = ColorPallet.Error;
                    }

                    // restart parameters

                    len = 0;
                    richTextBox1.SelectionStart = pos;
                    text = "";

                }
            }

            richTextBox1.SelectionStart = oldSelectionStart;
            richTextBox1.SelectionLength = oldSelectionLength;
        }
        finally
        {
            // turn on events
            SendMessage(richTextBox1.Handle, EM_SETEVENTMASK, 0, eventMask);
            // turn on redrawing
            SendMessage(richTextBox1.Handle, WM_SETREDRAW, 1, IntPtr.Zero);
            richTextBox1.Refresh();
        }
    }

    private void CodeEditor_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (!_changes) return;
        switch (MessageBox.Show("Do You want to save code?", "Exit", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
        {
            case DialogResult.Yes:
                Code = richTextBox1.Text.Split('\n').ToList();
                DialogResult = DialogResult.OK;
                break;
            case DialogResult.No:
                DialogResult = DialogResult.No;
                break;
            case DialogResult.Cancel:
                e.Cancel = true;
                return;
        }
    }
    
}