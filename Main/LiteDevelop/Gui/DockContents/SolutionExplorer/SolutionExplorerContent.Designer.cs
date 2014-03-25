namespace LiteDevelop.Gui.DockContents.SolutionExplorer
{
    partial class SolutionExplorerContent
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
            this.mainTreeView = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.existingItemSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.existingFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.startupProjectSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.setAsStartupProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.explorerSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.viewInExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTreeView
            // 
            this.mainTreeView.ContextMenuStrip = this.contextMenuStrip1;
            this.mainTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTreeView.FullRowSelect = true;
            this.mainTreeView.HideSelection = false;
            this.mainTreeView.LabelEdit = true;
            this.mainTreeView.Location = new System.Drawing.Point(0, 0);
            this.mainTreeView.Name = "mainTreeView";
            this.mainTreeView.ShowLines = false;
            this.mainTreeView.ShowNodeToolTips = true;
            this.mainTreeView.Size = new System.Drawing.Size(282, 380);
            this.mainTreeView.TabIndex = 0;
            this.mainTreeView.AfterLabelEdit += new System.Windows.Forms.NodeLabelEditEventHandler(this.mainTreeView_AfterLabelEdit);
            this.mainTreeView.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.mainTreeView_AfterSelect);
            this.mainTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.mainTreeView_AfterSelect);
            this.mainTreeView.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.mainTreeView_NodeMouseClick);
            this.mainTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.mainTreeView_NodeMouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.openToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.startupProjectSeparator,
            this.setAsStartupProjectToolStripMenuItem,
            this.explorerSeparator,
            this.viewInExplorerToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(186, 148);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newFileToolStripMenuItem,
            this.newDirectoryToolStripMenuItem,
            this.newProjectToolStripMenuItem,
            this.existingItemSeparator,
            this.existingFileToolStripMenuItem});
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.addToolStripMenuItem.Text = "Add";
            // 
            // newFileToolStripMenuItem
            // 
            this.newFileToolStripMenuItem.Name = "newFileToolStripMenuItem";
            this.newFileToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newFileToolStripMenuItem.Text = "New File";
            this.newFileToolStripMenuItem.Click += new System.EventHandler(this.newFileToolStripMenuItem_Click);
            // 
            // newDirectoryToolStripMenuItem
            // 
            this.newDirectoryToolStripMenuItem.Name = "newDirectoryToolStripMenuItem";
            this.newDirectoryToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newDirectoryToolStripMenuItem.Text = "New Directory";
            this.newDirectoryToolStripMenuItem.Click += new System.EventHandler(this.newDirectoryToolStripMenuItem_Click);
            // 
            // newProjectToolStripMenuItem
            // 
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            this.newProjectToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.newProjectToolStripMenuItem.Text = "New Project";
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.newProjectToolStripMenuItem_Click);
            // 
            // existingItemSeparator
            // 
            this.existingItemSeparator.Name = "existingItemSeparator";
            this.existingItemSeparator.Size = new System.Drawing.Size(149, 6);
            // 
            // existingFileToolStripMenuItem
            // 
            this.existingFileToolStripMenuItem.Name = "existingFileToolStripMenuItem";
            this.existingFileToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.existingFileToolStripMenuItem.Text = "Existing File...";
            this.existingFileToolStripMenuItem.Click += new System.EventHandler(this.existingFileToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // startupProjectSeparator
            // 
            this.startupProjectSeparator.Name = "startupProjectSeparator";
            this.startupProjectSeparator.Size = new System.Drawing.Size(182, 6);
            // 
            // setAsStartupProjectToolStripMenuItem
            // 
            this.setAsStartupProjectToolStripMenuItem.Name = "setAsStartupProjectToolStripMenuItem";
            this.setAsStartupProjectToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.setAsStartupProjectToolStripMenuItem.Text = "Set as Startup Project";
            this.setAsStartupProjectToolStripMenuItem.Click += new System.EventHandler(this.setAsStartupProjectToolStripMenuItem_Click);
            // 
            // explorerSeparator
            // 
            this.explorerSeparator.Name = "explorerSeparator";
            this.explorerSeparator.Size = new System.Drawing.Size(182, 6);
            // 
            // viewInExplorerToolStripMenuItem
            // 
            this.viewInExplorerToolStripMenuItem.Name = "viewInExplorerToolStripMenuItem";
            this.viewInExplorerToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.viewInExplorerToolStripMenuItem.Text = "View in Explorer";
            this.viewInExplorerToolStripMenuItem.Click += new System.EventHandler(this.viewInExplorerToolStripMenuItem_Click);
            // 
            // SolutionExplorerContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(282, 380);
            this.Controls.Add(this.mainTreeView);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SolutionExplorerContent";
            this.Text = "Solution Explorer";
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView mainTreeView;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setAsStartupProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewInExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem existingFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator existingItemSeparator;
        private System.Windows.Forms.ToolStripSeparator startupProjectSeparator;
        private System.Windows.Forms.ToolStripSeparator explorerSeparator;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
    }
}