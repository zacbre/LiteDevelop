using System;
using System.Collections.Generic;
using System.Linq;

namespace LiteDevelop.Essentials.CodeEditor.Syntax
{
    public interface ICodeBlockCompleter
    {
        IDictionary<string, string> BlockIdentifiers { get; }
    }
}
