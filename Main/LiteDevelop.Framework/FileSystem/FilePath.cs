using System;
using System.IO;
using System.Linq;

namespace LiteDevelop.Framework.FileSystem
{
    /// <summary>
    /// Represents a path to a file or directory.
    /// </summary>
    public class FilePath : IComparable<FilePath>, IComparable<string>
    {
        public FilePath(string path)
            : this(string.Empty, path)
        {
        }

        public FilePath(Project project, string hintPath)
            : this(project.ProjectDirectory, hintPath)
        {
        }

        public FilePath(FilePath directory, string hintPath)
            : this(directory.FullPath, hintPath)
        {
        }

        public FilePath(string directory, string hintPath)
        {
        	if (!string.IsNullOrEmpty(directory))
            {
        		if (string.IsNullOrEmpty(hintPath))
        			FullPath = directory;
        		else 
                	FullPath = Uri.UnescapeDataString(new Uri(new Uri(directory + Path.DirectorySeparatorChar), hintPath).LocalPath);
            }
            else
            {
                FullPath = hintPath;
            }
        }

        /// <summary>
        /// Gets the file or directory name.
        /// </summary>
        public string FileName 
        { 
            get
            {
                return Path.GetFileNameWithoutExtension(FullPath); 
            }
        }

        /// <summary>
        /// Gets the full path to the file or directory.
        /// </summary>
        public string FullPath 
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the path has an extension or not.
        /// </summary>
        public bool HasExtension 
        { 
            get
            {
                return Path.HasExtension(FullPath);
            }
        }

        /// <summary>
        /// Gets the extension of the file path.
        /// </summary>
        public string Extension 
        {
            get
            { 
                return Path.GetExtension(FullPath); 
            } 
        }

        /// <summary>
        /// Gets the parent directory.
        /// </summary>
        public FilePath ParentDirectory 
        { 
            get 
            { 
                return new FilePath(Path.GetDirectoryName(FullPath)); 
            } 
        }

        /// <summary>
        /// Changes the extension of the file path and creates a new instance of <see cref="LiteDevelop.Framework.FileSystem.FilePath" /> using the new path.
        /// </summary>
        public FilePath ChangeExtension(string extension)
        {
            return new FilePath(Path.ChangeExtension(FullPath, extension));
        }

        /// <summary>
        /// Combines paths and creates a new instance of <see cref="LiteDevelop.Framework.FileSystem.FilePath" /> using the new path.
        /// </summary>
        public FilePath Combine(params string[] subfolders)
        {
            Array.Resize(ref subfolders, subfolders.Length + 1);
            Array.Copy(subfolders, 0, subfolders, 1, subfolders.Length - 1);
            subfolders[0] = FullPath;
            return new FilePath(Path.Combine(subfolders));
        }

        /// <summary>
        /// Changes the name and creates a new instance of <see cref="LiteDevelop.Framework.FileSystem.FilePath" /> using the new path.
        /// </summary>
        public FilePath ChangeName(string newName)
        {
            return this.ParentDirectory.Combine(newName);
        }

        /// <summary>
        /// Gets the hint path in string format relative to a project.
        /// </summary>
        public string GetRelativePath(Project project)
        {
            if (project == null)
                return FullPath;
            return GetRelativePath(project.ProjectDirectory);
        }

        /// <summary>
        /// Gets the hint path in string format relative to a directory.
        /// </summary>
        public string GetRelativePath(FilePath directory)
        {
            if (directory == null)
                return FullPath;

            return GetRelativePath(directory.FullPath);
        }

        /// <summary>
        /// Gets the hint path in string format relative to a directory.
        /// </summary>
        public string GetRelativePath(string directory)
        {
            if (string.IsNullOrWhiteSpace(directory))
            {
                return FullPath;
            }
            var uri = new Uri(directory + System.IO.Path.DirectorySeparatorChar).MakeRelativeUri(new Uri(FullPath + System.IO.Path.DirectorySeparatorChar));
            string path = System.Web.HttpUtility.UrlDecode(uri.ToString()).Replace('/', System.IO.Path.DirectorySeparatorChar);
            if (path.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
            	path = path.Remove(path.Length - 1);
            return path;
        }

        public bool Equals(FilePath path)
        {
            if (path == null)
                return false;
            return Equals(path.FullPath);
        }
        
        public bool Equals(string path)
        {
            if (path == null)
                return false;
            return FullPath.Equals(path, StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is FilePath)
                return Equals(obj as FilePath);
            if (obj is string)
                return Equals(obj as string);
            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return FullPath.GetHashCode();
        }

        public static bool operator ==(FilePath a, FilePath b)
        {
            if ((object)a == null)
            {
                return (object)b == null;
            }

            return a.Equals(b);
        }

        public static bool operator !=(FilePath a, FilePath b)
        {
            return !(a == b);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return FullPath;
        }

        #region IComparable<FilePath> Members

        /// <inheritdoc />
        public int CompareTo(FilePath other)
        {
            return string.Compare(FullPath, other.FullPath, true);
        }

        #endregion

        #region IComparable<string> Members

        /// <inheritdoc />
        public int CompareTo(string other)
        {
            return string.Compare(FullPath, other, true);
        }

        #endregion
    }
}
