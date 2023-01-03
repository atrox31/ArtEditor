namespace ArtCore_Editor.AdvancedAssets.SceneManager.GuiEditor
{
    partial class GuiEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.GuiElementList = new System.Windows.Forms.TreeView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.GuiElementProperties = new System.Windows.Forms.DataGridView();
            this.VariableFIeld = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ValueField = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GuiElementProperties)).BeginInit();
            this.SuspendLayout();
            // 
            // GuiElementList
            // 
            this.GuiElementList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GuiElementList.HideSelection = false;
            this.GuiElementList.Location = new System.Drawing.Point(3, 19);
            this.GuiElementList.Name = "GuiElementList";
            this.GuiElementList.PathSeparator = "/";
            this.GuiElementList.Size = new System.Drawing.Size(206, 363);
            this.GuiElementList.TabIndex = 3;
            this.GuiElementList.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.GuiElementList_AfterSelect);
            this.GuiElementList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.GuiElementList_MouseClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.GuiElementList);
            this.groupBox1.Location = new System.Drawing.Point(12, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(212, 385);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Elements tree";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(559, 366);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Accept";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(478, 366);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // GuiElementProperties
            // 
            this.GuiElementProperties.AllowUserToAddRows = false;
            this.GuiElementProperties.AllowUserToDeleteRows = false;
            this.GuiElementProperties.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GuiElementProperties.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.VariableFIeld,
            this.ValueField});
            this.GuiElementProperties.Location = new System.Drawing.Point(230, 12);
            this.GuiElementProperties.MultiSelect = false;
            this.GuiElementProperties.Name = "GuiElementProperties";
            this.GuiElementProperties.RowHeadersVisible = false;
            this.GuiElementProperties.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.GuiElementProperties.RowTemplate.Height = 25;
            this.GuiElementProperties.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.GuiElementProperties.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.GuiElementProperties.ShowCellToolTips = false;
            this.GuiElementProperties.ShowEditingIcon = false;
            this.GuiElementProperties.Size = new System.Drawing.Size(407, 348);
            this.GuiElementProperties.TabIndex = 0;
            this.GuiElementProperties.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.GuiElementProperties_CellBeginEdit);
            this.GuiElementProperties.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.GuiElementProperties_CellEndEdit);
            // 
            // VariableFIeld
            // 
            this.VariableFIeld.HeaderText = "Variable";
            this.VariableFIeld.Name = "VariableFIeld";
            this.VariableFIeld.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.VariableFIeld.Width = 120;
            // 
            // ValueField
            // 
            this.ValueField.HeaderText = "Value";
            this.ValueField.Name = "ValueField";
            this.ValueField.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ValueField.Width = 300;
            // 
            // GuiEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(646, 393);
            this.Controls.Add(this.GuiElementProperties);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GuiEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "GuiEditor";
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GuiElementProperties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TreeView GuiElementList;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView GuiElementProperties;
        private System.Windows.Forms.DataGridViewTextBoxColumn VariableFIeld;
        private System.Windows.Forms.DataGridViewTextBoxColumn ValueField;
    }
}