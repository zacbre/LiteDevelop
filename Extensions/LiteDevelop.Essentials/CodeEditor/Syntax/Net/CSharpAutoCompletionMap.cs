using System;
using System.Collections.Generic;
using System.Linq;
using FastColoredTextBoxNS;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Framework.Languages.Net;


namespace LiteDevelop.Essentials.CodeEditor.Syntax.Net
{
    /// <summary>
    /// An auto-completion map specific for C# source codes.
    /// </summary>
    public class CSharpAutoCompletionMap : NetAutoCompletionMap 
    {
        private static string[] _separators = new string[] { ".", "->" };
        private static CSharpLanguage _language = LanguageDescriptor.GetLanguage<CSharpLanguage>();
        private static Dictionary<string, string> _blockIdentifiers = new Dictionary<string, string>()
        {
            {"{", "}"},
            {"[", "]"},
            {"(", ")"},
            {"\"", "\""},
            {"'", "'"},
        };

        public CSharpAutoCompletionMap(AutocompleteMenu menu)
            : base(menu, _separators)
        {
        }

        /// <summary>
        /// Creates and updates the snapshot with the specific C# source code.
        /// </summary>
        /// <param name="source"></param>
        public override void UpdateSnapshot(string source)
        {
            this.CurrentSnapshot = _language.CreateSourceSnapshot(source) as NetSourceSnapshot;
        }

        public override string ThisKeyword
        {
            get { return "this"; }
        }

        public override string BaseKeyword
        {
            get { return "base"; }
        }

        public override LanguageDescriptor Language
        {
            get { return _language; }
        }

        public override IDictionary<string, string> BlockIdentifiers
        {
            get { return _blockIdentifiers; }
        }

        public override IEnumerator<AutocompleteItem> GetEnumerator()
        {
            var previousFragment = Globals.GetPreviousFragment(AutoCompleteMenu.Fragment, SearchPattern);
            if (previousFragment == null || !IsMemberIdentifier(previousFragment.Text))
            {
                var enumerator = base.GetEnumerator();
                while (enumerator.MoveNext())
                    yield return enumerator.Current;
            }
        }

        private bool IsMemberIdentifier(string word)
        {
            return _language.MemberIdentifiers.Contains(word) || (CurrentSnapshot as NetSourceSnapshot).GetTypeByName(word) != null;
        }
    }
}
