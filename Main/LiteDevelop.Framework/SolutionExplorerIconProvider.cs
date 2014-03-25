using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Framework
{
    public class SolutionExplorerIconProvider : IconProvider
    {
        public const int Index_Directory = 0;
        public const int Index_Solution = 1;
        public const int Index_Project = 2;
        public const int Index_ReferencesDirectory = 3;
        public const int Index_AssemblyRef = 4;
        public const int Index_Properties = 5;

        private readonly ImageList _imageList;
        private readonly List<string> _cachedExtensions;
        private readonly int _startIndex;

        public SolutionExplorerIconProvider()
        {
            _imageList = new ImageList();
            _imageList.ColorDepth = ColorDepth.Depth32Bit;
            _imageList.TransparentColor = Color.Green;

            _cachedExtensions = new List<string>();

            var iconTable = Properties.Resources.browserIcons;
            _imageList.Images.Add(IconProvider.GetIconFromSpriteTable(iconTable, new Size(16, 16), AssemblyIconProvider.Index_Directory + 1));
            _imageList.Images.Add(Properties.Resources.solution);
            _imageList.Images.Add(Properties.Resources.project);
            _imageList.Images.Add(IconProvider.GetIconFromSpriteTable(iconTable, new Size(16, 16), AssemblyIconProvider.Index_ReferenceDirectory + 1));
            _imageList.Images.Add(IconProvider.GetIconFromSpriteTable(iconTable, new Size(16, 16), AssemblyIconProvider.Index_AssemblyRef + 1));
            _imageList.Images.Add(Properties.Resources.config);
            _startIndex = 6;
        }

        /// <inheritdoc />
        public override ImageList ImageList
        {
            get { return _imageList; }
        }

        /// <inheritdoc />
        public override int GetImageIndex(object member)
        {
            if (member is Solution)
                return Index_Solution;
            if (member is ProjectEntry || member is Project)
                return Index_Project;
            if (member is SolutionFolder)
                return Index_Directory;

            string filePath = member as string;
            if (member is FileInfo)
            {
                filePath = (member as FileInfo).FullName;
            }
            else if (member is IFilePathProvider)
            {
                filePath = (member as IFilePathProvider).FilePath.FullPath;
            }
            else if (member is DirectoryInfo || member is SolutionFolder)
            {
                return Index_Directory;
            }
            
            string extension = Path.GetExtension(filePath);
            int index = _cachedExtensions.IndexOf(extension);
            if (index == -1)
            {
                if (extension == ".exe" || extension == ".dll")
                    return Index_AssemblyRef;

                if (File.Exists(filePath))
                {
                    _imageList.Images.Add(Icon.ExtractAssociatedIcon(filePath).ToBitmap());
                    index = _cachedExtensions.Count;
                    _cachedExtensions.Add(extension);
                }
                else
                {
                    index = -1;
                }
            }
            return index + _startIndex;
        }
    }
}
