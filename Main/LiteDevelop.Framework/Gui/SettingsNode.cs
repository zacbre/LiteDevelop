using System;
using System.Linq;
using System.Windows.Forms;

namespace LiteDevelop.Framework.Gui
{
    /// <summary>
    /// A special tree node used in LiteDevelop's settings dialog.
    /// </summary>
    public class SettingsNode : TreeNode
    {
        public event EventHandler EditorControlChanged;
        private SettingsControl _editorControl;

        public SettingsNode(string title, SettingsControl editorControl)
            : base(title)
        {
            EditorControl = editorControl;
        }

        /// <summary>
        /// The editor control associated with this tree node.
        /// </summary>
        public SettingsControl EditorControl
        {
            get { return _editorControl; }
            set
            {
                if (_editorControl != value)
                {
                    _editorControl = value;
                    OnEditorControlChanged(EventArgs.Empty);
                }
            }
        }

        protected virtual void OnEditorControlChanged(EventArgs e)
        {
            if (EditorControlChanged != null)
                EditorControlChanged(this, e);
        }

        /// <summary>
        /// Applies all settings defined in the settings node and its sub nodes.
        /// </summary>
        public void ApplySettingsInAllNodes()
        {
            ApplySettingsInAllNodes(this);
        }

        /// <summary>
        /// Loads all user defined presets in the settings node and its sub nodes.
        /// </summary>
        public void LoadUserDefinedPresetsInAllNodes()
        {
            LoadUserDefinedPresetsInAllNodes(this);
        }

        /// <summary>
        /// Applies all settings defined in the settings node and its sub nodes.
        /// </summary>
        /// <param name="node">The node to start with to apply all settings.</param>
        public static void ApplySettingsInAllNodes(SettingsNode node)
        {
            if (node.EditorControl != null)
                node.EditorControl.ApplySettings();

            foreach (TreeNode subNode in node.Nodes)
            {
                if (subNode is SettingsNode)
                    ApplySettingsInAllNodes(subNode as SettingsNode);
            }
        }

        /// <summary>
        /// Loads all user defined presets in the settings node and its sub nodes.
        /// </summary>
        /// <param name="node">The node to start with to load all user defined presets.</param>
        public static void LoadUserDefinedPresetsInAllNodes(SettingsNode node)
        {
            if (node.EditorControl != null)
                node.EditorControl.LoadUserDefinedPresets();

            foreach (TreeNode subNode in node.Nodes)
            {
                if (subNode is SettingsNode)
                    LoadUserDefinedPresetsInAllNodes(subNode as SettingsNode);
            }
        }

    }
    
    /// <summary>
    /// A control used by the settings dialog in LiteDevelop.
    /// </summary>
    public class SettingsControl : UserControl
    {
        /// <summary>
        /// Applies all settings specified in the control.
        /// </summary>
        public virtual void ApplySettings() 
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads all user defined presets into the control.
        /// </summary>
        public virtual void LoadUserDefinedPresets()
        {
            throw new NotImplementedException();
        }
    }
}
