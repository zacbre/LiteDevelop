using System;
using System.Linq;

namespace LiteDevelop.Framework.Languages
{
    public class PlainTextLanguage : LanguageDescriptor
    {
        /// <inheritdoc />
        public override string Name
        {
            get { return "Plain Text"; }
        }

        /// <inheritdoc />
        public override string LanguageOrder
        {
            get { return "Other"; }
        }

        /// <inheritdoc />
        public override string[] FileExtensions
        {
            get { return new string[0]; }
        }

        /// <inheritdoc />
        public override string StandardFileExtension
        {
            get { return ".txt"; }
        }

        /// <inheritdoc />
        public override string[] Keywords
        {
            get { return null; }
        }

        /// <inheritdoc />
        public override bool CaseSensitive
        {
            get { return false; }
        }

        /// <inheritdoc />
        public override string CommentPrefix
        {
            get { return string.Empty; }
        }

        /// <inheritdoc />
        public override string CommentSuffix
        {
            get { return string.Empty; }
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
            throw new NotImplementedException();
        }
    }
}
