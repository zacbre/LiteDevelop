using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;

namespace LiteDevelop.Framework.Extensions
{
    public interface IToolboxServiceProvider
    {
        /// <summary>
        /// Gets the toolbox service that is associated with this object.
        /// </summary>
        ToolboxService ToolboxService { get; }
    }

    public delegate void ToolBoxItemEventHandler(object sender, ToolBoxItemEventArgs e);

    public class ToolBoxItemEventArgs : EventArgs
    {
        public ToolBoxItemEventArgs(ToolboxItem item)
        {
            Item = item;
        }

        public ToolboxItem Item { get; private set; }
    }

    // TODO: fill in empty members
    public abstract class ToolboxService : IToolboxService
    {
        public event ToolBoxItemEventHandler SelectedItemUsed;
        public event ToolBoxItemEventHandler SelectedItemChanged;

        private CategoryNameCollection _categoryNames;
        private ToolboxItem _selectedToolBoxItem;

        public ToolboxService()
        {
            _categoryNames = new CategoryNameCollection(new string[] { "All Components" });
            SelectedCategory = _categoryNames[0];
        }

        public TreeView TreeView
        {
            get;
            set;
        }

        public virtual string SelectedCategory
        {
            get;
            set;
        }

        public virtual CategoryNameCollection CategoryNames
        {
            get { return _categoryNames; }
        }

        public virtual void AddCreator(ToolboxItemCreatorCallback creator, string format)
        {
        }

        public virtual void AddCreator(ToolboxItemCreatorCallback creator, string format, IDesignerHost host)
        {
        }

        public virtual void AddLinkedToolboxItem(ToolboxItem toolboxItem, string category, IDesignerHost host)
        {
        }

        public virtual void AddLinkedToolboxItem(ToolboxItem toolboxItem, IDesignerHost host)
        {
        }

        public virtual void AddToolboxItem(ToolboxItem toolboxItem, string category)
        {
        }

        public virtual void AddToolboxItem(ToolboxItem toolboxItem)
        {
        }

        public virtual ToolboxItem DeserializeToolboxItem(object serializedObject, IDesignerHost host)
        {
            return null;
        }

        public virtual ToolboxItem DeserializeToolboxItem(object serializedObject)
        {
            return null;
        }

        public virtual ToolboxItem GetSelectedToolboxItem(IDesignerHost host)
        {
            return _selectedToolBoxItem;
        }

        public virtual ToolboxItem GetSelectedToolboxItem()
        {
            return _selectedToolBoxItem;
        }

        public abstract ToolboxItemCollection GetToolboxItems(string category, IDesignerHost host);

        public abstract ToolboxItemCollection GetToolboxItems(string category);

        public abstract ToolboxItemCollection GetToolboxItems(IDesignerHost host);

        public abstract ToolboxItemCollection GetToolboxItems();

        public virtual bool IsSupported(object serializedObject, ICollection filterAttributes)
        {
            return true;
        }

        public virtual bool IsSupported(object serializedObject, IDesignerHost host)
        {
            return true;
        }

        public virtual bool IsToolboxItem(object serializedObject, IDesignerHost host)
        {
            return false;
        }

        public virtual bool IsToolboxItem(object serializedObject)
        {
            return false;
        }

        public virtual void Refresh()
        {
        }

        public virtual void RemoveCreator(string format, IDesignerHost host)
        {
        }

        public virtual void RemoveCreator(string format)
        {
        }

        public virtual void RemoveToolboxItem(ToolboxItem toolboxItem, string category)
        {
        }

        public virtual void RemoveToolboxItem(ToolboxItem toolboxItem)
        {
        }

        public virtual void SelectedToolboxItemUsed()
        {
            OnSelectedItemUsed(new ToolBoxItemEventArgs(GetSelectedToolboxItem()));
        }

        public virtual object SerializeToolboxItem(ToolboxItem toolboxItem)
        {
            return null;
        }

        public virtual bool SetCursor()
        {
            return !(_selectedToolBoxItem == null || _selectedToolBoxItem.DisplayName == "Pointer");
        }

        public virtual void SetSelectedToolboxItem(ToolboxItem toolboxItem)
        {
            if (toolboxItem != _selectedToolBoxItem)
            {
                _selectedToolBoxItem = toolboxItem;

                OnSelectedItemChanged(new ToolBoxItemEventArgs(_selectedToolBoxItem));
            }
        }

        protected virtual void OnSelectedItemUsed(ToolBoxItemEventArgs e)
        {
            if (SelectedItemUsed != null)
                SelectedItemUsed(this, e);
        }

        protected virtual void OnSelectedItemChanged(ToolBoxItemEventArgs e)
        {
            if (SelectedItemChanged != null)
                SelectedItemChanged(this, e);
        }
    }
}
