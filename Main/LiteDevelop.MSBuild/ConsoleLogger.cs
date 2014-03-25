using System;
using System.Linq;
using Microsoft.Build.Framework;

namespace LiteDevelop.MSBuild
{
    public class ConsoleLogger : ILogger
    {
        private IEventSource _eventSource;

        public void Initialize(IEventSource eventSource)
        {
            _eventSource = eventSource;
            eventSource.ErrorRaised += eventSource_ErrorRaised;
            eventSource.MessageRaised += eventSource_MessageRaised;
            eventSource.WarningRaised += eventSource_WarningRaised;
            eventSource.ProjectStarted += eventSource_ProjectStarted;
            eventSource.ProjectFinished += eventSource_ProjectFinished;
        }
        
        private void eventSource_ProjectStarted(object sender, ProjectStartedEventArgs e)
        {
        	Console.WriteLine("project started: file=\"{0}\"", e.ProjectFile);
        }

        private void eventSource_ProjectFinished(object sender, ProjectFinishedEventArgs e)
        {
        	Console.WriteLine("project finished: file=\"{0}\"", e.ProjectFile);
        }
        
        private void eventSource_WarningRaised(object sender, BuildWarningEventArgs e)
        {
            Console.WriteLine("warning: line={0} column={1} project=\"{2}\" file=\"{3}\" => {4}", e.LineNumber, e.ColumnNumber, e.ProjectFile, e.File, e.Message);
        }

        private void eventSource_ErrorRaised(object sender, BuildErrorEventArgs e)
        {
            Console.WriteLine("error: line={0} column={1} project=\"{2}\" file=\"{3}\" => {4}", e.LineNumber, e.ColumnNumber, e.ProjectFile, e.File, e.Message);
        }

        private void eventSource_MessageRaised(object sender, BuildMessageEventArgs e)
        {
            if ((e.Importance == MessageImportance.High && IsVerbosityAtLeast(LoggerVerbosity.Minimal))
                || (e.Importance == MessageImportance.Normal && IsVerbosityAtLeast(LoggerVerbosity.Normal))
                || (e.Importance == MessageImportance.Low && IsVerbosityAtLeast(LoggerVerbosity.Detailed))              
            )
            {
                Console.WriteLine(e.Message);
            }
        }

        public string Parameters
        {
            get;
            set;
        }

        public void Shutdown()
        {
            _eventSource.ErrorRaised -= eventSource_ErrorRaised;
            _eventSource.MessageRaised -= eventSource_MessageRaised;
            _eventSource.WarningRaised -= eventSource_WarningRaised;
        }

        public LoggerVerbosity Verbosity
        {
            get;
            set;
        }

        private bool IsVerbosityAtLeast(LoggerVerbosity verbosity)
        {
            return (int)verbosity <= (int)Verbosity;
        }
    }
}
