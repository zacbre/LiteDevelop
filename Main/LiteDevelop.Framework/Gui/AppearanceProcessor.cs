using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace LiteDevelop.Framework.Gui
{
    public class AppearanceProcessor
    {
        private AppearanceMap _appearanceMap;

        public AppearanceProcessor(AppearanceMap map)
        {
            _appearanceMap = map;
        }

        public void ApplyAppearanceOnObject(object obj, DefaultAppearanceDefinition definition)
        {
            ApplyAppearanceOnObject(obj, definition.ToString());
        }

        public void ApplyAppearanceOnObject(object obj, string descriptionIdentifier)
        {
            var description = _appearanceMap.GetDescriptionById(descriptionIdentifier);
            if (description != null)
            {
                SetPropertyValue(obj, "ForeColor", description.ForeColor);
                SetPropertyValue(obj, "BackColor", description.BackColor);
                SetPropertyValue(obj, "Font", new Font(GetPropertyValue<Font>(obj, "Font"), description.FontStyle));
            }
        }

        private static void SetPropertyValue(object instance, string name, object value)
        {
            instance.GetType().GetProperty(name).SetValue(instance, value, null);
        }

        private static T GetPropertyValue<T>(object instance, string name)
        {
            return (T)instance.GetType().GetProperty(name).GetValue(instance, null);
        }
    }
}
