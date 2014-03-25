namespace LiteDevelop.Gui.Settings
{
    partial class GeneralSettingsEditor
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
        	this.defaultProjectsPathLabel = new System.Windows.Forms.Label();
        	this.outputWindowCheckBox = new System.Windows.Forms.CheckBox();
        	this.defaultPathTextBox = new System.Windows.Forms.TextBox();
        	this.errorListCheckBox = new System.Windows.Forms.CheckBox();
        	this.browseButton = new System.Windows.Forms.Button();
        	this.SuspendLayout();
        	// 
        	// defaultProjectsPathLabel
        	// 
        	this.defaultProjectsPathLabel.AutoSize = true;
        	this.defaultProjectsPathLabel.Location = new System.Drawing.Point(9, 11);
        	this.defaultProjectsPathLabel.Name = "defaultProjectsPathLabel";
        	this.defaultProjectsPathLabel.Size = new System.Drawing.Size(108, 13);
        	this.defaultProjectsPathLabel.TabIndex = 1;
        	this.defaultProjectsPathLabel.Text = "Default projects path:";
        	// 
        	// outputWindowCheckBox
        	// 
        	this.outputWindowCheckBox.AutoSize = true;
        	this.outputWindowCheckBox.Location = new System.Drawing.Point(12, 63);
        	this.outputWindowCheckBox.Name = "outputWindowCheckBox";
        	this.outputWindowCheckBox.Size = new System.Drawing.Size(196, 17);
        	this.outputWindowCheckBox.TabIndex = 0;
        	this.outputWindowCheckBox.Text = "Show output window when building.";
        	this.outputWindowCheckBox.UseVisualStyleBackColor = true;
        	// 
        	// defaultPathTextBox
        	// 
        	this.defaultPathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
			| System.Windows.Forms.AnchorStyles.Right)));
        	this.defaultPathTextBox.Location = new System.Drawing.Point(12, 27);
        	this.defaultPathTextBox.Name = "defaultPathTextBox";
        	this.defaultPathTextBox.Size = new System.Drawing.Size(192, 20);
        	this.defaultPathTextBox.TabIndex = 2;
        	// 
        	// errorListCheckBox
        	// 
        	this.errorListCheckBox.AutoSize = true;
        	this.errorListCheckBox.Location = new System.Drawing.Point(12, 86);
        	this.errorListCheckBox.Name = "errorListCheckBox";
        	this.errorListCheckBox.Size = new System.Drawing.Size(177, 17);
        	this.errorListCheckBox.TabIndex = 3;
        	this.errorListCheckBox.Text = "Show error list when build failed.";
        	this.errorListCheckBox.UseVisualStyleBackColor = true;
        	// 
        	// browseButton
        	// 
        	this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        	this.browseButton.Location = new System.Drawing.Point(210, 25);
        	this.browseButton.Name = "browseButton";
        	this.browseButton.Size = new System.Drawing.Size(75, 23);
        	this.browseButton.TabIndex = 4;
        	this.browseButton.Text = "Browse";
        	this.browseButton.UseVisualStyleBackColor = true;
        	this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
        	// 
        	// GeneralSettingsEditor
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.Controls.Add(this.browseButton);
        	this.Controls.Add(this.errorListCheckBox);
        	this.Controls.Add(this.defaultPathTextBox);
        	this.Controls.Add(this.defaultProjectsPathLabel);
        	this.Controls.Add(this.outputWindowCheckBox);
        	this.Name = "GeneralSettingsEditor";
        	this.Size = new System.Drawing.Size(288, 202);
        	this.ResumeLayout(false);
        	this.PerformLayout();
        }
        private System.Windows.Forms.Button browseButton;

        #endregion

        private System.Windows.Forms.CheckBox outputWindowCheckBox;
        private System.Windows.Forms.TextBox defaultPathTextBox;
        private System.Windows.Forms.CheckBox errorListCheckBox;
        private System.Windows.Forms.Label defaultProjectsPathLabel;
    }
}
