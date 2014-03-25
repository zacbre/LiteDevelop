namespace LiteDevelop.Essentials.CodeEditor.Gui
{
    partial class GeneralSettingsControl
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
            this.lineNumbersCheckBox = new System.Windows.Forms.CheckBox();
            this.wordWrapCheckBox = new System.Windows.Forms.CheckBox();
            this.syntaxHighlightingCheckBox = new System.Windows.Forms.CheckBox();
            this.trackChangesCheckBox = new System.Windows.Forms.CheckBox();
            this.highLightCurrentLineCheckBox = new System.Windows.Forms.CheckBox();
            this.documentMiniMapCheckBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lineNumbersCheckBox
            // 
            this.lineNumbersCheckBox.AutoSize = true;
            this.lineNumbersCheckBox.Location = new System.Drawing.Point(13, 14);
            this.lineNumbersCheckBox.Name = "lineNumbersCheckBox";
            this.lineNumbersCheckBox.Size = new System.Drawing.Size(115, 17);
            this.lineNumbersCheckBox.TabIndex = 5;
            this.lineNumbersCheckBox.Text = "Show line numbers";
            this.lineNumbersCheckBox.UseVisualStyleBackColor = true;
            // 
            // wordWrapCheckBox
            // 
            this.wordWrapCheckBox.AutoSize = true;
            this.wordWrapCheckBox.Location = new System.Drawing.Point(13, 37);
            this.wordWrapCheckBox.Name = "wordWrapCheckBox";
            this.wordWrapCheckBox.Size = new System.Drawing.Size(78, 17);
            this.wordWrapCheckBox.TabIndex = 6;
            this.wordWrapCheckBox.Text = "Word wrap";
            this.wordWrapCheckBox.UseVisualStyleBackColor = true;
            // 
            // syntaxHighlightingCheckBox
            // 
            this.syntaxHighlightingCheckBox.AutoSize = true;
            this.syntaxHighlightingCheckBox.Location = new System.Drawing.Point(13, 60);
            this.syntaxHighlightingCheckBox.Name = "syntaxHighlightingCheckBox";
            this.syntaxHighlightingCheckBox.Size = new System.Drawing.Size(114, 17);
            this.syntaxHighlightingCheckBox.TabIndex = 7;
            this.syntaxHighlightingCheckBox.Text = "Syntax highlighting";
            this.syntaxHighlightingCheckBox.UseVisualStyleBackColor = true;
            // 
            // trackChangesCheckBox
            // 
            this.trackChangesCheckBox.AutoSize = true;
            this.trackChangesCheckBox.Location = new System.Drawing.Point(13, 83);
            this.trackChangesCheckBox.Name = "trackChangesCheckBox";
            this.trackChangesCheckBox.Size = new System.Drawing.Size(142, 17);
            this.trackChangesCheckBox.TabIndex = 8;
            this.trackChangesCheckBox.Text = "Track unsaved changes";
            this.trackChangesCheckBox.UseVisualStyleBackColor = true;
            // 
            // highLightCurrentLineCheckBox
            // 
            this.highLightCurrentLineCheckBox.AutoSize = true;
            this.highLightCurrentLineCheckBox.Location = new System.Drawing.Point(13, 106);
            this.highLightCurrentLineCheckBox.Name = "highLightCurrentLineCheckBox";
            this.highLightCurrentLineCheckBox.Size = new System.Drawing.Size(122, 17);
            this.highLightCurrentLineCheckBox.TabIndex = 9;
            this.highLightCurrentLineCheckBox.Text = "Highlight current line";
            this.highLightCurrentLineCheckBox.UseVisualStyleBackColor = true;
            // 
            // documentMiniMapCheckBox
            // 
            this.documentMiniMapCheckBox.AutoSize = true;
            this.documentMiniMapCheckBox.Location = new System.Drawing.Point(13, 129);
            this.documentMiniMapCheckBox.Name = "documentMiniMapCheckBox";
            this.documentMiniMapCheckBox.Size = new System.Drawing.Size(147, 17);
            this.documentMiniMapCheckBox.TabIndex = 10;
            this.documentMiniMapCheckBox.Text = "Show document mini map";
            this.documentMiniMapCheckBox.UseVisualStyleBackColor = true;
            // 
            // GeneralSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.documentMiniMapCheckBox);
            this.Controls.Add(this.highLightCurrentLineCheckBox);
            this.Controls.Add(this.trackChangesCheckBox);
            this.Controls.Add(this.syntaxHighlightingCheckBox);
            this.Controls.Add(this.wordWrapCheckBox);
            this.Controls.Add(this.lineNumbersCheckBox);
            this.Name = "GeneralSettingsControl";
            this.Size = new System.Drawing.Size(352, 223);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox lineNumbersCheckBox;
        private System.Windows.Forms.CheckBox wordWrapCheckBox;
        private System.Windows.Forms.CheckBox syntaxHighlightingCheckBox;
        private System.Windows.Forms.CheckBox trackChangesCheckBox;
        private System.Windows.Forms.CheckBox highLightCurrentLineCheckBox;
        private System.Windows.Forms.CheckBox documentMiniMapCheckBox;
    }
}
