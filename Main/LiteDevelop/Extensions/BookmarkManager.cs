using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LiteDevelop.Framework;
using LiteDevelop.Framework.Extensions;
using LiteDevelop.Framework.FileSystem;

namespace LiteDevelop.Extensions
{
    public class BookmarkManager : IBookmarkManager
    {
        public BookmarkManager()
        {
            Bookmarks = new EventBasedCollection<Bookmark>();
        }

        #region IBookmarkManager Members

        public EventBasedCollection<Bookmark> Bookmarks
        {
            get;
            private set;
        }

        public IEnumerable<TBookmark> GetBookmarks<TBookmark>() where TBookmark : Bookmark
        {
            foreach (var item in Bookmarks.Where(x => x is TBookmark))
                yield return item as TBookmark;
        }

        public IEnumerable<Bookmark> GetBookmarks(FilePath file)
        {
            return Bookmarks.Where(x => x.FilePath == file);
        }

        public void ClearBookmarksFromFile(FilePath path)
        {
            RemoveBookmarks(x => x.FilePath == path);
        }

        public void ClearBookmarksOnLine(FilePath path, int line)
        {
            RemoveBookmarks(x => x.FilePath == path && x.Line == line);
        }

        #endregion

        private void RemoveBookmarks(Predicate<Bookmark> predicate)
        {
            int index = 0;
            while (index < Bookmarks.Count)
            {
                if (predicate(Bookmarks[index]))
                {
                    Bookmarks.RemoveAt(index);
                    index--;
                }
                index++;
            }
        }
    }
}
