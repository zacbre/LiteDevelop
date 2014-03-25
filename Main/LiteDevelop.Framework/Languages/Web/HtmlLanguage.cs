using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace LiteDevelop.Framework.Languages.Web
{
    public class HtmlLanguage : WebLanguageDescriptor 
    {
        private readonly WebLanguageData _data;

        public HtmlLanguage()
        {
            using (var fileStream = File.Open(Path.Combine(Application.StartupPath, "Languages", "Web", "Html.xml"), FileMode.Open, FileAccess.Read))
            {
                var serializer = new XmlSerializer(typeof(WebLanguageData));
                _data = (WebLanguageData)serializer.Deserialize(fileStream);
            }
        }

        /// <inheritdoc />
        public override string Name
        {
            get { return "Html"; }
        }

        /// <inheritdoc />
        public override string[] FileExtensions
        {
            get { return new string[] { ".htm", ".html", ".shtml" }; }
        }

        /// <inheritdoc />
        public override string StandardFileExtension
        {
            get { return ".html"; }
        }

        /// <inheritdoc />
        public override string[] Keywords
        {
            get { return _data.Keywords; }
        }

        /// <inheritdoc />
        public override bool CaseSensitive
        {
            get { return false; }
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
            get { return _data.Snippets; }
        }

        /// <inheritdoc />
        public override bool CanCreateSnapshot
        {
            get { return false; }
        }

        /// <inheritdoc />
        public override Languages.SourceSnapshot CreateSourceSnapshot(string source)
        {
            throw new NotSupportedException();
        }

    }
}
