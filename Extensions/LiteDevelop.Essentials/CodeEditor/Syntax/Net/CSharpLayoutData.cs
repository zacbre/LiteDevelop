using System;
using System.Linq;
using FastColoredTextBoxNS;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Framework.Languages.Net;

namespace LiteDevelop.Essentials.CodeEditor.Syntax.Net
{
    public class CSharpLayoutData : EditorLayoutData
    {

        public override LanguageDescriptor Language
        {
            get { return LanguageDescriptor.GetLanguage<CSharpLanguage>(); }
        }

        public override SyntaxDescriptor TextBoxSyntaxDescriptor
        {
            get { return null; }
        }

        public override Language TextBoxLanguage
        {
            get { return FastColoredTextBoxNS.Language.CSharp; }
        }

        public override AutoCompletionMap CreateAutoCompletionMap(AutocompleteMenu menu)
        {
            return new CSharpAutoCompletionMap(menu);
        }

    }
}
