using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using System.IO;

namespace LiteDevelop.Framework
{
    /// <summary>
    /// Provides icons for members (e.g. classes, methods, fields) that may appear in several source codes.
    /// </summary>
    public class AssemblyIconProvider : IconProvider, IDisposable 
    {
        public const int Index_Namespace = 0;
        public const int Index_Class = 6;
        public const int Index_Interface = 12;
        public const int Index_Structure = 18;
        public const int Index_Enum = 24;
        public const int Index_Delegate = 30;
        public const int Index_Text = 41;
        public const int Index_Constructor = 48;
        public const int Index_Method = 60;
        public const int Index_Field = 84;
        public const int Index_Constant = 96;
        public const int Index_Property = 102;
        public const int Index_Event = 138;
        public const int Index_Assembly = 150;
        public const int Index_AssemblyRef = 151;
        public const int Index_Module = 152;
        public const int Index_ModuleRef = 153;
        public const int Index_ReferenceDirectory = 154;
        public const int Index_Directory = 155;
        public const int Index_Resource = 157;
        public const int Index_File = 160;
        public const int Index_Message = 164;
        public const int Index_Error = 165;
        public const int Index_Wait = 166;

        private ImageList _imageList = new ImageList();
        private List<int> _initializedIndices = new List<int>();
        private Bitmap _iconTable;

        /// <inheritdoc />
        public override ImageList ImageList 
        {
            get
            {
                return _imageList; 
            }
        }

        public AssemblyIconProvider()
        {
            _iconTable = Properties.Resources.browserIcons;
            _imageList.ColorDepth = ColorDepth.Depth32Bit;
            _imageList.TransparentColor = Color.Green;

            for (int i = 1; i <= _iconTable.Width / 16; i++)
                _imageList.Images.Add(IconProvider.GetIconFromSpriteTable(_iconTable, new Size(16, 16), i));
        }

        /// <inheritdoc />
        public override int GetImageIndex(object member)
        {
            int index = Index_Namespace;
            var type = (member == null ? typeof(object) : (member is Type ? member as Type : member.GetType()));
            
            if (type.IsBasedOn(typeof(ConstructorInfo)))
                index = Index_Constructor;
            else if (type.IsBasedOn(typeof(MethodInfo)))
                index = Index_Method;
            else if (type.IsBasedOn(typeof(FieldInfo)))
                index = Index_Field;
            else if (type.IsBasedOn(typeof(EventInfo)))
                index = Index_Event;
            else if (type.IsBasedOn(typeof(PropertyInfo)))
                index = Index_Property;
            else if (member is string || type.IsBasedOn(typeof(FileInfo)))
                index = Index_File;
            else if (type.IsBasedOn(typeof(DirectoryInfo)))
                index = Index_Directory;
            else if (member is Type)
            {
                if (type.IsEnum)
                    index = Index_Enum;
                else if (type.IsValueType)
                    index = Index_Structure;
                else
                    index = Index_Class;
            }


            return index;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            ImageList.Dispose();
        }
    }
}
