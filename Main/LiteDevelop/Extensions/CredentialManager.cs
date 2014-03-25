using System;
using System.Linq;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Gui.Forms;

namespace LiteDevelop.Extensions
{
    public class CredentialManager : ICredentialManager
    {
        public bool RequestCredential(CredentialRequest request, out System.Net.NetworkCredential credential)
        {
            credential = null;

            var dialog = new CredentialsDialog(request);
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                credential = dialog.GetCredential();
                return true;
            }

            return false;
        }
    }
}
