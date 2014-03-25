using System;
using System.Linq;
using System.Windows.Forms;

namespace LiteDevelop.Framework.Gui
{
    public abstract class LiteViewContent : IDisposable
    {
        public event FormClosingEventHandler Closing;
        public event FormClosedEventHandler Closed;
        public event EventHandler TextChanged;
        public event EventHandler ControlChanged;
        public event DragEventHandler DragEnter;
        public event DragEventHandler DragDrop;

        private string _text = string.Empty;
        private Control _control;

        public LiteViewContent()
        {
            UseDefaultFileDrop = true;
        }

        /// <summary>
        /// Gets or sets the title of the view content
        /// </summary>
        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnTextChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets or sets the control that works as a container for the view content.
        /// </summary>
        public Control Control
        {
            get
            {
                return _control;
            }
            set
            {
                if (_control != value)
                {
                    _control = value;
                    OnControlChanged(EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether LiteDevelop should handle file drag-drop events on the control.
        /// </summary>
        public bool UseDefaultFileDrop
        {
            get;
            set;
        }

        /// <summary>
        /// Requests the view content to close.
        /// </summary>
        public void Close()
        {
            Close(false);
        }

        /// <summary>
        /// Requests or forces the view content to close.
        /// </summary>
        public void Close(bool forced)
        {
            var args = new FormClosingEventArgs(CloseReason.None, false);
            OnClosing(args);
            if (forced || !args.Cancel)
            {
                if (Closed != null)
                    Closed(this, new FormClosedEventArgs(CloseReason.None));
                Dispose();
            }
        }

        /// <inheritdoc />
        public virtual void Dispose()
        {
        }

        /// <summary>
        /// Dispatches a drag enter event.
        /// </summary>
        /// <param name="e">The event arguments to dispatch.</param>
        public void DispatchDragEnterEvent(DragEventArgs e)
        {
            if (DragEnter != null)
                DragEnter(this, e);
        }

        /// <summary>
        /// Dispatches a drag drop event.
        /// </summary>
        /// <param name="e">The event arguments to dispatch.</param>
        public void DispatchDragDropEvent(DragEventArgs e)
        {
            if (DragDrop != null)
                DragDrop(this, e);
        }

        protected virtual void OnTextChanged(EventArgs e)
        {
            if (TextChanged != null)
                TextChanged(this, e);
        }

        protected virtual void OnControlChanged(EventArgs e)
        {
            if (Control != null)
            {
                Control.AllowDrop = true;
                Control.DragEnter += Control_DragEnter;
                Control.DragDrop += Control_DragDrop;
            }

            if (ControlChanged != null)
                ControlChanged(this, e);
        }

        private void Control_DragEnter(object sender, DragEventArgs e)
        {
            DispatchDragEnterEvent(e);
        }

        private void Control_DragDrop(object sender, DragEventArgs e)
        {
            DispatchDragDropEvent(e);
        }

        protected virtual void OnClosing(FormClosingEventArgs e)
        {
            if (Closing != null)
                Closing(this, e);
        }

    }
}
