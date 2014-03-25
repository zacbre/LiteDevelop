namespace LiteDevelop.Gui.Forms
{
    partial class SplashScreen
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
            System.Windows.Forms.Label label4;
            this.fadeInTimer = new System.Windows.Forms.Timer(this.components);
            this.versionLabel = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label4
            // 
            label4.BackColor = System.Drawing.Color.Transparent;
            label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label4.Location = new System.Drawing.Point(261, 239);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(206, 13);
            label4.TabIndex = 3;
            label4.Text = "Loading";
            label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            label4.UseWaitCursor = true;
            // 
            // fadeInTimer
            // 
            this.fadeInTimer.Enabled = true;
            this.fadeInTimer.Interval = 10;
            this.fadeInTimer.Tick += new System.EventHandler(this.fadeInTimer_Tick);
            // 
            // versionLabel
            // 
            this.versionLabel.AutoEllipsis = true;
            this.versionLabel.BackColor = System.Drawing.Color.Transparent;
            this.versionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.Location = new System.Drawing.Point(258, 152);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(209, 18);
            this.versionLabel.TabIndex = 2;
            this.versionLabel.Text = "<version>";
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.versionLabel.UseWaitCursor = true;
            // 
            // SplashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::LiteDevelop.Properties.Resources.Splash_LiteDev;
            this.ClientSize = new System.Drawing.Size(500, 279);
            this.Controls.Add(label4);
            this.Controls.Add(this.versionLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SplashScreen";
            this.Opacity = 0D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SplashScreen";
            this.UseWaitCursor = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer fadeInTimer;
        private System.Windows.Forms.Label versionLabel;
    }
}