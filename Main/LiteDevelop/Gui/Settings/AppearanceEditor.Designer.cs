namespace LiteDevelop.Gui.Settings
{
    partial class AppearanceEditor
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.extensionsLabel = new System.Windows.Forms.Label();
            this.extensionsComboBox = new System.Windows.Forms.ComboBox();
            this.descriptionsListView = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.foreColorLabel = new System.Windows.Forms.Label();
            this.backColorLabel = new System.Windows.Forms.Label();
            this.previewLabel = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.strikeoutCheckBox = new System.Windows.Forms.CheckBox();
            this.underlineCheckBox = new System.Windows.Forms.CheckBox();
            this.italicCheckBox = new System.Windows.Forms.CheckBox();
            this.boldCheckBox = new System.Windows.Forms.CheckBox();
            this.backColorTransparentButton = new System.Windows.Forms.Button();
            this.foreColorTransparentButton = new System.Windows.Forms.Button();
            this.backColorPanel = new System.Windows.Forms.Panel();
            this.foreColorPanel = new System.Windows.Forms.Panel();
            this.useDefaultsButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // extensionsLabel
            // 
            this.extensionsLabel.AutoSize = true;
            this.extensionsLabel.Location = new System.Drawing.Point(12, 9);
            this.extensionsLabel.Name = "extensionsLabel";
            this.extensionsLabel.Size = new System.Drawing.Size(56, 13);
            this.extensionsLabel.TabIndex = 0;
            this.extensionsLabel.Text = "Extension:";
            // 
            // extensionsComboBox
            // 
            this.extensionsComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.extensionsComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.extensionsComboBox.FormattingEnabled = true;
            this.extensionsComboBox.Location = new System.Drawing.Point(15, 25);
            this.extensionsComboBox.Name = "extensionsComboBox";
            this.extensionsComboBox.Size = new System.Drawing.Size(521, 21);
            this.extensionsComboBox.TabIndex = 1;
            this.extensionsComboBox.SelectedIndexChanged += new System.EventHandler(this.extensionsComboBox_SelectedIndexChanged);
            // 
            // descriptionsListView
            // 
            this.descriptionsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.descriptionsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.descriptionsListView.FullRowSelect = true;
            this.descriptionsListView.HideSelection = false;
            this.descriptionsListView.Location = new System.Drawing.Point(0, 0);
            this.descriptionsListView.MultiSelect = false;
            this.descriptionsListView.Name = "descriptionsListView";
            this.descriptionsListView.Size = new System.Drawing.Size(256, 337);
            this.descriptionsListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.descriptionsListView.TabIndex = 2;
            this.descriptionsListView.UseCompatibleStateImageBehavior = false;
            this.descriptionsListView.View = System.Windows.Forms.View.Details;
            this.descriptionsListView.SelectedIndexChanged += new System.EventHandler(this.descriptionsListView_SelectedIndexChanged);
            this.descriptionsListView.SizeChanged += new System.EventHandler(this.descriptionsListView_SizeChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Key";
            this.columnHeader1.Width = 187;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Preview";
            this.columnHeader2.Width = 59;
            // 
            // foreColorLabel
            // 
            this.foreColorLabel.AutoSize = true;
            this.foreColorLabel.Location = new System.Drawing.Point(7, 9);
            this.foreColorLabel.Name = "foreColorLabel";
            this.foreColorLabel.Size = new System.Drawing.Size(57, 13);
            this.foreColorLabel.TabIndex = 5;
            this.foreColorLabel.Text = "Fore color:";
            // 
            // backColorLabel
            // 
            this.backColorLabel.AutoSize = true;
            this.backColorLabel.Location = new System.Drawing.Point(7, 34);
            this.backColorLabel.Name = "backColorLabel";
            this.backColorLabel.Size = new System.Drawing.Size(61, 13);
            this.backColorLabel.TabIndex = 6;
            this.backColorLabel.Text = "Back color:";
            // 
            // previewLabel
            // 
            this.previewLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.previewLabel.AutoEllipsis = true;
            this.previewLabel.BackColor = System.Drawing.SystemColors.Window;
            this.previewLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.previewLabel.Location = new System.Drawing.Point(4, 129);
            this.previewLabel.Name = "previewLabel";
            this.previewLabel.Size = new System.Drawing.Size(340, 208);
            this.previewLabel.TabIndex = 7;
            this.previewLabel.Text = "AaBbCc(123) = { \"...\" };";
            this.previewLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(15, 52);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.descriptionsListView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.strikeoutCheckBox);
            this.splitContainer1.Panel2.Controls.Add(this.underlineCheckBox);
            this.splitContainer1.Panel2.Controls.Add(this.italicCheckBox);
            this.splitContainer1.Panel2.Controls.Add(this.boldCheckBox);
            this.splitContainer1.Panel2.Controls.Add(this.backColorTransparentButton);
            this.splitContainer1.Panel2.Controls.Add(this.foreColorTransparentButton);
            this.splitContainer1.Panel2.Controls.Add(this.backColorPanel);
            this.splitContainer1.Panel2.Controls.Add(this.foreColorPanel);
            this.splitContainer1.Panel2.Controls.Add(this.previewLabel);
            this.splitContainer1.Panel2.Controls.Add(this.backColorLabel);
            this.splitContainer1.Panel2.Controls.Add(this.foreColorLabel);
            this.splitContainer1.Panel2.Enabled = false;
            this.splitContainer1.Size = new System.Drawing.Size(604, 337);
            this.splitContainer1.SplitterDistance = 256;
            this.splitContainer1.TabIndex = 8;
            // 
            // strikeoutCheckBox
            // 
            this.strikeoutCheckBox.AutoSize = true;
            this.strikeoutCheckBox.Location = new System.Drawing.Point(102, 89);
            this.strikeoutCheckBox.Name = "strikeoutCheckBox";
            this.strikeoutCheckBox.Size = new System.Drawing.Size(68, 17);
            this.strikeoutCheckBox.TabIndex = 15;
            this.strikeoutCheckBox.Text = "Strikeout";
            this.strikeoutCheckBox.UseVisualStyleBackColor = true;
            this.strikeoutCheckBox.CheckedChanged += new System.EventHandler(this.styleCheckBox_CheckedChanged);
            // 
            // underlineCheckBox
            // 
            this.underlineCheckBox.AutoSize = true;
            this.underlineCheckBox.Location = new System.Drawing.Point(102, 66);
            this.underlineCheckBox.Name = "underlineCheckBox";
            this.underlineCheckBox.Size = new System.Drawing.Size(71, 17);
            this.underlineCheckBox.TabIndex = 14;
            this.underlineCheckBox.Text = "Underline";
            this.underlineCheckBox.UseVisualStyleBackColor = true;
            this.underlineCheckBox.CheckedChanged += new System.EventHandler(this.styleCheckBox_CheckedChanged);
            // 
            // italicCheckBox
            // 
            this.italicCheckBox.AutoSize = true;
            this.italicCheckBox.Location = new System.Drawing.Point(10, 89);
            this.italicCheckBox.Name = "italicCheckBox";
            this.italicCheckBox.Size = new System.Drawing.Size(48, 17);
            this.italicCheckBox.TabIndex = 13;
            this.italicCheckBox.Text = "Italic";
            this.italicCheckBox.UseVisualStyleBackColor = true;
            this.italicCheckBox.CheckedChanged += new System.EventHandler(this.styleCheckBox_CheckedChanged);
            // 
            // boldCheckBox
            // 
            this.boldCheckBox.AutoSize = true;
            this.boldCheckBox.Location = new System.Drawing.Point(10, 66);
            this.boldCheckBox.Name = "boldCheckBox";
            this.boldCheckBox.Size = new System.Drawing.Size(47, 17);
            this.boldCheckBox.TabIndex = 12;
            this.boldCheckBox.Text = "Bold";
            this.boldCheckBox.UseVisualStyleBackColor = true;
            this.boldCheckBox.CheckedChanged += new System.EventHandler(this.styleCheckBox_CheckedChanged);
            // 
            // backColorTransparentButton
            // 
            this.backColorTransparentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.backColorTransparentButton.Location = new System.Drawing.Point(266, 29);
            this.backColorTransparentButton.Name = "backColorTransparentButton";
            this.backColorTransparentButton.Size = new System.Drawing.Size(75, 23);
            this.backColorTransparentButton.TabIndex = 11;
            this.backColorTransparentButton.Text = "Transparent";
            this.backColorTransparentButton.UseVisualStyleBackColor = true;
            this.backColorTransparentButton.Click += new System.EventHandler(this.backColorTransparentButton_Click);
            // 
            // foreColorTransparentButton
            // 
            this.foreColorTransparentButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.foreColorTransparentButton.Location = new System.Drawing.Point(266, 4);
            this.foreColorTransparentButton.Name = "foreColorTransparentButton";
            this.foreColorTransparentButton.Size = new System.Drawing.Size(75, 23);
            this.foreColorTransparentButton.TabIndex = 10;
            this.foreColorTransparentButton.Text = "Transparent";
            this.foreColorTransparentButton.UseVisualStyleBackColor = true;
            this.foreColorTransparentButton.Click += new System.EventHandler(this.foreColorTransparentButton_Click);
            // 
            // backColorPanel
            // 
            this.backColorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.backColorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.backColorPanel.Location = new System.Drawing.Point(102, 29);
            this.backColorPanel.Name = "backColorPanel";
            this.backColorPanel.Size = new System.Drawing.Size(159, 23);
            this.backColorPanel.TabIndex = 9;
            this.backColorPanel.Click += new System.EventHandler(this.colorPanel_Click);
            // 
            // foreColorPanel
            // 
            this.foreColorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.foreColorPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.foreColorPanel.Location = new System.Drawing.Point(102, 4);
            this.foreColorPanel.Name = "foreColorPanel";
            this.foreColorPanel.Size = new System.Drawing.Size(159, 23);
            this.foreColorPanel.TabIndex = 8;
            this.foreColorPanel.Click += new System.EventHandler(this.colorPanel_Click);
            // 
            // useDefaultsButton
            // 
            this.useDefaultsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.useDefaultsButton.Location = new System.Drawing.Point(541, 23);
            this.useDefaultsButton.Name = "useDefaultsButton";
            this.useDefaultsButton.Size = new System.Drawing.Size(75, 23);
            this.useDefaultsButton.TabIndex = 16;
            this.useDefaultsButton.Text = "Use defaults";
            this.useDefaultsButton.UseVisualStyleBackColor = true;
            this.useDefaultsButton.Click += new System.EventHandler(this.useDefaultsButton_Click);
            // 
            // AppearanceEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.useDefaultsButton);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.extensionsComboBox);
            this.Controls.Add(this.extensionsLabel);
            this.Name = "AppearanceEditor";
            this.Size = new System.Drawing.Size(631, 407);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label extensionsLabel;
        private System.Windows.Forms.ComboBox extensionsComboBox;
        private System.Windows.Forms.ListView descriptionsListView;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Label foreColorLabel;
        private System.Windows.Forms.Label backColorLabel;
        private System.Windows.Forms.Label previewLabel;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel backColorPanel;
        private System.Windows.Forms.Panel foreColorPanel;
        private System.Windows.Forms.Button backColorTransparentButton;
        private System.Windows.Forms.Button foreColorTransparentButton;
        private System.Windows.Forms.CheckBox underlineCheckBox;
        private System.Windows.Forms.CheckBox italicCheckBox;
        private System.Windows.Forms.CheckBox boldCheckBox;
        private System.Windows.Forms.CheckBox strikeoutCheckBox;
        private System.Windows.Forms.Button useDefaultsButton;
    }
}
