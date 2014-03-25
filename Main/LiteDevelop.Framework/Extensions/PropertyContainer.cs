using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteDevelop.Framework.Extensions
{
    public interface IPropertyContainerProvider
    {
        PropertyContainer PropertyContainer { get; }
    }

    public class PropertyContainer
    {
        public event EventHandler SelectedObjectsChanged;

        private object[] _selectedObjects;

        public object[] SelectedObjects
        {
            get { return _selectedObjects; }
            set
            {
                if (_selectedObjects !=value)
                {
                    _selectedObjects = value;
                    OnSelectedObjectsChanged(EventArgs.Empty);
                }
            }
        }

        protected virtual void OnSelectedObjectsChanged(EventArgs e)
        {
            if (SelectedObjectsChanged != null)
                SelectedObjectsChanged(this, e);
        }


    }
}
