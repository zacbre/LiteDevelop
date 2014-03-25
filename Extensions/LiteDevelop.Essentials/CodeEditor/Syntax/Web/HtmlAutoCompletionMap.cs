using System;
using System.Collections.Generic;
using System.Linq;
using FastColoredTextBoxNS;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Framework.Languages.Web;

namespace LiteDevelop.Essentials.CodeEditor.Syntax.Web
{
    public class HtmlAutoCompletionMap : WebAutoCompletionMap
    {
        private static LanguageDescriptor _language = LanguageDescriptor.GetLanguage<HtmlLanguage>();

        public HtmlAutoCompletionMap(AutocompleteMenu menu)
            : base(menu)
        {
        }

        public override IEnumerator<AutocompleteItem> GetEnumerator()
        {
            foreach (var keyword in Language.Keywords)
            {
                yield return new CodeEditorSnippetAutoCompleteItem(keyword, string.Format("<{0}>^</{0}>", keyword))
                    {
                        SurpressSpaceBar = true,
                    };
            }
        }

        public override string SearchPattern
        {
            get { return @"[<\w]"; }
        }

        public override LanguageDescriptor Language
        {
            get { return _language; }
        }
    }
}
