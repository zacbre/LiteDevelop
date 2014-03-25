using System;
using System.Collections.Generic;
using System.Linq;
using FastColoredTextBoxNS;
using LiteDevelop.Framework;
using LiteDevelop.Framework.Languages;

namespace LiteDevelop.Essentials.CodeEditor.Syntax
{
    /// <summary>
    /// The base of all auto-completion mappings and provides an enumerator for getting all the auto-complete members.
    /// </summary>
    public abstract class AutoCompletionMap : IEnumerable<AutocompleteItem>
    {
        public event EventHandler IconProviderChanged;

        IconProvider _iconProvider;
        AutocompleteMenu _menu;

        public AutoCompletionMap(AutocompleteMenu menu)
        {
            _menu = menu;
        }

        public AutocompleteMenu AutoCompleteMenu
        {
            get { return _menu; }
        }

        public abstract string SearchPattern
        {
            get;
        }

        public abstract LanguageDescriptor Language 
        {
            get; 
        }

        /// <summary>
        /// Gets or sets the icon provider of this auto-completion map.
        /// </summary>
        public IconProvider IconProvider
        {
            get { return _iconProvider; }
            set
            {
                if (_iconProvider != value)
                {
                    _iconProvider = value;

                    AutoCompleteMenu.ImageList = AutoCompleteMenu.Items.ImageList = _iconProvider.ImageList;
                    if (IconProviderChanged != null)
                        IconProviderChanged(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets the enumerator for this auto-completion map, returning all auto-complete members.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator<AutocompleteItem> GetEnumerator();
        
        /// <summary>
        /// Gets the enumerator for this auto-completion map, returning all auto-complete members.
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

}
