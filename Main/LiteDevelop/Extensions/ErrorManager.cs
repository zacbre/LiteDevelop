using System;
using System.Linq;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Extensions
{
    public class ErrorManager : IErrorManager
    {
        public event BuildErrorEventHandler ReportedError;
        public event BuildErrorEventHandler NavigateToErrorRequested;
        
        public void ReportError(BuildError error)
        {
            if (ReportedError != null)
                ReportedError(this, new BuildErrorEventArgs(error));
        }

        public void RequestNavigateToError(BuildError error)
        {
            if (NavigateToErrorRequested != null)
                NavigateToErrorRequested(this, new BuildErrorEventArgs(error));
        }
    }
}
