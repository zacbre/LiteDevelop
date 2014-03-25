using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework.Extensions;
using WeifenLuo.WinFormsUI.Docking;

namespace LiteDevelop.Gui.DockContents
{
    public partial class PropertiesContent : DockContent
    {
        private Dictionary<object, string> _componentMuiIdentifiers;
        private PropertyContainer _propertyContainer;

        public PropertiesContent()
        {
            InitializeComponent();
            this.HideOnClose = true;
            this.Icon = Icon.FromHandle(Properties.Resources.property.GetHicon());
            LiteDevelopApplication.Current.InitializedApplication += Current_InitializedApplication;
        }

        private void Current_InitializedApplication(object sender, EventArgs e)
        {
            _componentMuiIdentifiers = new Dictionary<object, string>()
            {
                {this, "PropertiesContent.Title"},
            };

            LiteDevelopApplication.Current.ExtensionHost.UILanguageChanged += ExtensionHost_UILanguageChanged;
            LiteDevelopApplication.Current.ExtensionHost.ControlManager.AppearanceChanged += ControlManager_AppearanceChanged;
            ExtensionHost_UILanguageChanged(null, null);

        }

        public void SetPropertyContainer(PropertyContainer container)
        {
            if (_propertyContainer != null)
            {
                _propertyContainer.SelectedObjectsChanged -= _propertyContainer_SelectedObjectsChanged;
            }

            _propertyContainer = container;
            UpdatePropertyGrid();

            if (_propertyContainer != null)
            {
                _propertyContainer.SelectedObjectsChanged += _propertyContainer_SelectedObjectsChanged;
            }
        }

        private void UpdatePropertyGrid()
        {
            if (_propertyContainer == null)
            {
                mainPropertyGrid.SelectedObjects = null;
            }
            else 
            {
                mainPropertyGrid.SelectedObjects = _propertyContainer.SelectedObjects;
            }
        }

        private void _propertyContainer_SelectedObjectsChanged(object sender, EventArgs e)
        {
            UpdatePropertyGrid();
        }

        private void ExtensionHost_UILanguageChanged(object sender, EventArgs e)
        {
            LiteDevelopApplication.Current.MuiProcessor.ApplyLanguageOnComponents(_componentMuiIdentifiers);
        }

        private void ControlManager_AppearanceChanged(object sender, EventArgs e)
        {
            var processor = LiteDevelopApplication.Current.ExtensionHost.ControlManager.GlobalAppearanceMap.Processor;
            processor.ApplyAppearanceOnObject(this, Framework.Gui.DefaultAppearanceDefinition.Window);
            processor.ApplyAppearanceOnObject(mainPropertyGrid, Framework.Gui.DefaultAppearanceDefinition.ListView);
        }
    }
}
