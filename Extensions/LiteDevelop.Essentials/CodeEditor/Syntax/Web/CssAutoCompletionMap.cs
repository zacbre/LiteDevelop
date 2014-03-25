using System;
using System.Collections.Generic;
using System.Linq;
using FastColoredTextBoxNS;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Framework.Languages.Web;

namespace LiteDevelop.Essentials.CodeEditor.Syntax.Web
{
    public class CssAutoCompletionMap : WebAutoCompletionMap
    {
        private static CssLanguage _language = LanguageDescriptor.GetLanguage<CssLanguage>();

        public CssAutoCompletionMap(AutocompleteMenu menu)
            : base(menu)
        {
        }

        public override IEnumerator<AutocompleteItem> GetEnumerator()
        {
            foreach (var keyWord in Language.Keywords)
                yield return new AutocompleteItem(keyWord);
        }

        public override LanguageDescriptor Language
        {
            get { return _language; }
        }
    }
}
