using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ArtCore_Editor.Enums;

namespace ArtCore_Editor.Instance_Manager.code;

public partial class CodeEditor : Form
{
    public List<string> Code = new();
    private bool _changes;

    public CodeEditor(string text = null)
    {
        InitializeComponent(); Program.ApplyTheme(this);
        if (text == null) return;
        Code = text.Split('\n').ToList();
        richTextBox1.Text = text;
        _changes = false;
    }

    private void richTextBox1_TextChanged(object sender, EventArgs e)
    {
        _changes = true;
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