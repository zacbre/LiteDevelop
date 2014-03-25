namespace LiteDevelop.Gui.Forms
{
    partial class ErrorDialog
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
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Panel panel1;
            System.Windows.Forms.Label label2;
            System.Windows.Forms.Label label3;
            this.iconPictureBox = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.repositoryLinkLabel = new System.Windows.Forms.LinkLabel();
            this.messageLabel = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            label1 = new System.Windows.Forms.Label();
            panel1 = new System.Windows.Forms.Panel();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.Location = new System.Drawing.Point(60, 14);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(406, 26);
            label1.TabIndex = 0;
            label1.Text = "LiteDevelop encountered an unhandled exception and needs to close.\r\nWe are sorry " +
                "for the inconvenience.";
            // 
            // panel1
            // 
            panel1.BackColor = System.Drawing.Color.White;
            panel1.Controls.Add(this.iconPictureBox);
            panel1.Controls.Add(label1);
            panel1.Dock = System.Windows.Forms.DockStyle.Top;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(506, 60);
            panel1.TabIndex = 1;
            // 
            // iconPictureBox
            // 
            this.iconPictureBox.Location = new System.Drawing.Point(15, 12);
            this.iconPictureBox.Name = "iconPictureBox";
            this.iconPictureBox.Size = new System.Drawing.Size(36, 36);
            this.iconPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.iconPictureBox.TabIndex = 1;
            this.iconPictureBox.TabStop = false;
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(3, 44);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(469, 26);
            label2.TabIndex = 3;
            label2.Text = "To help us resolve this issue, please go to the source repository of this version" +
                " of LiteDevelop and open a new issue with the exception details.\r\n";
            // 
            // label3
            // 
            label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(3, 127);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(409, 13);
            label3.TabIndex = 5;
            label3.Text = "Thank you for the patience and we will try to update the program as soon as possi" +
                "ble.";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(419, 226);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Close";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // repositoryLinkLabel
            // 
            this.repositoryLinkLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.repositoryLinkLabel.AutoSize = true;
            this.repositoryLinkLabel.LinkArea = new System.Windows.Forms.LinkArea(12, 3);
            this.repositoryLinkLabel.Location = new System.Drawing.Point(3, 86);
            this.repositoryLinkLabel.Name = "repositoryLinkLabel";
            this.repositoryLinkLabel.Size = new System.Drawing.Size(78, 17);
            this.repositoryLinkLabel.TabIndex = 4;
            this.repositoryLinkLabel.TabStop = true;
            this.repositoryLinkLabel.Text = "Repository: {0}";
            this.repositoryLinkLabel.UseCompatibleTextRendering = true;
            // 
            // messageLabel
            // 
            this.messageLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.messageLabel.AutoSize = true;
            this.messageLabel.Location = new System.Drawing.Point(3, 12);
            this.messageLabel.Name = "messageLabel";
            this.messageLabel.Size = new System.Drawing.Size(21, 13);
            this.messageLabel.TabIndex = 6;
            this.messageLabel.Text = "{0}";
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(278, 226);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(135, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "View Exception Details";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.messageLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.repositoryLinkLabel, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(15, 66);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(479, 154);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // ErrorDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 261);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Unhandled Exception - LiteDevelop";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPictureBox)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox iconPictureBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.LinkLabel repositoryLinkLabel;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}