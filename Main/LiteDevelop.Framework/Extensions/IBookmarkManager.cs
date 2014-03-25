using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Framework.Extensions
{
    public interface IBookmarkManager
    {
        EventBasedCollection<Bookmark> Bookmarks { get; }

        IEnumerable<TBookmark> GetBookmarks<TBookmark>() where TBookmark : Bookmark;

        IEnumerable<Bookmark> GetBookmarks(FilePath file);

        void ClearBookmarksFromFile(FilePath path);

        void ClearBookmarksOnLine(FilePath path, int line);
    }
}
