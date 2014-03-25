using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LiteDevelop.Gui.Forms
{
    public partial class ErrorDialog : Form
    {
        public ErrorDialog(Exception exception)
        {
            InitializeComponent();
            iconPictureBox.Image = SystemIcons.Error.ToBitmap();
            messageLabel.Text = string.Format(messageLabel.Text, exception.Message);
            repositoryLinkLabel.Text = string.Format(repositoryLinkLabel.Text, LiteDevelopApplication.SourceRepositoryUrl);
        }
    }
}
