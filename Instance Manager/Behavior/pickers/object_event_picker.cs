﻿using System;
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
    public partial class object_event_picker : Form
{
    public object_event_picker()
    {
        InitializeComponent();
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
            DialogResult=DialogResult.Cancel;
            Close();
        }
    }
}
