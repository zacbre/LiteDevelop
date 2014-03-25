using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteDevelop.Framework
{
    /// <summary>
    /// Represents a strongly typed collection of progress reporters which can be accessed all in once.
    /// </summary>
    public class ProgressReporterCollection<TProgressReporter> : EventBasedCollection<TProgressReporter>, IProgressReporter 
        where TProgressReporter: IProgressReporter
    {
        #region IProgressReporter Members

        public void Report(MessageSeverity severity, string message)
        {
            foreach (var item in this)
                item.Report(severity, message);
        }

        public int ProgressPercentage
        {
            get
            {
                return this[0].ProgressPercentage;
            }
            set
            {
                foreach (var item in this)
                    item.ProgressPercentage = value;
            }
        }

        public bool ProgressVisible
        {
            get
            {
                return this[0].ProgressVisible;
            }
            set
            {
                foreach (var item in this)
                    item.ProgressVisible = value;
            }
        }

        #endregion
    }

    /// <summary>
    /// Represents a collection of progress reporters which can be accessed all in once.
    /// </summary>
    public class ProgressReporterCollection : ProgressReporterCollection<IProgressReporter>
    {

    }

    public static class ProgressReporterCollectionExtensions
    {

        public static INamedProgressReporter GetReporterById(this IEnumerable<INamedProgressReporter> reporters, string id)
        {
            return reporters.FirstOrDefault(x => x.Identifier == id);
        }
    }

    
}
