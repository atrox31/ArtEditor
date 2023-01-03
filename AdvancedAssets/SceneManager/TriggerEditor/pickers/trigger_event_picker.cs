using System;
using System.Windows.Forms;
using ArtCore_Editor.Enums;

namespace ArtCore_Editor.AdvancedAssets.SceneManager.TriggerEditor.pickers;

public partial class TriggerEventPicker : Form
{
    public TriggerEventPicker()
    {
        InitializeComponent(); Program.ApplyTheme(this);
    }

    private Event.TriggerType _type;

    public Event.TriggerType Get()
    {
        return _type;
    }

    private void object_event_picker_Load(object sender, EventArgs e)
    {
        listBox1.DataSource = Enum.GetValues(typeof(Event.TriggerType));
    }

    private void button2_Click(object sender, EventArgs e)
    {
        bool tryParse = Enum.TryParse(listBox1.SelectedItem.ToString(), out _type);
        DialogResult = tryParse ? DialogResult.OK : DialogResult.Cancel;
        Close();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}