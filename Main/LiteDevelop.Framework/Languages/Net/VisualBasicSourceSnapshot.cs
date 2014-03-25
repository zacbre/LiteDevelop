using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LiteDevelop.Framework.Languages.Net
{
    public class VisualBasicSourceSnapshot : NetSourceSnapshot
    {
        private static readonly Regex _importsRegex = new Regex(@"Imports\s+[^\r\n]+", RegexOptions.IgnoreCase);
        private static readonly Regex _namespaceRegex = new Regex(@"Namespace\s+[^\r\n]+", RegexOptions.IgnoreCase);
        private static readonly Regex _membersRegex = new Regex(string.Format(@"(?<modifiers>(({0})\s+)+)(?<memberType>(\s+|{1}))?\s*(?<name>\w+)(\s+As\s+(New\s+)?(?<returnType>[\w\.]+))?[^\r\n]*", 
                RegexUtilities.BuildRegexAlternativeRange(LanguageDescriptor.GetLanguage<VisualBasicLanguage>().Modifiers), "Class|Property|Function|Sub|Module"));
        private static readonly Regex _baseTypeRegex = new Regex(@"Inherits\s+(?<baseType>[^\s]+)");

        private static VisualBasicLanguage _language = LanguageDescriptor.GetLanguage<VisualBasicLanguage>();

        /// <inheritdoc />
        public override NetSnapshotMember[] UsingNamespaces { get; protected set; }

        /// <inheritdoc />
        public override NetSnapshotMember[] Namespaces { get; protected set; }

        /// <inheritdoc />
        public override NetSnapshotMember[] Types { get; protected set; }

        /// <inheritdoc />
        public override NetSnapshotMember[] Methods { get; protected set; }

        /// <inheritdoc />
        public override NetSnapshotMember[] Fields { get; protected set; }

        /// <inheritdoc />
        public override NetSnapshotMember[] Properties { get; protected set; }

        /// <inheritdoc />
        public override NetSnapshotMember[] Events { get; protected set; } // TODO

        /// <inheritdoc />
        public override LanguageDescriptor Language
        {
            get { return _language; }
        }

        public VisualBasicSourceSnapshot(string source)
            : base(source)
        {
            SearchImports(source);
            SearchMembers(source);
        }

        private void SearchImports(string source)
        {
            var namespaces = new List<NetSnapshotMember>();

            foreach (Match match in _importsRegex.Matches(source))
            {
                namespaces.Add(new NetSnapshotMember(match.Value.Split(' ')[1], new string[] { "Imports" }, string.Empty, match.Index, match.Index + match.Length));
            }

            namespaces.Sort(new SnapshotMemberNameComparer());
            UsingNamespaces = namespaces.ToArray();
        }

        private void SearchMembers(string source)
        {
            var namespaces = new List<NetSnapshotMember>();
            var types = new List<NetSnapshotMember>();
            var methods = new List<NetSnapshotMember>();
            var fields = new List<NetSnapshotMember>();
            var properties = new List<NetSnapshotMember>();

            foreach (Match match in _namespaceRegex.Matches(source))
            {
                namespaces.Add(new NetSnapshotMember(match.Value.Split(' ')[1], new string[] { "Namespace" }, string.Empty, match.Index, match.Index + match.Length));
            }

            foreach (Match match in _membersRegex.Matches(source))
            {
                string modifiers = match.Groups["modifiers"].Value.Trim();
                string memberType = match.Groups["memberType"].Value.Trim();
                string returnType = match.Groups["returnType"].Value.Trim();
                string name = match.Groups["name"].Value.Trim();

                if (string.Compare(memberType, "function", true) == 0)
                {
                    string line = match.Value;
                    Regex secondMatch = new Regex(@"As\s+(?<type>[^\s]+)", RegexOptions.IgnoreCase);
                    var matches = secondMatch.Matches(line);
                    if (matches.Count > 0)
                    {
                        returnType = matches[matches.Count - 1].Groups["type"].Value.Trim();
                    }
                }
                else if (string.Compare(memberType, "sub", true) == 0)
                {
                    returnType = typeof(void).FullName;
                }

                if (string.IsNullOrEmpty(returnType))
                {
                    returnType = typeof(object).FullName;
                }

                var member = new NetSnapshotMember(name, GetModifiers(modifiers), returnType, match.Index, match.Index + match.Length);
                
                if (string.Compare(memberType, "class", true) == 0 ||
                    string.Compare(memberType, "interface", true) == 0 ||
                    string.Compare(memberType, "structure", true) == 0 ||
                    string.Compare(memberType, "enum", true) == 0)
                {
                    var baseTypeMatch = _baseTypeRegex.Match(source, match.Index);
                    if (baseTypeMatch.Success)
                        member.ValueType = baseTypeMatch.Groups["baseType"].Value;
                    else
                        member.ValueType = string.Empty;

                    types.Add(member);
                }
                else if (string.Compare(memberType, "function", true) == 0 || string.Compare(memberType, "sub", true) == 0)
                {
                    methods.Add(member);
                }
                else if (string.Compare(memberType, "property", true) == 0)
                {
                    properties.Add(member);
                }
                else
                {
                    fields.Add(member);
                }
            }

            this.Namespaces = namespaces.ToArray();
            this.Types = types.ToArray();
            this.Fields = fields.ToArray();
            this.Methods = methods.ToArray();
            this.Properties = properties.ToArray();
            this.Events = new NetSnapshotMember[0];
        }

        private string BuildRegexAlternativeRange(string[] input)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append("(");
            for (int i = 0; i < input.Length; i++)
                builder.Append(string.Format("{0}{1}", input[i], i == input.Length - 1 ? string.Empty : "|"));
            builder.Append(")");

            return builder.ToString();
        }

        private string[] GetModifiers(string input)
        {
            MatchCollection matches = Regex.Matches(input, BuildRegexAlternativeRange(LanguageDescriptor.GetLanguage<VisualBasicLanguage>().Modifiers));
            string[] output = new string[matches.Count];

            for (int i = 0; i < matches.Count; i++)
            {
                output[i] = matches[i].Value;
            }

            return output;
        }

        /// <inheritdoc />
        public override SnapshotMember GetMemberByName(string name)
        {
            NetSnapshotMember member;
            if ((member = Methods.FirstOrDefault(x => x.Name == name)) != null)
                return member;
            if ((member = Fields.FirstOrDefault(x => x.Name == name)) != null)
                return member;
            if ((member = Properties.FirstOrDefault(x => x.Name == name)) != null)
                return member;
            if ((member = Types.FirstOrDefault(x => x.Name == name)) != null)
                return member;
            return null;
        }
    }
}
