using System;
using System.CodeDom.Compiler;
using System.Linq;

namespace LiteDevelop.Framework.Languages.Net
{
    /// <summary>
    /// Provides information about a .NET language.
    /// </summary>
    public abstract class NetLanguageDescriptor : LanguageDescriptor
    {
        /// <inheritdoc />
        public override string LanguageOrder
        {
            get { return ".NET"; }
        }

        /// <summary>
        /// Gets the modifiers of this .NET language.
        /// </summary>
        public abstract string[] Modifiers { get; }

        /// <summary>
        /// Gets keywords that indicate a member declaration.
        /// </summary>
        public abstract string[] MemberIdentifiers { get; }

        /// <summary>
        /// Gets the type aliases of this .NET language
        /// </summary>
        public abstract NetTypeAlias[] TypeAliases { get; }

        /// <summary>
        /// Gets the CodeDom provider associated with this .NET language.
        /// </summary>
        public abstract CodeDomProvider CodeProvider { get; }

    }
}
