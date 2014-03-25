namespace LiteDevelop.Gui.Forms
{
    partial class CreateProjectDialog
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
            this.directoryHeaderLabel = new System.Windows.Forms.Label();
            this.nameHeaderLabel = new System.Windows.Forms.Label();
            this.browseButton = new System.Windows.Forms.Button();
            this.directoryTextBox = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.languagesTreeView = new System.Windows.Forms.TreeView();
            this.templatesListView = new System.Windows.Forms.ListView();
            this.fileNameTextBox = new System.Windows.Forms.TextBox();
            this.cancelButton = new System.Windows.Forms.Button();
            this.okButton = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // directoryHeaderLabel
            // 
            this.directoryHeaderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.directoryHeaderLabel.AutoSize = true;
            this.directoryHeaderLabel.Location = new System.Drawing.Point(9, 251);
            this.directoryHeaderLabel.Name = "directoryHeaderLabel";
            this.directoryHeaderLabel.Size = new System.Drawing.Size(52, 13);
            this.directoryHeaderLabel.TabIndex = 14;
            this.directoryHeaderLabel.Text = "Directory:";
            // 
            // nameHeaderLabel
            // 
            this.nameHeaderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nameHeaderLabel.AutoSize = true;
            this.nameHeaderLabel.Location = new System.Drawing.Point(9, 225);
            this.nameHeaderLabel.Name = "nameHeaderLabel";
            this.nameHeaderLabel.Size = new System.Drawing.Size(38, 13);
            this.nameHeaderLabel.TabIndex = 15;
            this.nameHeaderLabel.Text = "Name:";
            // 
            // browseButton
            // 
            this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.browseButton.Location = new System.Drawing.Point(439, 246);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 13;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // directoryTextBox
            // 
            this.directoryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryTextBox.Location = new System.Drawing.Point(64, 248);
            this.directoryTextBox.Name = "directoryTextBox";
            this.directoryTextBox.Size = new System.Drawing.Size(369, 20);
            this.directoryTextBox.TabIndex = 12;
            this.directoryTextBox.TextChanged += new System.EventHandler(this.control_Changed);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.languagesTreeView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.templatesListView);
            this.splitContainer1.Size = new System.Drawing.Size(502, 204);
            this.splitContainer1.SplitterDistance = 134;
            this.splitContainer1.TabIndex = 11;
            // 
            // languagesTreeView
            // 
            this.languagesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.languagesTreeView.FullRowSelect = true;
            this.languagesTreeView.HideSelection = false;
            this.languagesTreeView.Location = new System.Drawing.Point(0, 0);
            this.languagesTreeView.Name = "languagesTreeView";
            this.languagesTreeView.ShowLines = false;
            this.languagesTreeView.Size = new System.Drawing.Size(134, 204);
            this.languagesTreeView.TabIndex = 0;
            this.languagesTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.languagesTreeView_AfterSelect);
            // 
            // templatesListView
            // 
            this.templatesListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.templatesListView.HideSelection = false;
            this.templatesListView.Location = new System.Drawing.Point(0, 0);
            this.templatesListView.MultiSelect = false;
            this.templatesListView.Name = "templatesListView";
            this.templatesListView.Size = new System.Drawing.Size(364, 204);
            this.templatesListView.TabIndex = 0;
            this.templatesListView.UseCompatibleStateImageBehavior = false;
            this.templatesListView.SelectedIndexChanged += new System.EventHandler(this.control_Changed);
            // 
            // fileNameTextBox
            // 
            this.fileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileNameTextBox.Location = new System.Drawing.Point(64, 222);
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.Size = new System.Drawing.Size(450, 20);
            this.fileNameTextBox.TabIndex = 10;
            this.fileNameTextBox.TextChanged += new System.EventHandler(this.control_Changed);
            this.fileNameTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CreateProjectDialog_KeyUp);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(358, 275);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 9;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(439, 275);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 8;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(64, 279);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(154, 17);
            this.checkBox1.TabIndex = 16;
            this.checkBox1.Text = "Create directory for solution";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // CreateProjectDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 313);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.nameHeaderLabel);
            this.Controls.Add(this.directoryHeaderLabel);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.directoryTextBox);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.fileNameTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateProjectDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create new project";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CreateProjectDialog_KeyUp);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.TextBox directoryTextBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView languagesTreeView;
        private System.Windows.Forms.ListView templatesListView;
        private System.Windows.Forms.TextBox fileNameTextBox;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Label directoryHeaderLabel;
        private System.Windows.Forms.Label nameHeaderLabel;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}