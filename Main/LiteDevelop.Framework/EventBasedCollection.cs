using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LiteDevelop.Framework
{
    /// <summary>
    /// Represents a strongly typed event-driven collection of objects that can be accessed by an index. Provides events which occur after modifying the collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public class EventBasedCollection<T> : ICollection<T>, IList<T>, IEnumerable<T>
    {
        public event CollectionChangingEventHandler InsertingItem;
        public event CollectionChangeEventHandler InsertedItem;
        public event CollectionChangingEventHandler RemovingItem;
        public event CollectionChangeEventHandler RemovedItem;
        public event EventHandler ClearedCollection;

        private readonly List<T> _collection;

        public EventBasedCollection()
        {
            _collection = new List<T>();
        }

        /// <inheritdoc />
        public void Add(T item)
        {
            Insert(this.Count, item);
        }

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (var item in collection)
                Add(item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            while (Count != 0)
                this.Remove(this[0]);

            if (ClearedCollection != null)
                ClearedCollection(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            return _collection.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            _collection.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public int Count
        {
            get { return _collection.Count; }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <inheritdoc />
        public bool Remove(T item)
        {
            int index = this.IndexOf(item);
            if (index == -1)
                return false;

            this.RemoveAt(index);

            return true;
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        /// <inheritdoc />
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        /// <inheritdoc />
        public int IndexOf(T item)
        {
            return _collection.IndexOf(item);
        }

        /// <inheritdoc />
        public void Insert(int index, T item)
        {
            var eventArgs = new CollectionChangingEventArgs(item, index);
            OnInsertingItem(eventArgs);
            if (!eventArgs.Cancel)
            {
                _collection.Insert(index, item);
                OnInsertedItem(new CollectionChangedEventArgs(item, index));
            }
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            T item = _collection[index];
            var eventArgs = new CollectionChangingEventArgs(item, index);
            OnRemovingItem(eventArgs);
            if (!eventArgs.Cancel)
            {
                _collection.RemoveAt(index);
                OnRemovedItem(new CollectionChangedEventArgs(item, index));
            }
        }

        /// <inheritdoc />
        public T this[int index]
        {
            get
            {
                return _collection[index];
            }
            set
            {
                _collection[index] = value;
            }
        }

        /// <summary>
        /// Copies the collection elements to a read-only collection.
        /// </summary>
        /// <returns>An instance of a <see cref="System.Collections.ObjectModel.ReadOnlyCollection{T}"/> holding the same elements as the source.</returns>
        public ReadOnlyCollection<T> AsReadOnly()
        {
            return _collection.AsReadOnly();
        }

        protected virtual void OnInsertingItem(CollectionChangingEventArgs e)
        {
            if (InsertingItem != null)
                InsertingItem(this, e);
        }

        protected virtual void OnInsertedItem(CollectionChangedEventArgs e)
        {
            if (InsertedItem != null)
                InsertedItem(this, e);
        }

        protected virtual void OnRemovingItem(CollectionChangingEventArgs e)
        {
            if (RemovingItem != null)
                RemovingItem(this, e);
        }

        protected virtual void OnRemovedItem(CollectionChangedEventArgs e)
        {
            if (RemovedItem != null)
                RemovedItem(this, e);
        }
    }

    public delegate void CollectionChangeEventHandler(object sender, CollectionChangedEventArgs e);

    public class CollectionChangedEventArgs : EventArgs
    {
        public CollectionChangedEventArgs(object obj, int index)
        {
            this.TargetObject = obj;
            this.TargetIndex = index;
        }

        public object TargetObject
        {
            get;
            private set;
        }

        public int TargetIndex
        {
            get;
            private set;
        }
    }

    public delegate void CollectionChangingEventHandler(object sender, CollectionChangingEventArgs e);

    public class CollectionChangingEventArgs : CollectionChangedEventArgs
    {
        public CollectionChangingEventArgs(object obj, int index)
            : base(obj, index)
        {
        }

        public bool Cancel
        { 
            get;
            set; 
        }
    }

}
