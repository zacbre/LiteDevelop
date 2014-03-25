using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LiteDevelop.Framework.FileSystem;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Framework.Languages.Net;
using LiteDevelop.Framework.Languages.Web;

namespace LiteDevelop.Framework.Languages
{
    /// <summary>
    /// Provides information about a specific language.
    /// </summary>
    public abstract class LanguageDescriptor
    {
        static LanguageDescriptor()
        {
            RegisteredLanguages = new EventBasedCollection<LanguageDescriptor>()
            {
                new PlainTextLanguage(),

                // .NET
                new CSharpLanguage(),
                new VisualBasicLanguage(),

                // Web markup
                new HtmlLanguage(),
                new CssLanguage(),
                new PhpLanguage(),
                new XmlLanguage(),
            };
        }

        /// <summary>
        /// Gets a list of registered language descriptors
        /// </summary>
        public static EventBasedCollection<LanguageDescriptor> RegisteredLanguages
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the instance of a specific language descriptor by specifying a file.
        /// </summary>
        /// <param name="path">The file to get the language from.</param>
        /// <returns>A language descriptor determined by the file's extension, or null if none can be found.</returns>
        public static LanguageDescriptor GetLanguageByPath(FilePath path)
        {
            return GetLanguageByPath(path.FullPath);
        }

        /// <summary>
        /// Gets the instance of a specific language descriptor by specifying a file.
        /// </summary>
        /// <param name="path">The file to get the language from.</param>
        /// <returns>A language descriptor determined by the file's extension, or null if none can be found.</returns>
        public static LanguageDescriptor GetLanguageByPath(string path)
        {
            string extension = Path.GetExtension(path);
            var descriptor = RegisteredLanguages.FirstOrDefault(x => !x.SupportAnyExtension && x.FileExtensions.Contains(extension.ToLower()));
            if (descriptor == null)
            {
                return GetLanguage<PlainTextLanguage>();
            }
            return descriptor;
        }

        /// <summary>
        /// Gets the instance of a specific language descriptor by specifying the name.
        /// </summary>
        /// <param name="name">The name of the language to get.</param>
        /// <returns>A language descriptor with the given name, or null if none can be found.</returns>
        public static LanguageDescriptor GetLanguageByName(string name)
        {
            var descriptor = RegisteredLanguages.FirstOrDefault(x => x.Name == name);
            if (descriptor == null)
            {
                throw new ArgumentException(string.Format("The language descriptor with the name {0} is either not supported or not registered.", name));
            }
            return descriptor;
        }

        /// <summary>
        /// Gets the instance of a specific language descriptor.
        /// </summary>
        /// <typeparam name="TLanguage">The language descriptor to get.</typeparam>
        /// <returns>A language descriptor of type <typeparamref name="TLanguage"/>, or null if none can be found.</returns>
        public static TLanguage GetLanguage<TLanguage>() where TLanguage : LanguageDescriptor
        {
            var descriptor = RegisteredLanguages.FirstOrDefault(x => x is TLanguage);
            if (descriptor == null)
            {
                throw new ArgumentException(string.Format("The language descriptor of type {0} is either not supported or not registered.", typeof(TLanguage)));
            }
            return descriptor as TLanguage;
        }

        public LanguageDescriptor()
        {
            Templates = new List<Template>();
        }

        /// <summary>
        /// Gets the name of this language.
        /// </summary>
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// Gets the order of this language.
        /// </summary>
        public abstract string LanguageOrder
        {
            get;
        }
        
        /// <summary>
        /// Gets the file extension of a normal source file of this language.
        /// </summary>
        public abstract string[] FileExtensions
        {
            get;
        }

        /// <summary>
        /// Gets the standard file extension of a normal source file of this language.
        /// </summary>
        public abstract string StandardFileExtension
        {
            get;
        }

        /// <summary>
        /// Determines when the language supports any kind of file extension.
        /// </summary>
        public bool SupportAnyExtension
        {
            get { return FileExtensions == null || FileExtensions.Length == 0; }
        }

        /// <summary>
        /// Gets an array of key words that are used by this language.
        /// </summary>
        public abstract string[] Keywords
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the language is case sensitive or not.
        /// </summary>
        public abstract bool CaseSensitive
        {
            get;
        }

        /// <summary>
        /// Gets the prefix used for commenting in this language.
        /// </summary>
        public abstract string CommentPrefix { get; }

        /// <summary>
        /// Gets the suffix used for commenting in this language.
        /// </summary>
        public abstract string CommentSuffix { get; }

        /// <summary>
        /// Gets an array of snippets that can be used for developing in this language.
        /// </summary>
        public abstract Snippet[] Snippets
        {
            get;
        }

        /// <summary>
        /// Gets a list of templates available for this language.
        /// </summary>
        public virtual List<Template> Templates
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the language descriptor can create a snapshot.
        /// </summary>
        public abstract bool CanCreateSnapshot
        {
            get;
        }

        /// <summary>
        /// Creates a new source snapshot of a source by following the rules of this language.
        /// </summary>
        /// <param name="source">The source code to make the snapshot from.</param>
        /// <returns>A snapshot of the given source.</returns>
        public abstract SourceSnapshot CreateSourceSnapshot(string source);

        /// <summary>
        /// Determines what registered project descriptors support this language.
        /// </summary>
        /// <returns>An enumerable collection of project descriptors supporting this language.</returns>
        public IEnumerable<ProjectDescriptor> GetProjectDescriptors()
        {
            foreach (var descriptor in ProjectDescriptor.ProjectDescriptors)
                if (descriptor.Languages.FirstOrDefault(x => x.GetType() == this.GetType()) != null)
                    yield return descriptor;
        }
    }
}
