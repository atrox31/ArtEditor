using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Collections.Specialized;
using ArtCore_Editor.Main;
using System.Collections;
using System.Windows.Input;

namespace ArtCore_Editor.etc
{
    public partial class ArtCoreSettings : Form
    {
        private bool _saved = true;
        public readonly OrderedDictionary UserProperties = new OrderedDictionary();

        /// <summary>
        /// Open editor of core settings, fill with default values
        /// if user override something its updated
        /// Show dialog return Ok or Cancel
        /// OK - save changes
        /// Cancel - not save changes
        /// </summary>
        /// <param name="userProperties"></param>
        public ArtCoreSettings(OrderedDictionary userProperties)
        {
            InitializeComponent(); Program.ApplyTheme(this);

            if (userProperties == null) return;
            // copy content from user properties to not change if user don`t want to
            // why?
            // in onLoad check if default settings file exists and close
            // if not. I cannot do this in constructor. For stay with global look
            // type obj = new type(arg);
            // if(obj.showDialog() == DialogResult.something)
            //
            // first copy user settings, and on load populate grid
            // on exit get from grid to UserProperties if user want to save
            // after that UserProperties is copied to original dictionary
            // in c++ i can do this better but here i don`t
            foreach (DictionaryEntry keyValuePair in userProperties)
            {
                UserProperties.Add(keyValuePair.Key, keyValuePair.Value);
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _saved = true;
            Close();
        }

        private void ArtCoreSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_saved)
            {
                switch (MessageBox.Show("Do You want to save changes?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        _saved = true;
                        break;
                    case DialogResult.No:
                        _saved = false;
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        return;
                        break;
                }
            }
            DialogResult = _saved ? DialogResult.OK : DialogResult.Cancel;
            // if user not want to save data return here
            if (!_saved) return;
            // no data in grid, error in open or user delete everything
            if (dataGridView1.Rows.Count == 1) return;
            // populate UserProperties with changes from data grid
            UserProperties.Clear();
            foreach (DataGridViewRow dataGridViewRow in dataGridView1.Rows)
            {
                // get key and value from cell
                string key = (string)dataGridViewRow.Cells[0].Value;
                string value = (string)dataGridViewRow.Cells[1].Value;

                // situations
                // test IsNullOrEmpty                                                       key         value              sum
                // 1. key and value is empty <- this is new line row, skip                  true  (1) + true  (1) = (2) -> continue
                // 2. key is empty but value not <- this is problem, can not be             true  (1) + false (0) = (1) -> error, break
                // 3. key is not empty but value is empty <- user not type in value field   false (0) + true  (1) = (1) -> error, break
                // 4. key and value have something <- good                                  false (0) + false (0) = (0) -> add value

                int keyBool = string.IsNullOrEmpty(key) ? 1 : 0;
                int valueBool = string.IsNullOrEmpty(value) ? 1 : 0;
                
                switch (keyBool + valueBool)
                {
                    case 2:
                        continue;
                    case 1:
                        MessageBox.Show("Error, can not parse settings data!", "Error", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        e.Cancel = true;
                        _saved = false;
                        return;
                    case 0:
                        // update it
                        UserProperties.Add(key!, value);
                        break;
                }
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            _saved = false;
        }

        private void ArtCoreSettings_Load(object sender, EventArgs e)
        {

            string iniFile = Program.ProgramDirectory + "\\" + "Core" + "\\" + "DefaultSettings" + "\\" +
                              "default_settings.ini";

            if (Functions.Functions.ErrorCheck(
                    File.Exists(iniFile),
                    "Default settings file not found!"
                )) { Close(); return; }

            foreach (string line in File.ReadAllLines(iniFile))
            {
                // skip empty lines or lines with comment
                if (line.Length == 0 || line.StartsWith("//")) continue;
                // get setting = value pair
                string[] setting = line.Split('=');
                // check if its real pair
                if (setting.Length != 2) continue;
                // add new only if not exists - this is default value
                if (!UserProperties.Contains(setting[0]))
                {
                    UserProperties.Add(setting[0], setting[1]);
                }
            }

            // populate grid
            foreach (DictionaryEntry userProperty in UserProperties)
            {
                dataGridView1.Rows.Add(userProperty.Key, userProperty.Value);
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // check if this in new row
            if (((string)dataGridView1.Rows[e.RowIndex].Cells[0].Value)?.Length > 0)
            { return; } // normal not null field, can edit

            // get name of field
            string fieldName = Pick_forms.GetString.Get("Setting field name");

            if (Functions.Functions.ErrorCheck(!string.IsNullOrEmpty(fieldName), "Field empty!"))
            { e.Cancel = true; return; }

            foreach (DataGridViewRow dataGridViewRow in dataGridView1.Rows)
            {
                // check if field have something
                string key = (string)dataGridViewRow.Cells[0].Value;
                if (key == null) continue;

                // no duplicates are allow
                if (Functions.Functions.ErrorCheck(key != fieldName, "Target field exists!"))
                { e.Cancel = true; return; }
            }

            // get value
            string value = Pick_forms.GetString.Get("Value");

            // check value
            if (Functions.Functions.ErrorCheck(!string.IsNullOrEmpty(value), "Type value!"))
            { e.Cancel = true; return; }

            // all ok, add
            dataGridView1.Rows[e.RowIndex].Cells[0].Value = fieldName;
            dataGridView1.Rows[e.RowIndex].Cells[1].Value = value;
            e.Cancel = true;
        }
    }
}
