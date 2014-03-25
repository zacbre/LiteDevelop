using System;
using System.Collections.Generic;
using System.Linq;
using FastColoredTextBoxNS;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Framework.Languages.Web;

namespace LiteDevelop.Essentials.CodeEditor.Syntax.Web
{
    public class PhpAutoCompletionMap : WebAutoCompletionMap
    {
        private static PhpLanguage _language = LanguageDescriptor.GetLanguage<PhpLanguage>();

        public PhpAutoCompletionMap(AutocompleteMenu menu)
            : base(menu)
        {
        }

        public override IEnumerator<AutocompleteItem> GetEnumerator()
        {
            foreach (var keyWord in Language.Keywords)
                yield return new CodeEditorAutoCompleteItem(keyWord);
        }

        public override LanguageDescriptor Language
        {
            get { return _language; }
        }
    }
}
