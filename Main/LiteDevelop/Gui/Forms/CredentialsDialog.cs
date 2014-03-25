using System;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using LiteDevelop.Framework.Extensions;

namespace LiteDevelop.Gui.Forms
{
    public partial class CredentialsDialog : Form
    {
        // required for the designer to work.
        private CredentialsDialog()
        {
            InitializeComponent();
        }

        public CredentialsDialog(CredentialRequest request)
            : this()
        {
            messageLabel.Text = request.Message;
            try
            {
                foreach (var frame in request.StackTrace.GetFrames())
                {
                    stackTraceListView.Items.Add(new ListViewItem(new string[]
                    {
                        frame.GetMethod().ToString(),
                        frame.GetMethod().Module.Assembly.FullName,
                        frame.GetMethod().Module.FullyQualifiedName,
                    }));
                }
            }
            catch (Exception ex)
            {
                stackTraceListView.Items.Add(new ListViewItem(new string[]
                {
                    string.Format("Failed to read stack trace. {0}", ex.Message),
                    ex.TargetSite.Module.Assembly.FullName,
                    ex.TargetSite.Module.FullyQualifiedName,
                }));
            }
        }

        public NetworkCredential GetCredential()
        {
            return new NetworkCredential(usernameTextBox.Text, passwordTextBox.Text);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (MaximumSize.Height != 180)
            {
                MaximumSize = new Size(9999, 180);
                linkLabel1.Text = "What is requesting this?";
            }
            else
            {
                MaximumSize = new Size(9999, 9999);
                Height = 360;
                linkLabel1.Text = "Hide stack trace";
            }
        }
    }
}
