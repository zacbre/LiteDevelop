using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteDevelop.Framework.Languages.Net;
using LiteDevelop.Framework.Languages;
using System.Reflection;

namespace LiteDevelop.Essentials.CodeEditor.Syntax.Net
{
    public class VisualBasicAutoCompletionMap : NetAutoCompletionMap 
    {
        private static string[] _separators = new string[] { "." };
        private static VisualBasicLanguage _language = LanguageDescriptor.GetLanguage<VisualBasicLanguage>();
        private static Dictionary<string, string> _blockIdentifiers = new Dictionary<string, string>()
        {
            {"{", "}"},
            {"[", "]"},
            {"(", ")"},
            {"\"", "\""},
        };

        public VisualBasicAutoCompletionMap(FastColoredTextBoxNS.AutocompleteMenu menu)
            : base(menu, _separators)
        {
        }

        public override void UpdateSnapshot(string source)
        {
            this.CurrentSnapshot = _language.CreateSourceSnapshot(source);
        }

        public override string ThisKeyword
        {
            get { return "Me"; }
        }

        public override string BaseKeyword
        {
            get { return "MyBase"; }
        }

        public override LanguageDescriptor Language
        {
            get { return _language; }
        }

        public override IDictionary<string, string> BlockIdentifiers
        {
            get { return _blockIdentifiers; }
        }

        public override IEnumerator<FastColoredTextBoxNS.AutocompleteItem> GetEnumerator()
        {
            var previousFragment = Globals.GetPreviousFragment(AutoCompleteMenu.Fragment, this.SearchPattern);
            if (previousFragment == null)
            {
                var enumerator = base.GetEnumerator();
                while (enumerator.MoveNext())
                    yield return enumerator.Current;
            }
            else
            {
                if (!IsMemberIdentifier(previousFragment.Text))
                {
                    var enumerator = base.GetEnumerator();
                    while (enumerator.MoveNext())
                        yield return enumerator.Current;
                }
                else
                {
                    if (IsModifier(previousFragment.Text))
                    {
                        foreach (var modifier in _language.Modifiers)
                            yield return new CodeEditorAutoCompleteItem(modifier, IconProvider.GetImageIndex(modifier));

                        foreach (var word in _language.MemberIdentifiers)
                            yield return new CodeEditorAutoCompleteItem(word, IconProvider.GetImageIndex(word));
                    }

                    if (StringsAreEqual(previousFragment.Text, "Sub"))
                        yield return new CodeEditorAutoCompleteItem("New ", IconProvider.GetImageIndex("Sub"));
                }
                
            }
        }

        private bool IsMemberIdentifier(string word)
        {
            return IsModifier(word) || _language.MemberIdentifiers.FirstOrDefault(x => StringsAreEqual(word, x)) != null;
        }

        private bool IsModifier(string word)
        {
            return _language.Modifiers.FirstOrDefault(x => StringsAreEqual(word, x)) != null;
        }


    }
}
