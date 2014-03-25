namespace LiteDevelop.Gui.DockContents
{
    partial class ToolboxContent
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
            this.toolBoxTreeView = new System.Windows.Forms.TreeView();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // toolBoxTreeView
            // 
            this.toolBoxTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolBoxTreeView.FullRowSelect = true;
            this.toolBoxTreeView.HideSelection = false;
            this.toolBoxTreeView.Location = new System.Drawing.Point(0, 0);
            this.toolBoxTreeView.Name = "toolBoxTreeView";
            this.toolBoxTreeView.ShowLines = false;
            this.toolBoxTreeView.ShowNodeToolTips = true;
            this.toolBoxTreeView.Size = new System.Drawing.Size(284, 262);
            this.toolBoxTreeView.TabIndex = 0;
            this.toolBoxTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.toolBoxTreeView_AfterSelect);
            this.toolBoxTreeView.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.toolBoxTreeView_NodeMouseDoubleClick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(260, 244);
            this.label1.TabIndex = 1;
            this.label1.Text = "There are no toolbox items available for this document.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // ToolBoxContent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolBoxTreeView);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ToolBoxContent";
            this.Text = "Toolbox";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView toolBoxTreeView;
        private System.Windows.Forms.Label label1;
    }
}