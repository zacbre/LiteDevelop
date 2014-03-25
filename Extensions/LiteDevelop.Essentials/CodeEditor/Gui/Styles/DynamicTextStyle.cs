using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using FastColoredTextBoxNS;
using LiteDevelop.Framework.Gui;

namespace LiteDevelop.Essentials.CodeEditor.Gui.Styles
{
    public class DynamicTextStyle : TextStyle 
    {
        public DynamicTextStyle(AppearanceDescription description)
            : base(new SolidBrush(description.ForeColor), new SolidBrush(description.BackColor), description.FontStyle)
        {
            Description = description;
            description.ForeColorChanged += description_ForeColorChanged;
            description.BackColorChanged += description_BackColorChanged;
            description.FontStyleChanged += description_FontStyleChanged;
        }

        private void description_ForeColorChanged(object sender, EventArgs e)
        {
            if (ForeBrush != null)
                ForeBrush.Dispose();
            ForeBrush = new SolidBrush(Description.ForeColor);
        }

        private void description_BackColorChanged(object sender, EventArgs e)
        {
            if (BackgroundBrush != null)
                BackgroundBrush.Dispose();
            BackgroundBrush = new SolidBrush(Description.BackColor);
        }

        private void description_FontStyleChanged(object sender, EventArgs e)
        {
            FontStyle = Description.FontStyle;
        }

        public AppearanceDescription Description
        {
            get;
            set;
        }
        
    }
}
