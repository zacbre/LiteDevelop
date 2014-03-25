namespace LiteDevelop.Gui.Forms
{
    partial class AboutDialog
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
            System.Windows.Forms.Label founderLabel;
            this.projectFounderHeaderLabel = new System.Windows.Forms.Label();
            this.versionHeaderLabel = new System.Windows.Forms.Label();
            this.repositoryHeaderLabel = new System.Windows.Forms.Label();
            this.closeButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.versionLabel = new System.Windows.Forms.Label();
            this.repositoryLinkLabel = new System.Windows.Forms.LinkLabel();
            this.additionalReleaseInfoTextBox = new System.Windows.Forms.TextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.additionalReleaseInfoLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.extensionCopyrightHeaderLabel = new System.Windows.Forms.Label();
            this.extensionVersionHeaderLabel = new System.Windows.Forms.Label();
            this.extensionNameHeaderLabel = new System.Windows.Forms.Label();
            this.extensionNameLabel = new System.Windows.Forms.Label();
            this.extensionVersionLabel = new System.Windows.Forms.Label();
            this.extensionAuthorHeaderLabel = new System.Windows.Forms.Label();
            this.extensionDescriptionHeaderLabel = new System.Windows.Forms.Label();
            this.extensionAuthorLabel = new System.Windows.Forms.Label();
            this.extensionDescriptionLabel = new System.Windows.Forms.Label();
            this.extensionCopyrightLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            founderLabel = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // founderLabel
            // 
            founderLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            founderLabel.AutoEllipsis = true;
            founderLabel.AutoSize = true;
            founderLabel.Location = new System.Drawing.Point(165, 5);
            founderLabel.Name = "founderLabel";
            founderLabel.Size = new System.Drawing.Size(43, 13);
            founderLabel.TabIndex = 3;
            founderLabel.Text = "Jerre S.";
            // 
            // projectFounderHeaderLabel
            // 
            this.projectFounderHeaderLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.projectFounderHeaderLabel.AutoEllipsis = true;
            this.projectFounderHeaderLabel.AutoSize = true;
            this.projectFounderHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.projectFounderHeaderLabel.Location = new System.Drawing.Point(3, 5);
            this.projectFounderHeaderLabel.Name = "projectFounderHeaderLabel";
            this.projectFounderHeaderLabel.Size = new System.Drawing.Size(101, 13);
            this.projectFounderHeaderLabel.TabIndex = 0;
            this.projectFounderHeaderLabel.Text = "Project Founder:";
            // 
            // versionHeaderLabel
            // 
            this.versionHeaderLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.versionHeaderLabel.AutoEllipsis = true;
            this.versionHeaderLabel.AutoSize = true;
            this.versionHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionHeaderLabel.Location = new System.Drawing.Point(3, 29);
            this.versionHeaderLabel.Name = "versionHeaderLabel";
            this.versionHeaderLabel.Size = new System.Drawing.Size(53, 13);
            this.versionHeaderLabel.TabIndex = 5;
            this.versionHeaderLabel.Text = "Version:";
            // 
            // repositoryHeaderLabel
            // 
            this.repositoryHeaderLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.repositoryHeaderLabel.AutoEllipsis = true;
            this.repositoryHeaderLabel.AutoSize = true;
            this.repositoryHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.repositoryHeaderLabel.Location = new System.Drawing.Point(3, 54);
            this.repositoryHeaderLabel.Name = "repositoryHeaderLabel";
            this.repositoryHeaderLabel.Size = new System.Drawing.Size(112, 13);
            this.repositoryHeaderLabel.TabIndex = 7;
            this.repositoryHeaderLabel.Text = "Github Repository:";
            // 
            // closeButton
            // 
            this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.closeButton.Location = new System.Drawing.Point(524, 459);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 0;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "LiteDevelop";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.83146F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 65.16854F));
            this.tableLayoutPanel1.Controls.Add(this.versionHeaderLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.projectFounderHeaderLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(founderLabel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.versionLabel, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.repositoryHeaderLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.repositoryLinkLabel, 1, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(17, 46);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(466, 74);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // versionLabel
            // 
            this.versionLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.versionLabel.AutoEllipsis = true;
            this.versionLabel.AutoSize = true;
            this.versionLabel.Location = new System.Drawing.Point(165, 29);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(94, 13);
            this.versionLabel.TabIndex = 6;
            this.versionLabel.Text = "<program version>";
            // 
            // repositoryLinkLabel
            // 
            this.repositoryLinkLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.repositoryLinkLabel.AutoEllipsis = true;
            this.repositoryLinkLabel.AutoSize = true;
            this.repositoryLinkLabel.Location = new System.Drawing.Point(165, 54);
            this.repositoryLinkLabel.Name = "repositoryLinkLabel";
            this.repositoryLinkLabel.Size = new System.Drawing.Size(91, 13);
            this.repositoryLinkLabel.TabIndex = 8;
            this.repositoryLinkLabel.TabStop = true;
            this.repositoryLinkLabel.Text = "<github repo link>";
            this.repositoryLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.repositoryLinkLabel_LinkClicked);
            // 
            // additionalReleaseInfoTextBox
            // 
            this.additionalReleaseInfoTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.additionalReleaseInfoTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.additionalReleaseInfoTextBox.Location = new System.Drawing.Point(9, 151);
            this.additionalReleaseInfoTextBox.Multiline = true;
            this.additionalReleaseInfoTextBox.Name = "additionalReleaseInfoTextBox";
            this.additionalReleaseInfoTextBox.ReadOnly = true;
            this.additionalReleaseInfoTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.additionalReleaseInfoTextBox.Size = new System.Drawing.Size(355, 173);
            this.additionalReleaseInfoTextBox.TabIndex = 6;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Location = new System.Drawing.Point(17, 124);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.additionalReleaseInfoLabel);
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel2);
            this.splitContainer1.Panel2.Controls.Add(this.additionalReleaseInfoTextBox);
            this.splitContainer1.Size = new System.Drawing.Size(582, 329);
            this.splitContainer1.SplitterDistance = 206;
            this.splitContainer1.TabIndex = 7;
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(204, 327);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.listView1.SizeChanged += new System.EventHandler(this.listView1_SizeChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Tag = "1";
            this.columnHeader1.Text = "Module";
            this.columnHeader1.Width = 140;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Version";
            // 
            // additionalReleaseInfoLabel
            // 
            this.additionalReleaseInfoLabel.AccessibleDescription = " ";
            this.additionalReleaseInfoLabel.AutoSize = true;
            this.additionalReleaseInfoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.additionalReleaseInfoLabel.Location = new System.Drawing.Point(6, 135);
            this.additionalReleaseInfoLabel.Name = "additionalReleaseInfoLabel";
            this.additionalReleaseInfoLabel.Size = new System.Drawing.Size(178, 13);
            this.additionalReleaseInfoLabel.TabIndex = 8;
            this.additionalReleaseInfoLabel.Text = "Additional release information:";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.extensionCopyrightHeaderLabel, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.extensionVersionHeaderLabel, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.extensionNameHeaderLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.extensionNameLabel, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.extensionVersionLabel, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.extensionAuthorHeaderLabel, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.extensionDescriptionHeaderLabel, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.extensionAuthorLabel, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.extensionDescriptionLabel, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.extensionCopyrightLabel, 1, 4);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(361, 120);
            this.tableLayoutPanel2.TabIndex = 7;
            // 
            // extensionCopyrightHeaderLabel
            // 
            this.extensionCopyrightHeaderLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.extensionCopyrightHeaderLabel.AutoEllipsis = true;
            this.extensionCopyrightHeaderLabel.AutoSize = true;
            this.extensionCopyrightHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extensionCopyrightHeaderLabel.Location = new System.Drawing.Point(3, 101);
            this.extensionCopyrightHeaderLabel.Name = "extensionCopyrightHeaderLabel";
            this.extensionCopyrightHeaderLabel.Size = new System.Drawing.Size(64, 13);
            this.extensionCopyrightHeaderLabel.TabIndex = 10;
            this.extensionCopyrightHeaderLabel.Text = "Copyright:";
            // 
            // extensionVersionHeaderLabel
            // 
            this.extensionVersionHeaderLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.extensionVersionHeaderLabel.AutoEllipsis = true;
            this.extensionVersionHeaderLabel.AutoSize = true;
            this.extensionVersionHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extensionVersionHeaderLabel.Location = new System.Drawing.Point(3, 29);
            this.extensionVersionHeaderLabel.Name = "extensionVersionHeaderLabel";
            this.extensionVersionHeaderLabel.Size = new System.Drawing.Size(53, 13);
            this.extensionVersionHeaderLabel.TabIndex = 5;
            this.extensionVersionHeaderLabel.Text = "Version:";
            // 
            // extensionNameHeaderLabel
            // 
            this.extensionNameHeaderLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.extensionNameHeaderLabel.AutoEllipsis = true;
            this.extensionNameHeaderLabel.AutoSize = true;
            this.extensionNameHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extensionNameHeaderLabel.Location = new System.Drawing.Point(3, 5);
            this.extensionNameHeaderLabel.Name = "extensionNameHeaderLabel";
            this.extensionNameHeaderLabel.Size = new System.Drawing.Size(43, 13);
            this.extensionNameHeaderLabel.TabIndex = 0;
            this.extensionNameHeaderLabel.Text = "Name:";
            // 
            // extensionNameLabel
            // 
            this.extensionNameLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.extensionNameLabel.AutoEllipsis = true;
            this.extensionNameLabel.AutoSize = true;
            this.extensionNameLabel.Location = new System.Drawing.Point(103, 5);
            this.extensionNameLabel.Name = "extensionNameLabel";
            this.extensionNameLabel.Size = new System.Drawing.Size(255, 13);
            this.extensionNameLabel.TabIndex = 3;
            this.extensionNameLabel.Text = " ";
            this.extensionNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // extensionVersionLabel
            // 
            this.extensionVersionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.extensionVersionLabel.AutoEllipsis = true;
            this.extensionVersionLabel.AutoSize = true;
            this.extensionVersionLabel.Location = new System.Drawing.Point(103, 29);
            this.extensionVersionLabel.Name = "extensionVersionLabel";
            this.extensionVersionLabel.Size = new System.Drawing.Size(255, 13);
            this.extensionVersionLabel.TabIndex = 6;
            this.extensionVersionLabel.Text = " ";
            this.extensionVersionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // extensionAuthorHeaderLabel
            // 
            this.extensionAuthorHeaderLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.extensionAuthorHeaderLabel.AutoEllipsis = true;
            this.extensionAuthorHeaderLabel.AutoSize = true;
            this.extensionAuthorHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extensionAuthorHeaderLabel.Location = new System.Drawing.Point(3, 53);
            this.extensionAuthorHeaderLabel.Name = "extensionAuthorHeaderLabel";
            this.extensionAuthorHeaderLabel.Size = new System.Drawing.Size(48, 13);
            this.extensionAuthorHeaderLabel.TabIndex = 7;
            this.extensionAuthorHeaderLabel.Text = "Author:";
            // 
            // extensionDescriptionHeaderLabel
            // 
            this.extensionDescriptionHeaderLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.extensionDescriptionHeaderLabel.AutoEllipsis = true;
            this.extensionDescriptionHeaderLabel.AutoSize = true;
            this.extensionDescriptionHeaderLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extensionDescriptionHeaderLabel.Location = new System.Drawing.Point(3, 77);
            this.extensionDescriptionHeaderLabel.Name = "extensionDescriptionHeaderLabel";
            this.extensionDescriptionHeaderLabel.Size = new System.Drawing.Size(75, 13);
            this.extensionDescriptionHeaderLabel.TabIndex = 9;
            this.extensionDescriptionHeaderLabel.Text = "Description:";
            // 
            // extensionAuthorLabel
            // 
            this.extensionAuthorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.extensionAuthorLabel.AutoEllipsis = true;
            this.extensionAuthorLabel.AutoSize = true;
            this.extensionAuthorLabel.Location = new System.Drawing.Point(103, 53);
            this.extensionAuthorLabel.Name = "extensionAuthorLabel";
            this.extensionAuthorLabel.Size = new System.Drawing.Size(255, 13);
            this.extensionAuthorLabel.TabIndex = 11;
            this.extensionAuthorLabel.Text = " ";
            this.extensionAuthorLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // extensionDescriptionLabel
            // 
            this.extensionDescriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.extensionDescriptionLabel.AutoEllipsis = true;
            this.extensionDescriptionLabel.AutoSize = true;
            this.extensionDescriptionLabel.Location = new System.Drawing.Point(103, 77);
            this.extensionDescriptionLabel.Name = "extensionDescriptionLabel";
            this.extensionDescriptionLabel.Size = new System.Drawing.Size(255, 13);
            this.extensionDescriptionLabel.TabIndex = 12;
            this.extensionDescriptionLabel.Text = " ";
            this.extensionDescriptionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // extensionCopyrightLabel
            // 
            this.extensionCopyrightLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.extensionCopyrightLabel.AutoEllipsis = true;
            this.extensionCopyrightLabel.AutoSize = true;
            this.extensionCopyrightLabel.Location = new System.Drawing.Point(103, 101);
            this.extensionCopyrightLabel.Name = "extensionCopyrightLabel";
            this.extensionCopyrightLabel.Size = new System.Drawing.Size(255, 13);
            this.extensionCopyrightLabel.TabIndex = 13;
            this.extensionCopyrightLabel.Text = " ";
            this.extensionCopyrightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::LiteDevelop.Properties.Resources.feather;
            this.pictureBox1.Location = new System.Drawing.Point(489, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(110, 110);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // AboutDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(611, 494);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.closeButton);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "About";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button closeButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label versionLabel;
        private System.Windows.Forms.LinkLabel repositoryLinkLabel;
        private System.Windows.Forms.TextBox additionalReleaseInfoTextBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label projectFounderHeaderLabel;
        private System.Windows.Forms.Label versionHeaderLabel;
        private System.Windows.Forms.Label repositoryHeaderLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label extensionVersionHeaderLabel;
        private System.Windows.Forms.Label extensionNameHeaderLabel;
        private System.Windows.Forms.Label extensionVersionLabel;
        private System.Windows.Forms.Label extensionAuthorHeaderLabel;
        private System.Windows.Forms.Label additionalReleaseInfoLabel;
        private System.Windows.Forms.Label extensionCopyrightHeaderLabel;
        private System.Windows.Forms.Label extensionDescriptionHeaderLabel;
        private System.Windows.Forms.Label extensionAuthorLabel;
        private System.Windows.Forms.Label extensionDescriptionLabel;
        private System.Windows.Forms.Label extensionCopyrightLabel;
        private System.Windows.Forms.Label extensionNameLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}