using System;
using System.Linq;
using FastColoredTextBoxNS;

namespace LiteDevelop.Essentials.CodeEditor.Syntax.Web
{
    public abstract class WebAutoCompletionMap : AutoCompletionMap
    {
        public WebAutoCompletionMap(AutocompleteMenu menu)
            : base(menu)
        {
        }

        public override string SearchPattern
        {
            get { return @"[\w\.]"; }
        }
    }
}
