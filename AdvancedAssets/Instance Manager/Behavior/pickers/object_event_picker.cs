using System;
using System.Windows.Forms;
using ArtCore_Editor.Enums;

namespace ArtCore_Editor.AdvancedAssets.Instance_Manager.Behavior.pickers;

public partial class ObjectEventPicker : Form
{
    public ObjectEventPicker()
    {
        InitializeComponent(); Program.ApplyTheme(this);
    }
    public Event.EventType Type;

    private void object_event_picker_Load(object sender, EventArgs e)
    {
        listBox1.DataSource = Enum.GetValues(typeof(Event.EventType));
    }

    private void button2_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
        Enum.TryParse(listBox1.SelectedItem.ToString(), out Type);
        Close();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}