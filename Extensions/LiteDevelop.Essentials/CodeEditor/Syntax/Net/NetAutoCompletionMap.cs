using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FastColoredTextBoxNS;
using LiteDevelop.Framework;
using LiteDevelop.Framework.Languages;
using LiteDevelop.Framework.Languages.Net;

namespace LiteDevelop.Essentials.CodeEditor.Syntax.Net
{
    /// <summary>
    /// The base of all auto-completion maps for .NET languages.
    /// </summary>
    public abstract class NetAutoCompletionMap : AutoCompletionMap, ISnapshotProvider, ICodeBlockCompleter 
    {
        public event EventHandler SnapshotUpdated;
        private string[] _memberSeparators;
        private List<CodeEditorAutoCompleteItem> _keyWordItems;
        private List<CodeEditorSnippetAutoCompleteItem> _snippetItems;
        private NetSourceSnapshot _snapshot;
        private NetSourceSnapshot _lastSnapshot;
        private List<CodeEditorAutoCompleteItem> _cachedTypes;
        
        public NetAutoCompletionMap(AutocompleteMenu menu, string[] separators)
            : base(menu)
        {
            _memberSeparators = separators;
            _cachedTypes = new List<CodeEditorAutoCompleteItem>();
            IconProvider = IconProvider.GetProvider<AssemblyIconProvider>();

            InitKeywordItems(Language.Keywords);
            InitSnippetItems(Language.Snippets);
        }

        public override string SearchPattern
        {
            get { return @"[\w\.]"; }
        }

        public abstract string ThisKeyword { get; }
        public abstract string BaseKeyword { get; }

        public SourceSnapshot CurrentSnapshot
        {
            get { return _snapshot; }
            set
            {
                if (_snapshot != value)
                {
                    _lastSnapshot = _snapshot;
                    _snapshot = (NetSourceSnapshot)value;
                    OnSnapshotUpdated(EventArgs.Empty);
                }
            }
        }

        public abstract IDictionary<string, string> BlockIdentifiers { get; }

        public abstract void UpdateSnapshot(string source);

        /// <summary>
        /// Gets the enumerator for this auto-completion map, returning all auto-complete members.
        /// </summary>
        /// <returns></returns>
        public override IEnumerator<AutocompleteItem> GetEnumerator()
        {
            var currentFragment = this.AutoCompleteMenu.Fragment;
            string currentFragmentText = currentFragment.Text;

            int separatorIndex = GetSeparatorIndex(currentFragmentText);

            int currentPosition = this.AutoCompleteMenu.Fragment.tb.PlaceToPosition(currentFragment.Start);

            if (separatorIndex == -1)
            {
                // if no separator is detected, get snippets, this-members and cached type names.
                var snippetEnumerator = GetEnumeratorOfObject(currentPosition, null);
                var typeEnumerator = _cachedTypes.GetEnumerator();

                return snippetEnumerator.CombineWith(typeEnumerator).GetSortedEnumerator(CompareItems);
            }

            // if separator is detected, try to find autocomplete items for the current fragment.
            currentFragmentText = currentFragmentText.Remove(separatorIndex);

            if (!string.IsNullOrEmpty(currentFragmentText))
            {
                // get all pieces.
                string[] words = currentFragmentText.Split(_memberSeparators, StringSplitOptions.None);

                object currentMember = null;

                for (int i = 0; i < words.Length; i++)
                {
                    // find member of current piece.
                    currentMember = FindMember(currentPosition, currentMember, words[i]);
                }

                // get item enumerator and yield items.
                var enumerator = GetEnumeratorOfObject(currentPosition, currentMember);
                if (enumerator != null)
                {
                    return enumerator.GetSortedEnumerator(CompareItems);
                }
            }

            return new EmptyEnumerator<AutocompleteItem>();
        }

        private IEnumerator<AutocompleteItem> GetEnumeratorOfObject(int currentPosition, object item)
        {
            Type typeOfParent = GetTypeOfItem(currentPosition, item);
            if (item is Type)
                return GetTypeMembersEnumerator(typeOfParent, false);
            else if (item is MemberInfo || item is NetSnapshotMember)
                return GetTypeMembersEnumerator(typeOfParent, true);
            else if (item is string)
            {
                if (item.ToString() == ThisKeyword)
                    return GetSourceMembersEnumerator(true).CombineWith(GetTypeMembersEnumerator(typeOfParent, true));
                else if (item.ToString() == BaseKeyword)
                    return GetTypeMembersEnumerator(typeOfParent, true);
            }
            else if (item == null)
                return GetSnippetEnumerator().CombineWith(GetSourceMembersEnumerator(false));
               
            return null;
        }

        private IEnumerator<AutocompleteItem> GetSourceMembersEnumerator(bool useMemberItems = false)
        {
            var snapshot = this.CurrentSnapshot as NetSourceSnapshot;
            if (snapshot == null)
                yield break;

            var methods = new Dictionary<string, AutocompleteItem>();
            foreach (var member in snapshot.Methods)
            {
                AutocompleteItem item;
                if (!methods.TryGetValue(member.Name, out item))
                {
                    item = CreateItem(member, IconProvider.GetImageIndex(typeof(MethodInfo)), useMemberItems, true);

                    methods.Add(member.Name, item);
                    yield return item;
                }
                else if (string.IsNullOrEmpty(item.ToolTipText))
                {
                    item.ToolTipText = "2 Overloads";
                }
                else
                {
                    int overloads = int.Parse(item.ToolTipText.Split(' ')[0]) + 1;
                    item.ToolTipText = overloads.ToString() + " Overloads";
                }
            }

            foreach (var member in snapshot.Fields)
            {
                yield return CreateItem(member, IconProvider.GetImageIndex(typeof(FieldInfo)), useMemberItems, false);
            }

            foreach (var member in snapshot.Properties)
            {
                yield return CreateItem(member, IconProvider.GetImageIndex(typeof(PropertyInfo)), useMemberItems, false);
            }

            foreach (var member in snapshot.Events)
            {
                yield return CreateItem(member, IconProvider.GetImageIndex(typeof(EventInfo)), useMemberItems, false);
            }
        }

        private IEnumerator<AutocompleteItem> GetSnippetEnumerator()
        {
            int snippetImageIndex = IconProvider.GetImageIndex(string.Empty);
            for (int i = 0; i < _keyWordItems.Count; i++)
            {
                _keyWordItems[i].ImageIndex = snippetImageIndex;
                yield return _keyWordItems[i];
            }
            for (int i = 0; i < _snippetItems.Count; i++)
            {
                _snippetItems[i].ImageIndex = snippetImageIndex;
                yield return _snippetItems[i];
            }
            yield break;
        }

        private IEnumerator<AutocompleteItem> GetTypeMembersEnumerator(Type type, bool isInstance)
        {
            if (type == null)
                yield break;

            BindingFlags searchFlags;

            if (isInstance)
            {
                searchFlags = BindingFlags.Public | BindingFlags.Instance;
            }
            else
            {
                searchFlags = BindingFlags.Public | BindingFlags.Static;
            }

            if (!isInstance)
            {
                foreach (Type nestedType in type.GetNestedTypes(BindingFlags.Public))
                    yield return new CodeEditorMemberAutoCompleteItem(nestedType.Name, _memberSeparators)
                    {
                        ImageIndex = this.IconProvider.GetImageIndex(nestedType),
                        ToolTipTitle = string.Format("Class {0}", nestedType.FullName),
                        Tag = nestedType,
                    };
            }

            foreach (EventInfo @event in type.GetEvents(searchFlags))
                yield return new CodeEditorMemberAutoCompleteItem(@event.Name, _memberSeparators)
                {
                    ImageIndex = this.IconProvider.GetImageIndex(@event),
                    ToolTipTitle = string.Format("Event {0} {1}.{2}", @event.EventHandlerType.FullName, @event.DeclaringType.FullName, @event.Name),
                    Tag = @event,
                };

            foreach (FieldInfo field in type.GetFields(searchFlags))
                yield return new CodeEditorMemberAutoCompleteItem(field.Name, _memberSeparators)
                {
                    ImageIndex = this.IconProvider.GetImageIndex(field),
                    ToolTipTitle = string.Format("Field {0} {1}.{2}", field.FieldType.FullName, field.DeclaringType.FullName, field.Name),
                    Tag = field,
                };

            foreach (PropertyInfo property in type.GetProperties(searchFlags))
                yield return new CodeEditorMemberAutoCompleteItem(property.Name, _memberSeparators)
                {
                    ImageIndex = this.IconProvider.GetImageIndex(property),
                    ToolTipTitle = string.Format("Property {0} {1}.{2}", property.PropertyType.FullName, property.DeclaringType.FullName, property.Name),
                    Tag = property,
                };

            List<string> methods = new List<string>();
            foreach (MethodInfo method in type.GetMethods(searchFlags).Where(m => !m.IsSpecialName))
            {
                if (!methods.Contains(method.Name))
                {
                    yield return new CodeEditorMethodAutoCompleteItem(method.Name, _memberSeparators)
                    {
                        ImageIndex = this.IconProvider.GetImageIndex(method),
                        ToolTipTitle = string.Format("Method {0} {1}.{2}", method.ReturnType.FullName, method.DeclaringType.FullName, method.Name),
                        Tag = method,
                    };
                    methods.Add(method.Name);
                }
            }
            methods.Clear();
        }

        private void InitKeywordItems(string[] keyWords)
        {
            _keyWordItems = new List<CodeEditorAutoCompleteItem>();
            if (keyWords != null)
            {
                for (int i = 0; i < keyWords.Length; i++)
                {
                    var keywordItem = _keyWordItems.FirstOrDefault(x => x.Text == keyWords[i]);
                    if (keywordItem == null)
                        _keyWordItems.Add(new CodeEditorSnippetAutoCompleteItem(keyWords[i], keyWords[i]));
                }
            }
        }

        private void InitSnippetItems(Snippet[] snippets)
        {
            _snippetItems = new List<CodeEditorSnippetAutoCompleteItem>();
            if (snippets != null)
            {
                for (int i = 0; i < snippets.Length; i++)
                {
                    _snippetItems.Add(new CodeEditorSnippetAutoCompleteItem(snippets[i].Title, snippets[i].Code) { SurpressSpaceBar = true });
                    var keywordItem = _keyWordItems.FirstOrDefault(x => x.Text == snippets[i].Title);
                    if (keywordItem != null)
                        _keyWordItems.Remove(keywordItem);
                }
            }
        }

        private object FindMember(int currentPosition, object parent, string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            if (name[0] == ')')
            {
                name = FindActualMethodName(name, this.AutoCompleteMenu.Fragment.Start.iChar);
            }

            // try to get a type from current name.
            Type typeByName = _snapshot.GetTypeByName(name);
 
            if (typeByName != null)
                return typeByName;

            // try to find enumerator of parent
            IEnumerator<AutocompleteItem> enumerator = GetEnumeratorOfObject(currentPosition, parent);
            
            if (enumerator != null)
            {
                // search for the member with the same name.
                while (enumerator.MoveNext())
                {
                    if (enumerator.Current.Tag is MemberInfo)
                    {
                        if (StringsAreEqual((enumerator.Current.Tag as MemberInfo).Name, name))
                            return enumerator.Current.Tag;
                    }
                    else if (enumerator.Current.Tag is NetSnapshotMember)
                    {
                        if (StringsAreEqual((enumerator.Current.Tag as NetSnapshotMember).Name, name))
                            return enumerator.Current.Tag;
                    }
                }
            }

            return name;
        }

        private Type GetTypeOfItem(int currentPosition, object item)
        {
            if (item is PropertyInfo)
                return (item as PropertyInfo).PropertyType;
            if (item is FieldInfo)
                return (item as FieldInfo).FieldType;
            if (item is MethodInfo)
                return (item as MethodInfo).ReturnType;
            if (item is Type)
                return item as Type;
            if (item is NetSnapshotMember)
                return _snapshot.GetTypeByName((item as NetSnapshotMember).ValueType);
            if (item is string)
            {
                if (((item as string) == ThisKeyword || (item as string) == BaseKeyword) && _snapshot.Types.Length != 0)
                {
                    
                    var classMember = FindEnclosingMember(_snapshot.Types, currentPosition);
                    if (classMember != null && !string.IsNullOrEmpty(classMember.ValueType))
                        return _snapshot.GetTypeByName(classMember.ValueType);
                    return typeof(object);
                }
                    
                return _snapshot.GetTypeByName(item as string);
            }
            return null;
        }

        private AutocompleteItem CreateItem(NetSnapshotMember member, int imageIndex, bool isMember, bool isMethod)
        {
            AutocompleteItem item;
            if (isMember)
            {
                if (isMethod)
                    item = new CodeEditorMethodAutoCompleteItem(member.Name, _memberSeparators);
                else
                    item = new CodeEditorMemberAutoCompleteItem(member.Name, _memberSeparators);

            }
            else
                item = new CodeEditorAutoCompleteItem(member.Name);

            item.ImageIndex = imageIndex;
            item.ToolTipTitle = member.ToString();
            item.Tag = member;
            return item;
        }

        private int GetSeparatorIndex(string fragment)
        {
            for (int i = 0; i < _memberSeparators.Length; i++)
            {
                int index = fragment.LastIndexOf(_memberSeparators[i]);
                if (index != -1)
                    return index;
            }
            return -1;
        }

        private void UpdateTypeCache()
        {
            _cachedTypes.Clear();
            AddToTypeCache(_snapshot.UsingNamespaces);
        }

        private void AddToTypeCache(IEnumerable<NetSnapshotMember> usingNamesaces)
        {            
            foreach (var usingNamespace in usingNamesaces)
            {
                _cachedTypes.Add(new CodeEditorAutoCompleteItem(usingNamespace.Name, IconProvider.GetImageIndex(null)));

                // TODO: better assembly searching
                // - only search project references
                foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                {
                    var enumerator = asm.GetTypesInNamespace(usingNamespace.Name);
                    while (enumerator.MoveNext())
                        _cachedTypes.Add(new CodeEditorAutoCompleteItem(enumerator.Current.Name, IconProvider.GetImageIndex(typeof(Type))) { Tag = enumerator.Current });
                }
            }
        }

        private void RemoveFromTypeCache(IEnumerable<NetSnapshotMember> usingNamesaces)
        {
            if (_cachedTypes.Count == 0)
                return;

            int index = 0, maximum = _cachedTypes.Count;
            while (index < maximum)
            {
                var cachedItem = _cachedTypes[index];
                var cachedType = cachedItem.Tag as Type;

                foreach (var usingNamespace in usingNamesaces)
                {
                    if (cachedType == null)
                    {
                        if (StringsAreEqual(cachedItem.MenuText ?? cachedItem.Text, usingNamespace.Name))
                        {
                            _cachedTypes.RemoveAt(index);
                            index--;
                            maximum--;
                            break;
                        }
                    }
                    else
                    {
                        if (StringsAreEqual(cachedType.Namespace, usingNamespace.Name))
                        {
                            _cachedTypes.RemoveAt(index);
                            index--;
                            maximum--;
                            break;
                        }
                    }
                }

                index++;
            }
        }

        private string FindActualMethodName(string line, int startingIndex)
        {
            if (startingIndex >= line.Length)
                return string.Empty;

            int index = 0;
            int nestedBracets = 0;

            for (int i = startingIndex; i >= 0; i--)
            {
                if (line[i] == ')')
                    nestedBracets++;
                if (line[i] == '(')
                    nestedBracets--;
                if (nestedBracets == 0)
                {
                    index = i;
                    break;
                }
            }

            return line.Substring(index, startingIndex - index);
        }

        private static int CompareItems(AutocompleteItem x, AutocompleteItem y)
        { 
            return (x.MenuText ?? x.Text).CompareTo(y.MenuText ?? y.Text);
        }

        private static NetSnapshotMember FindEnclosingMember(NetSnapshotMember[] members, int index)
        {
            for (int i = 0; i < members.Length; i++)
            {
                if (members[i].Start > index)
                {
                    if (i == 0)
                        return null;
                    return members[i - 1];
                }
            }

            return members[members.Length - 1];
        }

        protected bool StringsAreEqual(string a, string b)
        {
            return string.Compare(a, b, !Language.CaseSensitive) == 0;
        }

        protected virtual void OnSnapshotUpdated(EventArgs e)
        {
            if (_lastSnapshot == null)
            {
                UpdateTypeCache();
            }
            else
            {
                var result = ArrayComparer.CompareArrays(_snapshot.UsingNamespaces, _lastSnapshot.UsingNamespaces, new SnapshotMemberNameComparer());
                if (!result.ArraysAreEqual)
                {
                    RemoveFromTypeCache(result.ElementsMissing.Cast<NetSnapshotMember>());
                    AddToTypeCache(result.ElementsAdded.Cast<NetSnapshotMember>());
                }
            }

            if (SnapshotUpdated != null)
                SnapshotUpdated(this, e);
        }

    }
}
