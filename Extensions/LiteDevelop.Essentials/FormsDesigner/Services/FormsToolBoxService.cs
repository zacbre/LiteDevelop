using System;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework.Extensions;

namespace LiteDevelop.Essentials.FormsDesigner.Services
{
    public class FormsToolBoxService : ToolboxService
    {
        private ToolboxItemCollection _toolBoxItems;

        public FormsToolBoxService(FormsToolBoxBuilder builder)
        {
            _toolBoxItems = builder.CollectItemsFromAssembly(typeof(Form).Assembly);
        }

        public override ToolboxItemCollection GetToolboxItems(string category, IDesignerHost host)
        {
            return GetToolboxItems();
        }

        public override ToolboxItemCollection GetToolboxItems(string category)
        {
            return GetToolboxItems();
        }

        public override ToolboxItemCollection GetToolboxItems(IDesignerHost host)
        {
            return GetToolboxItems();
        }

        public override ToolboxItemCollection GetToolboxItems()
        {
            return _toolBoxItems;
        }

        public override void SetSelectedToolboxItem(ToolboxItem toolboxItem)
        {
            if (toolboxItem == null)
                base.SetSelectedToolboxItem(FindToolBoxItem(x => x.DisplayName == "Pointer"));
            else
                base.SetSelectedToolboxItem(toolboxItem);

        }

        private ToolboxItem FindToolBoxItem(Func<ToolboxItem, bool> condition)
        {
            foreach (ToolboxItem item in _toolBoxItems)
                if (condition(item))
                    return item;
            return null;
        }
    }
}
