using System;
using System.Linq;
using FastColoredTextBoxNS;
using LiteDevelop.Framework;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Essentials.CodeEditor.Syntax;
using LiteDevelop.Essentials.CodeEditor.Syntax.Net;
using LiteDevelop.Essentials.CodeEditor.Syntax.Web;

namespace LiteDevelop.Essentials.CodeEditor
{
    public abstract class EditorLayoutData
    {
        public static EventBasedCollection<EditorLayoutData> RegisteredLayouts = new EventBasedCollection<EditorLayoutData>()
        {
            new CSharpLayoutData(),
            new VisualBasicLayoutData(),

            new HtmlLayoutData(),
            new PhpLayoutData(),
            new CssLayoutData(),
            new XmlLayoutData(),
        };

        public static T GetLayout<T>() where T : EditorLayoutData
        {
            return RegisteredLayouts.FirstOrDefault(x => x is T) as T;
        }

        public static EditorLayoutData GetLayout(LanguageDescriptor language)
        {
            return RegisteredLayouts.FirstOrDefault(x => x.Language == language);
        }

        public abstract LanguageDescriptor Language { get; }
        public abstract SyntaxDescriptor TextBoxSyntaxDescriptor { get; }
        public abstract Language TextBoxLanguage { get; }

        public abstract AutoCompletionMap CreateAutoCompletionMap(AutocompleteMenu menu);
    }
}
