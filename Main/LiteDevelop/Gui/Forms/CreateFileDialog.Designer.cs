namespace LiteDevelop.Gui.Forms
{
    partial class CreateFileDialog
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
            this.nameHeaderLabel = new System.Windows.Forms.Label();
            this.directoryHeaderLabel = new System.Windows.Forms.Label();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.fileNameTextBox = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.languagesTreeView = new System.Windows.Forms.TreeView();
            this.templatesListView = new System.Windows.Forms.ListView();
            this.browseButton = new System.Windows.Forms.Button();
            this.directoryTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // nameHeaderLabel
            // 
            this.nameHeaderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nameHeaderLabel.AutoSize = true;
            this.nameHeaderLabel.Location = new System.Drawing.Point(9, 222);
            this.nameHeaderLabel.Name = "nameHeaderLabel";
            this.nameHeaderLabel.Size = new System.Drawing.Size(38, 13);
            this.nameHeaderLabel.TabIndex = 3;
            this.nameHeaderLabel.Text = "Name:";
            // 
            // directoryHeaderLabel
            // 
            this.directoryHeaderLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.directoryHeaderLabel.AutoSize = true;
            this.directoryHeaderLabel.Location = new System.Drawing.Point(9, 248);
            this.directoryHeaderLabel.Name = "directoryHeaderLabel";
            this.directoryHeaderLabel.Size = new System.Drawing.Size(52, 13);
            this.directoryHeaderLabel.TabIndex = 7;
            this.directoryHeaderLabel.Text = "Directory:";
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.Location = new System.Drawing.Point(428, 272);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "Ok";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(347, 272);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // fileNameTextBox
            // 
            this.fileNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.fileNameTextBox.Location = new System.Drawing.Point(64, 219);
            this.fileNameTextBox.Name = "fileNameTextBox";
            this.fileNameTextBox.Size = new System.Drawing.Size(439, 20);
            this.fileNameTextBox.TabIndex = 2;
            this.fileNameTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CreateFileDialog_KeyUp);
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
            this.splitContainer1.Size = new System.Drawing.Size(491, 201);
            this.splitContainer1.SplitterDistance = 132;
            this.splitContainer1.TabIndex = 4;
            // 
            // languagesTreeView
            // 
            this.languagesTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.languagesTreeView.FullRowSelect = true;
            this.languagesTreeView.HideSelection = false;
            this.languagesTreeView.Location = new System.Drawing.Point(0, 0);
            this.languagesTreeView.Name = "languagesTreeView";
            this.languagesTreeView.ShowLines = false;
            this.languagesTreeView.Size = new System.Drawing.Size(132, 201);
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
            this.templatesListView.Size = new System.Drawing.Size(355, 201);
            this.templatesListView.TabIndex = 0;
            this.templatesListView.UseCompatibleStateImageBehavior = false;
            this.templatesListView.SelectedIndexChanged += new System.EventHandler(this.templatesListView_SelectedIndexChanged);
            // 
            // browseButton
            // 
            this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.browseButton.Location = new System.Drawing.Point(428, 243);
            this.browseButton.Name = "browseButton";
            this.browseButton.Size = new System.Drawing.Size(75, 23);
            this.browseButton.TabIndex = 6;
            this.browseButton.Text = "Browse";
            this.browseButton.UseVisualStyleBackColor = true;
            this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
            // 
            // directoryTextBox
            // 
            this.directoryTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryTextBox.Location = new System.Drawing.Point(64, 245);
            this.directoryTextBox.Name = "directoryTextBox";
            this.directoryTextBox.Size = new System.Drawing.Size(358, 20);
            this.directoryTextBox.TabIndex = 5;
            // 
            // CreateFileDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(515, 307);
            this.Controls.Add(this.directoryHeaderLabel);
            this.Controls.Add(this.browseButton);
            this.Controls.Add(this.directoryTextBox);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.nameHeaderLabel);
            this.Controls.Add(this.fileNameTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CreateFileDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Create new file";
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.CreateFileDialog_KeyUp);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox fileNameTextBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView languagesTreeView;
        private System.Windows.Forms.ListView templatesListView;
        private System.Windows.Forms.Button browseButton;
        private System.Windows.Forms.TextBox directoryTextBox;
        private System.Windows.Forms.Label nameHeaderLabel;
        private System.Windows.Forms.Label directoryHeaderLabel;
    }
}