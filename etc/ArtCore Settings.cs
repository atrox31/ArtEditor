using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ArtCore_Editor
{
    public partial class ArtCore_Settings : Form
    {
        Control AddNewField(string name, string value)
        {
            GroupBox t_groupBox1 = new GroupBox();
            TextBox t_textBox1 = new TextBox();
            t_groupBox1.SuspendLayout();

            // 
            // groupBox1
            // 
            t_groupBox1.Controls.Add(t_textBox1);
            t_groupBox1.Location = new System.Drawing.Point(3, 3);
            t_groupBox1.Name = "groupBox_" + name;
            t_groupBox1.Size = new System.Drawing.Size(200, 50);
            t_groupBox1.TabIndex = 0;
            t_groupBox1.TabStop = false;
            t_groupBox1.Text = name;
            // 
            // VALUE_NAME_BOX
            // 
            t_textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            t_textBox1.Location = new System.Drawing.Point(3, 16);
            t_textBox1.Name = "value_" + name;
            t_textBox1.Text = value;
            t_textBox1.Size = new System.Drawing.Size(194, 20);
            t_textBox1.TabIndex = 0;

            t_groupBox1.ResumeLayout(false);
            t_groupBox1.PerformLayout();

            flowLayoutPanel1.Controls.Add(t_groupBox1);

            return t_groupBox1;
        }
        public ArtCore_Settings()
        {
            InitializeComponent();Program.ApplyTheme(this);
            foreach (PropertyInfo property in typeof(GameProject.ArtCorePreset).GetProperties())
            {
                var f_name = property.Name;
                int value = (int)property.GetValue(GameProject.GetInstance().ArtCoreDefaultSettings, null);
                AddNewField(f_name, value.ToString());

            }
            this.Size = new Size(Size.Width, 140 + flowLayoutPanel1.Controls.Count / 2 * 50);
            this.DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (PropertyInfo property in typeof(GameProject.ArtCorePreset).GetProperties())
            {
                var f_name = "value_" + property.Name;
                int value = Convert.ToInt32(((TextBox)flowLayoutPanel1.Controls.Find(f_name, true)[0]).Text);
                property.SetValue(GameProject.GetInstance().ArtCoreDefaultSettings, value);
            }
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void ArtCore_Settings_Load(object sender, EventArgs e)
        {

        }
    }
}
