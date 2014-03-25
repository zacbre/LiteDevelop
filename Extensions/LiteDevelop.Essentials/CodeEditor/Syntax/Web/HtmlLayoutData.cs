using System;
using System.Linq;
using FastColoredTextBoxNS;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Framework.Languages.Web;

namespace LiteDevelop.Essentials.CodeEditor.Syntax.Web
{
    public class HtmlLayoutData : EditorLayoutData
    {
        public override LanguageDescriptor Language
        {
            get { return LanguageDescriptor.GetLanguage<HtmlLanguage>(); }
        }

        public override SyntaxDescriptor TextBoxSyntaxDescriptor
        {
            get { return null; }
        }

        public override Language TextBoxLanguage
        {
            get { return FastColoredTextBoxNS.Language.HTML; }
        }

        public override AutoCompletionMap CreateAutoCompletionMap(AutocompleteMenu menu)
        {
            return new HtmlAutoCompletionMap(menu);
        }
    }
}
