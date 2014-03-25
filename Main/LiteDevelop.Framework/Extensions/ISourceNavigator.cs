using System;
using System.Linq;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Framework.Extensions
{
    /// <summary>
    /// Provides members for navigating to specific source locations
    /// </summary>
    public interface ISourceNavigator
    {
        /// <summary>
        /// Requests to navigate to a specific source location.
        /// </summary>
        /// <param name="location">The location to navigate to.</param>
        void NavigateToLocation(SourceLocation location); 
    }
}
