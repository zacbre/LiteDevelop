using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;

namespace LiteDevelop.Essentials.FormsDesigner.Services
{
    
    public class FormsToolBoxBuilder : IDisposable
    {

        private Dictionary<Assembly, ToolboxItemCollection> _cachedAssemblies = new Dictionary<Assembly, ToolboxItemCollection>();
         
        public ToolboxItemCollection CollectItemsFromAssembly(Assembly assembly)
        {
            if (_cachedAssemblies.ContainsKey(assembly))
                return _cachedAssemblies[assembly];

            var items = new List<ToolboxItem>();
   
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsPublic && !type.IsAbstract && type.HasConstructor() && type.IsBasedOn(typeof(Component)))
                {
                    var visibleAttribute = TypeDescriptor.GetAttributes(type)[typeof(DesignTimeVisibleAttribute)] as DesignTimeVisibleAttribute;
                    //var toolBoxAttribute = TypeDescriptor.GetAttributes(type)[typeof(ToolboxItemAttribute)] as ToolboxItemAttribute;
                    if (visibleAttribute != null && visibleAttribute.Visible)
                    {
                        items.Add(new ToolboxItem(type));
                    }
                }
            }
            items.Sort((x, y) => { return x.DisplayName.CompareTo(y.DisplayName); });
            return new ToolboxItemCollection(items.ToArray());
        }
   
        public void Dispose()
        {
            _cachedAssemblies.Clear();
        }
    }
}
