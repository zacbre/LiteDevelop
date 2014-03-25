namespace LiteDevelop.Essentials.CodeEditor.Gui
{
    partial class AutoCompleteSettingsControl
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.showSuggestionsListWhenLabel = new System.Windows.Forms.Label();
            this.showSuggestionsListComboBox = new System.Windows.Forms.ComboBox();
            this.commitSelectedItemWhenLabel = new System.Windows.Forms.Label();
            this.completeOnSpaceBarCheckBox = new System.Windows.Forms.CheckBox();
            this.autoCompleteCharsTextBox = new System.Windows.Forms.TextBox();
            this.autoAddParanthesesCheckBox = new System.Windows.Forms.CheckBox();
            this.autoCompleteCodeBlocksCheckBox = new System.Windows.Forms.CheckBox();
            this.autoListMembersCheckBox = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.showSuggestionsListWhenLabel);
            this.panel1.Controls.Add(this.showSuggestionsListComboBox);
            this.panel1.Controls.Add(this.commitSelectedItemWhenLabel);
            this.panel1.Controls.Add(this.completeOnSpaceBarCheckBox);
            this.panel1.Controls.Add(this.autoCompleteCharsTextBox);
            this.panel1.Location = new System.Drawing.Point(27, 35);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(346, 102);
            this.panel1.TabIndex = 13;
            // 
            // showSuggestionsListWhenLabel
            // 
            this.showSuggestionsListWhenLabel.AutoSize = true;
            this.showSuggestionsListWhenLabel.Location = new System.Drawing.Point(3, 6);
            this.showSuggestionsListWhenLabel.Name = "showSuggestionsListWhenLabel";
            this.showSuggestionsListWhenLabel.Size = new System.Drawing.Size(140, 13);
            this.showSuggestionsListWhenLabel.TabIndex = 11;
            this.showSuggestionsListWhenLabel.Text = "Show suggestions list when:";
            // 
            // showSuggestionsListComboBox
            // 
            this.showSuggestionsListComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.showSuggestionsListComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.showSuggestionsListComboBox.FormattingEnabled = true;
            this.showSuggestionsListComboBox.Items.AddRange(new object[] {
            "typing a character.",
            "pressing Ctrl+Space only."});
            this.showSuggestionsListComboBox.Location = new System.Drawing.Point(149, 2);
            this.showSuggestionsListComboBox.Name = "showSuggestionsListComboBox";
            this.showSuggestionsListComboBox.Size = new System.Drawing.Size(194, 21);
            this.showSuggestionsListComboBox.TabIndex = 12;
            // 
            // commitSelectedItemWhenLabel
            // 
            this.commitSelectedItemWhenLabel.AutoSize = true;
            this.commitSelectedItemWhenLabel.Location = new System.Drawing.Point(3, 39);
            this.commitSelectedItemWhenLabel.Name = "commitSelectedItemWhenLabel";
            this.commitSelectedItemWhenLabel.Size = new System.Drawing.Size(304, 13);
            this.commitSelectedItemWhenLabel.TabIndex = 5;
            this.commitSelectedItemWhenLabel.Text = "Commit selected item when pressing one of the following chars:";
            // 
            // completeOnSpaceBarCheckBox
            // 
            this.completeOnSpaceBarCheckBox.AutoSize = true;
            this.completeOnSpaceBarCheckBox.Location = new System.Drawing.Point(6, 80);
            this.completeOnSpaceBarCheckBox.Name = "completeOnSpaceBarCheckBox";
            this.completeOnSpaceBarCheckBox.Size = new System.Drawing.Size(264, 17);
            this.completeOnSpaceBarCheckBox.TabIndex = 8;
            this.completeOnSpaceBarCheckBox.Text = "Commit selected item when pressing the space bar";
            this.completeOnSpaceBarCheckBox.UseVisualStyleBackColor = true;
            // 
            // autoCompleteCharsTextBox
            // 
            this.autoCompleteCharsTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.autoCompleteCharsTextBox.Location = new System.Drawing.Point(6, 55);
            this.autoCompleteCharsTextBox.Name = "autoCompleteCharsTextBox";
            this.autoCompleteCharsTextBox.Size = new System.Drawing.Size(337, 20);
            this.autoCompleteCharsTextBox.TabIndex = 7;
            // 
            // autoAddParanthesesCheckBox
            // 
            this.autoAddParanthesesCheckBox.AutoSize = true;
            this.autoAddParanthesesCheckBox.Location = new System.Drawing.Point(13, 172);
            this.autoAddParanthesesCheckBox.Name = "autoAddParanthesesCheckBox";
            this.autoAddParanthesesCheckBox.Size = new System.Drawing.Size(329, 17);
            this.autoAddParanthesesCheckBox.TabIndex = 10;
            this.autoAddParanthesesCheckBox.Text = "Auto add method parantheses after inserting method suggestion.";
            this.autoAddParanthesesCheckBox.UseVisualStyleBackColor = true;
            // 
            // autoCompleteCodeBlocksCheckBox
            // 
            this.autoCompleteCodeBlocksCheckBox.AutoSize = true;
            this.autoCompleteCodeBlocksCheckBox.Location = new System.Drawing.Point(13, 149);
            this.autoCompleteCodeBlocksCheckBox.Name = "autoCompleteCodeBlocksCheckBox";
            this.autoCompleteCodeBlocksCheckBox.Size = new System.Drawing.Size(232, 17);
            this.autoCompleteCodeBlocksCheckBox.TabIndex = 9;
            this.autoCompleteCodeBlocksCheckBox.Text = "Auto complete code blocks (e.g. ( ) [ ] \" \" \' \')";
            this.autoCompleteCodeBlocksCheckBox.UseVisualStyleBackColor = true;
            // 
            // autoListMembersCheckBox
            // 
            this.autoListMembersCheckBox.AutoSize = true;
            this.autoListMembersCheckBox.Location = new System.Drawing.Point(13, 14);
            this.autoListMembersCheckBox.Name = "autoListMembersCheckBox";
            this.autoListMembersCheckBox.Size = new System.Drawing.Size(108, 17);
            this.autoListMembersCheckBox.TabIndex = 6;
            this.autoListMembersCheckBox.Text = "Auto list members";
            this.autoListMembersCheckBox.UseVisualStyleBackColor = true;
            this.autoListMembersCheckBox.CheckedChanged += new System.EventHandler(this.autoCompleteCheckBox_CheckedChanged);
            // 
            // AutoCompleteSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.autoAddParanthesesCheckBox);
            this.Controls.Add(this.autoCompleteCodeBlocksCheckBox);
            this.Controls.Add(this.autoListMembersCheckBox);
            this.Name = "AutoCompleteSettingsControl";
            this.Size = new System.Drawing.Size(392, 282);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox autoAddParanthesesCheckBox;
        private System.Windows.Forms.CheckBox autoCompleteCodeBlocksCheckBox;
        private System.Windows.Forms.CheckBox completeOnSpaceBarCheckBox;
        private System.Windows.Forms.TextBox autoCompleteCharsTextBox;
        private System.Windows.Forms.Label commitSelectedItemWhenLabel;
        private System.Windows.Forms.CheckBox autoListMembersCheckBox;
        private System.Windows.Forms.Label showSuggestionsListWhenLabel;
        private System.Windows.Forms.ComboBox showSuggestionsListComboBox;
        private System.Windows.Forms.Panel panel1;
    }
}
