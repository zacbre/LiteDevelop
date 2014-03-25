using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Windows.Forms;

namespace LiteDevelop.Framework.Gui
{
    public static class MenuService
    {
        public static void ShowContextMenu(IMenuCommandService service, Control owner, int x, int y)
        {
            var menu = new ContextMenuStrip();
            menu.Items.AddRange(GetMenuItems(service.Verbs));
            menu.Show(owner, x, y);

        }

        private static ToolStripMenuItem[] GetMenuItems(DesignerVerbCollection verbs)
        {
            var menuItems = new ToolStripMenuItem[verbs.Count];
            for (int i = 0; i < menuItems.Length; i++)
            {
                menuItems[i] = new DesignerToolStripMenuItem(verbs[i].Text, verbs[i]);
            }
            return menuItems;
        }

    }

    public class DesignerToolStripMenuItem : ToolStripMenuItem
    {
        public DesignerToolStripMenuItem(string text, DesignerVerb verb)
            : base(text)
        {
            if (verb == null)
                throw new ArgumentNullException("verb");
            Verb = verb;
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            Verb.Invoke();
        }

        public DesignerVerb Verb { get; private set; }

    }
}
