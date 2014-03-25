using System;
using System.Diagnostics;
using System.Linq;

namespace LiteDevelop.Framework.Extensions
{
    public class CredentialRequest
    {
        private readonly StackTrace _stackTrace;
        private readonly string _message;

        public CredentialRequest(string message)
        {
            _message = message;
            _stackTrace = new StackTrace();
        }

        public string Message
        {
            get { return _message; }
        }

        public StackTrace StackTrace
        {
            get { return _stackTrace; }
        }
    }
}
