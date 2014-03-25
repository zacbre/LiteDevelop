using System;
using System.Linq;
using FastColoredTextBoxNS;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Framework.Languages.Net;

namespace LiteDevelop.Essentials.CodeEditor.Syntax.Net
{
    public class VisualBasicLayoutData : EditorLayoutData
    {
        public override LanguageDescriptor Language
        {
            get { return LanguageDescriptor.GetLanguage<VisualBasicLanguage>(); }
        }

        public override SyntaxDescriptor TextBoxSyntaxDescriptor
        {
            get { return null; }
        }

        public override Language TextBoxLanguage
        {
            get { return FastColoredTextBoxNS.Language.VB; }
        }

        public override AutoCompletionMap CreateAutoCompletionMap(AutocompleteMenu menu)
        {
            return new VisualBasicAutoCompletionMap(menu);
        }
    }
}
