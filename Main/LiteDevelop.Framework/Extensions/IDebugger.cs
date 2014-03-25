using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using LiteDevelop.Framework.Debugging;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Framework.Extensions
{
    public interface IDebugger
    {
        bool CanDebugProject(Project project);

        DebuggerSession CreateSession();
    }
}
