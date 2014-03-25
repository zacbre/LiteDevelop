using System;
using System.Linq;

namespace LiteDevelop.Framework.Languages.Web
{
    public class XmlLanguage : WebLanguageDescriptor
    {
        /// <inheritdoc />
        public override string Name
        {
            get { return "Xml"; }
        }

        /// <inheritdoc />
        public override string[] FileExtensions
        {
            get { return new string[] { ".xml", ".xaml", ".csproj", ".vbproj", ".liteproj" }; }
        }

        /// <inheritdoc />
        public override string StandardFileExtension
        {
            get { return ".xml"; }
        }

        /// <inheritdoc />
        public override string[] Keywords
        {
            get { return new string[0]; }
        }

        /// <inheritdoc />
        public override bool CaseSensitive
        {
            get { return true; }
        }

        /// <inheritdoc />
        public override string CommentPrefix
        {
            get { return "<!--"; }
        }

        /// <inheritdoc />
        public override string CommentSuffix
        {
            get { return "-->"; }
        }

        /// <inheritdoc />
        public override Snippet[] Snippets
        {
            get { return new Snippet[0]; }
        }

        /// <inheritdoc />
        public override bool CanCreateSnapshot
        {
            get { return false; }
        }

        /// <inheritdoc />
        public override SourceSnapshot CreateSourceSnapshot(string source)
        {
            throw new NotSupportedException();
        }
    }
}
