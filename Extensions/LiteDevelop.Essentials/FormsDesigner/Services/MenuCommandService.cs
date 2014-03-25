using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.Gui;

namespace LiteDevelop.Essentials.FormsDesigner.Services
{
    public class MenuCommandService : System.ComponentModel.Design.MenuCommandService
    {
        private DesignerVerbCollection _verbs = new DesignerVerbCollection();
        private ILiteExtensionHost _extensionHost;
        private IDesignerHost _designerHost;

        public MenuCommandService(ILiteExtensionHost extensionHost, IDesignerHost parentHost)
            : base(parentHost)
        {
            _extensionHost = extensionHost;
            _designerHost = parentHost;

            AddVerb(CreateStandardVerb("Delete", StandardCommands.Delete, MenuCommandService_Delete));
        }

        public override DesignerVerbCollection Verbs
        {
            get
            {
                return _verbs;
            }
        }
   
        private DesignerVerb CreateStandardVerb(string text, CommandID command, EventHandler eventHandler)
        {
            var verb = new DesignerVerb(text, (o, e) => this.GlobalInvoke(command));
            if (eventHandler != null)
            {
                AddCommand(new MenuCommand(eventHandler, command));
            }

            return verb;
        }

        public override void ShowContextMenu(CommandID menuID, int x, int y)
        {
            var selectionService = this.GetService(typeof(ISelectionService)) as ISelectionService;
            var selectedControl = selectionService.PrimarySelection as Control;
            
            var point = selectedControl.PointToScreen(new Point(0, 0));
            MenuService.ShowContextMenu(this, selectedControl, x - point.X, y - point.Y);
        }

        private void MenuCommandService_Delete(object sender, EventArgs e)
        {
            var selectionService = _designerHost.GetService<ISelectionService>();
            var components = new object[selectionService.SelectionCount];
            selectionService.GetSelectedComponents().CopyTo(components, 0);

            foreach (var component in components)
            {
                _designerHost.DestroyComponent((IComponent)component);
            }
        }
    }


}
