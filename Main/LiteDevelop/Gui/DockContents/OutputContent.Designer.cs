namespace LiteDevelop.Gui.DockContents
{
    partial class OutputContent
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.showOutputFromLabel = new System.Windows.Forms.ToolStripLabel();
            this.outputSourcesToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.clearAllToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.wordWrapToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showOutputFromLabel,
            this.outputSourcesToolStripComboBox,
            this.toolStripSeparator2,
            this.clearAllToolStripButton,
            this.toolStripSeparator1,
            this.wordWrapToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(562, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // showOutputFromLabel
            // 
            this.showOutputFromLabel.Name = "showOutputFromLabel";
            this.showOutputFromLabel.Size = new System.Drawing.Size(107, 22);
            this.showOutputFromLabel.Text = "Show output from:";
            // 
            // outputSourcesToolStripComboBox
            // 
            this.outputSourcesToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.outputSourcesToolStripComboBox.Name = "outputSourcesToolStripComboBox";
            this.outputSourcesToolStripComboBox.Size = new System.Drawing.Size(121, 25);
            this.outputSourcesToolStripComboBox.SelectedIndexChanged += new System.EventHandler(this.outputSourcesToolStripComboBox_SelectedIndexChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // clearAllToolStripButton
            // 
            this.clearAllToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.clearAllToolStripButton.Image = global::LiteDevelop.Properties.Resources.remove;
            this.clearAllToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.clearAllToolStripButton.Name = "clearAllToolStripButton";
            this.clearAllToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.clearAllToolStripButton.Text = "Clear all";
            this.clearAllToolStripButton.Click += new System.EventHandler(this.clearAllToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // wordWrapToolStripButton
            // 
            this.wordWrapToolStripButton.CheckOnClick = true;
            this.wordWrapToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.wordWrapToolStripButton.Image = global::LiteDevelop.Properties.Resources.text;
            this.wordWrapToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.wordWrapToolStripButton.Name = "wordWrapToolStripButton";
            this.wordWrapToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.wordWrapToolStripButton.Text = "Toggle word wrap";
            this.wordWrapToolStripButton.Click += new System.EventHandler(this.wordWrapToolStripButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Window;
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(0, 25);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(562, 233);
            this.textBox1.TabIndex = 2;
            this.textBox1.WordWrap = false;
            // 
            // OutputContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 258);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "OutputContent";
            this.Text = "Output";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel showOutputFromLabel;
        private System.Windows.Forms.ToolStripComboBox outputSourcesToolStripComboBox;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton clearAllToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton wordWrapToolStripButton;
        private System.Windows.Forms.TextBox textBox1;
    }
}