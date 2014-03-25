namespace LiteDevelop.Gui.DockContents
{
    partial class ErrorContent
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorContent));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.goToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.errorsToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.warningsToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.messagesToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.listView1 = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.contextMenuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.toolStripSeparator1,
            this.goToFileToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(123, 54);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(119, 6);
            // 
            // goToFileToolStripMenuItem
            // 
            this.goToFileToolStripMenuItem.Name = "goToFileToolStripMenuItem";
            this.goToFileToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.goToFileToolStripMenuItem.Text = "Go to file";
            this.goToFileToolStripMenuItem.Click += new System.EventHandler(this.goToFileToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.errorsToolStripButton,
            this.toolStripSeparator2,
            this.warningsToolStripButton,
            this.toolStripSeparator3,
            this.messagesToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(405, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // errorsToolStripButton
            // 
            this.errorsToolStripButton.Checked = true;
            this.errorsToolStripButton.CheckOnClick = true;
            this.errorsToolStripButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.errorsToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("errorsToolStripButton.Image")));
            this.errorsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.errorsToolStripButton.Name = "errorsToolStripButton";
            this.errorsToolStripButton.Size = new System.Drawing.Size(66, 22);
            this.errorsToolStripButton.Text = "0 Errors";
            this.errorsToolStripButton.CheckedChanged += new System.EventHandler(this.toolStripButtons_CheckedChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // warningsToolStripButton
            // 
            this.warningsToolStripButton.Checked = true;
            this.warningsToolStripButton.CheckOnClick = true;
            this.warningsToolStripButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.warningsToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("warningsToolStripButton.Image")));
            this.warningsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.warningsToolStripButton.Name = "warningsToolStripButton";
            this.warningsToolStripButton.Size = new System.Drawing.Size(86, 22);
            this.warningsToolStripButton.Text = "0 Warnings";
            this.warningsToolStripButton.CheckedChanged += new System.EventHandler(this.toolStripButtons_CheckedChanged);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // messagesToolStripButton
            // 
            this.messagesToolStripButton.Checked = true;
            this.messagesToolStripButton.CheckOnClick = true;
            this.messagesToolStripButton.CheckState = System.Windows.Forms.CheckState.Checked;
            this.messagesToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("messagesToolStripButton.Image")));
            this.messagesToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.messagesToolStripButton.Name = "messagesToolStripButton";
            this.messagesToolStripButton.Size = new System.Drawing.Size(87, 22);
            this.messagesToolStripButton.Text = "0 Messages";
            this.messagesToolStripButton.CheckedChanged += new System.EventHandler(this.toolStripButtons_CheckedChanged);
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listView1.ContextMenuStrip = this.contextMenuStrip1;
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.Location = new System.Drawing.Point(0, 25);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(405, 252);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.SizeChanged += new System.EventHandler(this.listView1_SizeChanged);
            this.listView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.listView1_MouseDoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Tag = "5";
            this.columnHeader1.Text = "Message";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Tag = "";
            this.columnHeader2.Text = "File";
            this.columnHeader2.Width = 91;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Line";
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Column";
            // 
            // ErrorContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(405, 277);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ErrorContent";
            this.Text = "Error List";
            this.contextMenuStrip1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem goToFileToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton errorsToolStripButton;
        private System.Windows.Forms.ToolStripButton warningsToolStripButton;
        private System.Windows.Forms.ToolStripButton messagesToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}