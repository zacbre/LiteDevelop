using System;
using System.Linq;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Framework.Extensions
{
    /// <summary>
    /// Provides methods for reporting and navigating to errors.
    /// </summary>
    public interface IErrorManager
    {
        /// <summary>
        /// Occurs when an error has been reported.
        /// </summary>
        event BuildErrorEventHandler ReportedError;

        /// <summary>
        /// Occurs when a error navigation request has been sent.
        /// </summary>
        event BuildErrorEventHandler NavigateToErrorRequested;

        /// <summary>
        /// Reports an error.
        /// </summary>
        /// <param name="error">The error to report.</param>
        void ReportError(BuildError error);

        /// <summary>
        /// Requests to navigate to a specific error.
        /// </summary>
        /// <param name="error">The error to navigate to.</param>
        void RequestNavigateToError(BuildError error);
    }
}
