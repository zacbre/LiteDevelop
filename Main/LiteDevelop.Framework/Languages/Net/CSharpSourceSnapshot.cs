using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using LiteDevelop.Framework.FileSystem;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Framework.Languages.Net;

namespace LiteDevelop.Framework.Languages.Net
{
    // TODO: 
    // - filter out getters and setters with modifiers from field list.
    //

    public class CSharpSourceSnapshot : NetSourceSnapshot
    {
        private static Regex _usingRegex = new Regex(@"using\s+[^\r\n;]+");
        private static Regex _namespaceRegex = new Regex(@"namespace\s+[^\r\n;]+");
        private static Regex _membersRegex = new Regex(string.Format(@"\b(?<modifiers>(({0})\s+)+)((?<explicitMemberType>({1}))?|(?<returnType>[\w\.]+))\s+(?<name>\w+)\s*(:\s*(?<baseType>[\w\.<>]+))?(?<methodParanthese>\()?(?<propertyBrace>\{{)?",
                RegexUtilities.BuildRegexAlternativeRange(LanguageDescriptor.GetLanguage<CSharpLanguage>().Modifiers), "class|struct|interface|enum"));
        private static Regex _baseTypeRegex = new Regex(@"\w+\s*:\s*(?<baseType>[^\s]+)");

        private static CSharpLanguage _language = LanguageDescriptor.GetLanguage<CSharpLanguage>();

        public CSharpSourceSnapshot(string source)
            : base(source)
        {
            SearchUsings(source);
            SearchMembers(source);
        }

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

        private void SearchUsings(string source)
        {
            var namespaces = new List<NetSnapshotMember>();

            foreach (Match match in _usingRegex.Matches(source))
            {
                namespaces.Add(new NetSnapshotMember(match.Value.Split(' ')[1], new string[] { "using" }, string.Empty, match.Index, match.Index + match.Length));
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
            var events = new List<NetSnapshotMember>();

            foreach (Match match in _namespaceRegex.Matches(source))
            {
                namespaces.Add(new NetSnapshotMember(match.Value.Split(' ')[1], new string[] { "namespace" }, string.Empty, match.Index, match.Index + match.Length));
            }

            foreach (Match match in _membersRegex.Matches(source))
            {
                string modifiers = match.Groups["modifiers"].Value.Trim();
                string explicitMemberType = match.Groups["explicitMemberType"].Value.Trim();
                string returnType = match.Groups["returnType"].Value.Trim();
                string baseType = match.Groups["baseType"].Value.Trim();
                string name = match.Groups["name"].Value.Trim();
                bool isMethod = !string.IsNullOrEmpty(match.Groups["methodParanthese"].Value);
                bool isProperty = !string.IsNullOrEmpty(match.Groups["propertyBrace"].Value);

                var member = new NetSnapshotMember(name, GetModifiers(modifiers), returnType, match.Index, match.Index + match.Length);

                if (explicitMemberType == "event")
                {
                    events.Add(member);
                }
                else if (!string.IsNullOrEmpty(explicitMemberType))
                {
                    member.ValueType = baseType;
                    types.Add(member);
                }
                else if (isMethod)
                {
                    methods.Add(member);
                }
                else if (isProperty)
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
            this.Events = events.ToArray();
        }

        private string[] GetModifiers(string input)
        {
            MatchCollection matches = Regex.Matches(input, RegexUtilities.BuildRegexAlternativeRange(LanguageDescriptor.GetLanguage<CSharpLanguage>().Modifiers));
            string[] output = new string[matches.Count];

            for (int i = 0; i < matches.Count; i++)
            {
                output[i] = matches[i].Value;
            }

            return output;
        }

        private string GetReturnType(string input, string name)
        {
            Match returnTypeAndNameMatch = Regex.Match(input, @"\S+\s+" + name);
            if (returnTypeAndNameMatch.Success)
            {
                return returnTypeAndNameMatch.Value.Split(' ')[0];
            }
            return "System.Object";
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
