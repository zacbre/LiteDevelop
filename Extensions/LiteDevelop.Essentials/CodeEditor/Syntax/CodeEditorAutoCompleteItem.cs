using System;
using System.Linq;
using FastColoredTextBoxNS;

namespace LiteDevelop.Essentials.CodeEditor.Syntax
{
    public class CodeEditorAutoCompleteItem : AutocompleteItem
    {
        public CodeEditorAutoCompleteItem()
        {
        }

        public CodeEditorAutoCompleteItem(string text)
            : base(text)
        {
        }

        public CodeEditorAutoCompleteItem(string text, int imageIndex)
            : base(text, imageIndex)
        {
        }

        public bool SurpressSpaceBar
        {
            get;
            set;
        }

        public override CompareResult Compare(string fragmentText)
        {
            if ((MenuText ?? Text).StartsWith(fragmentText, StringComparison.InvariantCultureIgnoreCase))
                return CompareResult.VisibleAndSelected;
            return CompareResult.Hidden;
        }
    }

    public class CodeEditorMemberAutoCompleteItem : CodeEditorAutoCompleteItem
    {
        private string[] _separators;
        private string _parentMember;
        private string _currentSeparator;
        private string _lowerCaseText;

        public CodeEditorMemberAutoCompleteItem(string text, string[] separators)
            : base(text)
        {
            _separators = separators;
            _lowerCaseText = text.ToLower();
        }

        public override CompareResult Compare(string fragmentText)
        {
            int separatorIndex = GetSeparatorIndex(fragmentText, out _currentSeparator);
            if (separatorIndex == -1)
                return CompareResult.Hidden;

            _parentMember = fragmentText.Substring(0, separatorIndex);
            string member = fragmentText.Substring(separatorIndex + 1);

            if (string.IsNullOrEmpty(member))
                return CompareResult.Visible;

            if ((MenuText ?? Text).StartsWith(member, StringComparison.InvariantCultureIgnoreCase))
                return CompareResult.VisibleAndSelected;

            if (_lowerCaseText.Contains(member.ToLower()))
                return CompareResult.Visible;

            return CompareResult.Hidden;
        }

        public override string GetTextForReplace()
        {
            return _parentMember + _currentSeparator + Text;
        }

        private int GetSeparatorIndex(string fragment, out string separator)
        {
            separator = null;
            for (int i = 0; i < _separators.Length; i++)
            {
                int index = fragment.LastIndexOf(_separators[i]);
                if (index != -1)
                {
                    separator = _separators[i];
                    return index;
                }
            }
            return -1;
        }
    }

    public class CodeEditorMethodAutoCompleteItem : CodeEditorMemberAutoCompleteItem
    {
        public CodeEditorMethodAutoCompleteItem(string text, string[] separators)
            : base(text, separators)
        {
            AppendParantheses = true;
            SurpressSpaceBar = true;
            Parantheses = "(^)";
        }

        public bool AppendParantheses
        {
            get;
            set;
        }

        public string Parantheses
        {
            get;
            set;
        }

        public override void OnSelected(AutocompleteMenu popupMenu, SelectedEventArgs e)
        {
            e.Tb.BeginUpdate();
            e.Tb.Selection.BeginUpdate();

            base.OnSelected(popupMenu, e);

            if (AppendParantheses)
            {
                var fragmentStart = e.Tb.Selection.Start;
                e.Tb.InsertText(Parantheses);

                if (Parantheses.Contains('^'))
                {
                    e.Tb.Selection.Start = fragmentStart;

                    while (e.Tb.Selection.CharBeforeStart != '^')
                    {
                        if (!e.Tb.Selection.GoRightThroughFolded())
                            break;
                    }

                    e.Tb.Selection.GoLeft(true);
                    e.Tb.InsertText("");
                }
            }

            e.Tb.Selection.EndUpdate();
            e.Tb.EndUpdate();

        }
    }

    public class CodeEditorSnippetAutoCompleteItem : CodeEditorAutoCompleteItem
	{
		public CodeEditorSnippetAutoCompleteItem(string title, string snippet)
			: base(snippet)
		{
            MenuText = title;
		}
		
		public override CompareResult Compare(string fragmentText)
        {
            if ((MenuText ?? Text).StartsWith(fragmentText, StringComparison.InvariantCultureIgnoreCase))
                return CompareResult.VisibleAndSelected;
            return CompareResult.Hidden;
        }

        public override void OnSelected(AutocompleteMenu popupMenu, SelectedEventArgs e)
        {
            e.Tb.BeginUpdate();
            e.Tb.Selection.BeginUpdate();

            var fragmentStart = popupMenu.Fragment.Start;
            var selectionStart = e.Tb.Selection.Start;

            if (e.Tb.AutoIndent)
            {
                for (int iLine = fragmentStart.iLine + 1; iLine <= selectionStart.iLine; iLine++)
                {
                    e.Tb.Selection.Start = new Place(0, iLine);
                    e.Tb.DoAutoIndent(iLine);
                }
            }

            if (Text.Contains('^'))
            {
                e.Tb.Selection.Start = fragmentStart;

                while (e.Tb.Selection.CharBeforeStart != '^')
                {
                    if (!e.Tb.Selection.GoRightThroughFolded())
                        break;
                }

                e.Tb.Selection.GoLeft(true);
                e.Tb.InsertText("");
            }

            e.Tb.Selection.EndUpdate();
            e.Tb.EndUpdate();
        }

	}
}