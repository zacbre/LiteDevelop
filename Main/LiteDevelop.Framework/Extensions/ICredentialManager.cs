using System;
using System.Linq;
using System.Net;

namespace LiteDevelop.Framework.Extensions
{
    public interface ICredentialManager
    {
        bool RequestCredential(CredentialRequest request, out NetworkCredential credential);
    }
}
