using System;
using System.Linq;

namespace LiteDevelop.Framework
{
    /// <summary>
    /// Provides members for progress logging.
    /// </summary>
    public interface IProgressReporter
    {
        /// <summary>
        /// Reports a message to the logger.
        /// </summary>
        /// <param name="severity">The severity of the message.</param>
        /// <param name="message">The message to send.</param>
        void Report(MessageSeverity severity, string message);

        /// <summary>
        /// Gets or sets the current progress percentage, usually visualized in a progressbar.
        /// </summary>
        int ProgressPercentage { get; set; }

        /// <summary>
        /// Gets or sets the visibility of the progress, usually visualized in a progressbar.
        /// </summary>
        bool ProgressVisible { get; set; }
    }

    public interface INamedProgressReporter : IProgressReporter
    {
        event EventHandler DisplayNameChanged;

        string Identifier
        {
            get;
        }

        string DisplayName
        {
            get;
            set;
        }
    }

    public static class IProgressReporterExtensions
    {
        /// <summary>
        /// Reports a message to the logger with the lowest severity.
        /// </summary>
        /// <param name="reporter">The reporter to send the message to.</param>
        /// <param name="message">The message to send.</param>
        public static void Report(this IProgressReporter reporter, string message)
        {
            reporter.Report(MessageSeverity.Message, message);
        }

        /// <summary>
        /// Formats and reports a message to the logger with the lowest severity.
        /// </summary>
        /// <param name="reporter">The reporter to send the message to.</param>
        /// <param name="format">The format of the message to send.</param>
        /// <param name="arguments">An object array containing zero or more arguments to be formatted.</param>
        public static void Report(this IProgressReporter reporter, string format, params object[] arguments)
        {
            reporter.Report(MessageSeverity.Message, string.Format(format, arguments));
        }

        /// <summary>
        /// Formats and reports a message to the logger.
        /// </summary>
        /// <param name="reporter">The reporter to send the message to.</param>
        /// <param name="severity">The severity of the message.</param>
        /// <param name="format">The format of the message to send.</param>
        /// <param name="arguments">An object array containing zero or more arguments to be formatted.</param>
        public static void Report(this IProgressReporter reporter, MessageSeverity severity, string format, params object[] arguments)
        {
            reporter.Report(severity, string.Format(format, arguments));
        }
    }

    /// <summary>
    /// An enumeration indicating the severity of a message.
    /// </summary>
    public enum MessageSeverity
    {
        Message,
        Warning,
        Error,
        Success,
    }

    public sealed class EmptyProgressReporter : IProgressReporter
    {
        private static EmptyProgressReporter _instance;

        public static EmptyProgressReporter Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EmptyProgressReporter();
                return _instance;
            }
        }

        #region IProgressReporter Members

        public void Report(MessageSeverity severity, string message)
        {
        }

        public int ProgressPercentage
        {
            get;
            set;
        }

        public bool ProgressVisible
        {
            get;
            set;
        }

        #endregion
    }
}
