using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ArtCore_Editor.etc
{
    public partial class ArtCoreSettings : Form
    {
        Control AddNewField(string name, string value)
        {
            GroupBox tGroupBox1 = new GroupBox();
            TextBox tTextBox1 = new TextBox();
            tGroupBox1.SuspendLayout();

            // 
            // groupBox1
            // 
            tGroupBox1.Controls.Add(tTextBox1);
            tGroupBox1.Location = new Point(3, 3);
            tGroupBox1.Name = "groupBox_" + name;
            tGroupBox1.Size = new Size(200, 50);
            tGroupBox1.TabIndex = 0;
            tGroupBox1.TabStop = false;
            tGroupBox1.Text = name;
            // 
            // VALUE_NAME_BOX
            // 
            tTextBox1.Dock = DockStyle.Fill;
            tTextBox1.Location = new Point(3, 16);
            tTextBox1.Name = "value_" + name;
            tTextBox1.Text = value;
            tTextBox1.Size = new Size(194, 20);
            tTextBox1.TabIndex = 0;

            tGroupBox1.ResumeLayout(false);
            tGroupBox1.PerformLayout();

            flowLayoutPanel1.Controls.Add(tGroupBox1);

            return tGroupBox1;
        }
        public ArtCoreSettings()
        {
            InitializeComponent(); Program.ApplyTheme(this);
            foreach (PropertyInfo property in typeof(GameProject.ArtCorePreset).GetProperties())
            {
                string fName = property.Name;
                int value = (int)property.GetValue(GameProject.GetInstance().ArtCoreDefaultSettings, null);
                AddNewField(fName, value.ToString());

            }
            Size = new Size(Size.Width, 140 + flowLayoutPanel1.Controls.Count / 2 * 50);
            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (PropertyInfo property in typeof(GameProject.ArtCorePreset).GetProperties())
            {
                string fName = "value_" + property.Name;
                int value = Convert.ToInt32(((TextBox)flowLayoutPanel1.Controls.Find(fName, true)[0]).Text);
                property.SetValue(GameProject.GetInstance().ArtCoreDefaultSettings, value);
            }
            DialogResult = DialogResult.OK;
            Close();
        }
        
    }
}
