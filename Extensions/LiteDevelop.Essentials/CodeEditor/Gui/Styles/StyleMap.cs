using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteDevelop.Framework.Gui;

namespace LiteDevelop.Essentials.CodeEditor.Gui.Styles
{
    public class StyleMap
    {
        private AppearanceMap _map, _fallBackMap;

        public StyleMap(AppearanceMap map, AppearanceMap fallBackMap)
        {
            _map = map;
            _fallBackMap = fallBackMap;

            KeywordStyle = new DynamicTextStyle(GetDescription("Keyword"));
            StringStyle = new DynamicTextStyle(GetDescription("String"));
            NumberStyle = new DynamicTextStyle(GetDescription("Number"));
            TypeDefinitionStyle = new DynamicTextStyle(GetDescription("TypeDefinition"));
            CommentStyle = new DynamicTextStyle(GetDescription("Comment"));
            AttributeStyle = new DynamicTextStyle(GetDescription("Attribute"));
            BreakpointStyle = new DynamicTextStyle(GetDescription("Breakpoint"));
            InstructionPointer = new DynamicTextStyle(GetDescription("InstructionPointer"));
        }

        private AppearanceDescription GetDescription(string id)
        {
            AppearanceDescription description = _map.GetDescriptionById(id);
            if (description == null && (description = _fallBackMap.GetDescriptionById(id)) != null)
            {
                _map.Descriptions.Add(description);
            }
            return description;
        }

        public DynamicTextStyle KeywordStyle
        {
            get;
            private set;
        }

        public DynamicTextStyle StringStyle
        {
            get;
            private set;
        }

        public DynamicTextStyle TypeDefinitionStyle
        {
            get;
            private set;
        }

        public DynamicTextStyle NumberStyle
        {
            get;
            private set;
        }

        public DynamicTextStyle CommentStyle
        {
            get;
            private set;
        }

        public DynamicTextStyle AttributeStyle
        {
            get;
            private set;
        }

        public DynamicTextStyle BreakpointStyle
        {
            get;
            private set;
        }

        public DynamicTextStyle InstructionPointer
        {
            get;
            private set;
        }
    }
}
