using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.VisualBasic;

namespace LiteDevelop.Framework.Languages.Net
{
    public class VisualBasicLanguage : NetLanguageDescriptor
    {
        private readonly VBCodeProvider _codeProvider;
        private readonly NetLanguageData _data;
        private readonly string[] _keywords;

        public VisualBasicLanguage()
        {
            _codeProvider = new VBCodeProvider();

            using (var fileStream = File.Open(Path.Combine(Application.StartupPath, "Languages", "Net", "VisualBasic.xml"), FileMode.Open, FileAccess.Read))
            {
                var serializer = new XmlSerializer(typeof(NetLanguageData));
                _data = (NetLanguageData)serializer.Deserialize(fileStream);
            }
            _keywords = _data.Keywords.MergeWith(_data.Modifiers).MergeWith(_data.MemberIdentifiers);
        }

        /// <inheritdoc />
        public override string Name
        {
            get { return "Visual Basic"; }
        }
        
        /// <inheritdoc />
        public override string[] FileExtensions
        {
            get { return new string[] { ".vb" }; }
        }

        /// <inheritdoc />
        public override string StandardFileExtension
        {
            get { return ".vb"; }
        }

        /// <inheritdoc />
        public override string[] Keywords
        {
            get { return _keywords; }
        }

        /// <inheritdoc />
        public override string[] Modifiers
        {
            get { return _data.Modifiers; }
        }

        /// <inheritdoc />
        public override string[] MemberIdentifiers
        {
            get { return _data.MemberIdentifiers; }
        }

        /// <inheritdoc />
        public override string CommentPrefix
        {
            get { return "'"; }
        }

        /// <inheritdoc />
        public override string CommentSuffix
        {
            get { return string.Empty; }
        }

        /// <inheritdoc />
        public override bool CaseSensitive
        {
            get { return false; }
        }

        /// <inheritdoc />
        public override Snippet[] Snippets
        {
            get { return _data.Snippets; }
        }

        /// <inheritdoc />
        public override bool CanCreateSnapshot
        {
            get { return true; }
        }

        /// <inheritdoc />
        public override NetTypeAlias[] TypeAliases
        {
            get { return _data.TypeAliases; }
        }

        /// <inheritdoc />
        public override CodeDomProvider CodeProvider
        {
            get { return _codeProvider; }
        }

        /// <inheritdoc />
        public override Languages.SourceSnapshot CreateSourceSnapshot(string source)
        {
            return new VisualBasicSourceSnapshot(source);
        }
    }
}
