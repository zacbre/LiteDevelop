using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using LiteDevelop.Essentials.CodeEditor.Gui.Styles;
using LiteDevelop.Framework.Debugging;
using LiteDevelop.Framework.FileSystem;
using FastColoredTextBox = FastColoredTextBoxNS.FastColoredTextBox;
using LDBookmark = LiteDevelop.Framework.FileSystem.Bookmark;
using TBBookmark = FastColoredTextBoxNS.Bookmark;
using TextStyle = FastColoredTextBoxNS.TextStyle;

namespace LiteDevelop.Essentials.CodeEditor.Gui
{
    public class CodeEditorBookmark : TBBookmark
    {
        public CodeEditorBookmark(FastColoredTextBox textBox, LDBookmark bookmark, TextStyle textStyle)
            : base(textBox, string.Empty, bookmark.Line)
        {
            InnerBookmark = bookmark;
            Style = textStyle;
        }

        public LDBookmark InnerBookmark
        {
            get;
            protected set;
        }

        public TextStyle Style
        {
            get;
            set;
        }

        public bool ColorizeEntireLine
        {
            get;
            set;
        }

        public override void Paint(Graphics gr, Rectangle lineRect)
        {
            if (Style == null)
            {
                base.Paint(gr, lineRect);
                return;
            }

            var size = (TB.CharHeight);

            gr.FillEllipse(Style.BackgroundBrush, 0, lineRect.Top, size, size);

            if (ColorizeEntireLine)
                gr.FillRectangle(Style.BackgroundBrush, lineRect);
        }
    }

    public class CodeEditorBreakpoint : CodeEditorBookmark
    {
        public CodeEditorBreakpoint(FastColoredTextBox textBox, BreakpointBookmark breakpoint, TextStyle style)
            : base(textBox, breakpoint, style)
        {
            ColorizeEntireLine = true;
        }
    }

    public class CodeEditorInstructionPointer : CodeEditorBookmark 
    {
        public CodeEditorInstructionPointer(FastColoredTextBox textBox, TextStyle style)
            : base(textBox, new LDBookmark(new FilePath(""), 0, 0), style)
        {
            
        }
    }
}
