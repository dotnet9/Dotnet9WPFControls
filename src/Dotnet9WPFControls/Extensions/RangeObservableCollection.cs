using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace System.Collections.ObjectModel
{
    /// <summary>
    ///     ObservableCollection扩展类，增加AddRange方法
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    public class RangeObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        ///     批量添加数据
        /// </summary>
        /// <param name="collection"></param>
        public void AddRange(IEnumerable<T> collection)
        {
            foreach (T item in collection)
            {
                Items.Add(item);
            }

            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }

    /// <summary>
    ///     参考文章：https://cloud.tencent.com/developer/ask/sof/71776
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RangeCollection<T> : ObservableCollection<T>
    {
        #region Members

        /// <summary>
        ///     Occurs when a single item is added.
        /// </summary>
        public event EventHandler<ItemAddedEventArgs<T>>? ItemAdded;

        /// <summary>
        ///     Occurs when a single item is inserted.
        /// </summary>
        public event EventHandler<ItemInsertedEventArgs<T>>? ItemInserted;

        /// <summary>
        ///     Occurs when a single item is removed.
        /// </summary>
        public event EventHandler<ItemRemovedEventArgs<T>>? ItemRemoved;

        /// <summary>
        ///     Occurs when a single item is replaced.
        /// </summary>
        public event EventHandler<ItemReplacedEventArgs<T>>? ItemReplaced;

        /// <summary>
        ///     Occurs when items are added to this.
        /// </summary>
        public event EventHandler<ItemsAddedEventArgs<T>>? ItemsAdded;

        /// <summary>
        ///     Occurs when items are removed from this.
        /// </summary>
        public event EventHandler<ItemsRemovedEventArgs<T>>? ItemsRemoved;

        /// <summary>
        ///     Occurs when items are replaced within this.
        /// </summary>
        public event EventHandler<ItemsReplacedEventArgs<T>>? ItemsReplaced;

        /// <summary>
        ///     Occurs when entire collection is cleared.
        /// </summary>
        public event EventHandler<ItemsClearedEventArgs<T>>? ItemsCleared;

        /// <summary>
        ///     Occurs when entire collection is replaced.
        /// </summary>
        public event EventHandler<CollectionReplacedEventArgs<T>>? CollectionReplaced;

        #endregion

        #region Helper Methods

        /// <summary>
        ///     Throws exception if any of the specified objects are null.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        private void Check(params T[] items)
        {
            if (items.Any(item => item == null))
            {
                throw new ArgumentNullException("Item cannot be null.");
            }
        }

        private void Check(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("Items cannot be null.");
            }
        }

        private void Check(IEnumerable<IEnumerable<T>> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException("Items cannot be null.");
            }
        }

        private void RaiseChanged(NotifyCollectionChangedAction action)
        {
            OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            OnPropertyChanged(new PropertyChangedEventArgs("Item[]"));
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        #endregion

        #region Bulk Methods

        /// <summary>
        ///     Adds the elements of the specified collection to the end of this.
        /// </summary>
        public void AddRange(IEnumerable<T> newItems)
        {
            Check(newItems);
            foreach (T i in newItems)
            {
                Items.Add(i);
            }

            RaiseChanged(NotifyCollectionChangedAction.Reset);
            OnItemsAdded(new ItemsAddedEventArgs<T>(newItems));
        }

        /// <summary>
        ///     Adds variable IEnumerable<T> to this.
        /// </summary>
        /// <param name="List"></param>
        public void AddRange(params IEnumerable<T>[] newItems)
        {
            Check(newItems);
            foreach (IEnumerable<T> items in newItems)
            foreach (T item in items)
            {
                Items.Add(item);
            }

            RaiseChanged(NotifyCollectionChangedAction.Reset);
            //TO-DO: Raise OnItemsAdded with combined IEnumerable<T>.
        }

        /// <summary>
        ///     Removes the first occurence of each item in the specified collection.
        /// </summary>
        public void Remove(IEnumerable<T> oldItems)
        {
            Check(oldItems);
            foreach (T i in oldItems)
            {
                Items.Remove(i);
            }

            RaiseChanged(NotifyCollectionChangedAction.Reset);
            OnItemsRemoved(new ItemsRemovedEventArgs<T>(oldItems));
        }

        /// <summary>
        ///     Removes all occurences of each item in the specified collection.
        /// </summary>
        /// <param name="itemsToRemove"></param>
        public void RemoveAll(IEnumerable<T> oldItems)
        {
            Check(oldItems);
            HashSet<T> set = new HashSet<T>(oldItems);
            List<T>? list = this as List<T>;
            int i = 0;
            while (i < Count)
            {
                if (set.Contains(this[i]))
                {
                    RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }

            RaiseChanged(NotifyCollectionChangedAction.Reset);
            OnItemsRemoved(new ItemsRemovedEventArgs<T>(oldItems));
        }

        /// <summary>
        ///     Replaces all occurences of a single item with specified item.
        /// </summary>
        public void ReplaceAll(T old, T @new)
        {
            Check(old, @new);
            Replace(old, @new, false);
            RaiseChanged(NotifyCollectionChangedAction.Reset);
            OnItemReplaced(new ItemReplacedEventArgs<T>(old, @new));
        }

        /// <summary>
        ///     Clears this and adds specified collection.
        /// </summary>
        public void ReplaceCollection(IEnumerable<T> newItems, bool supressEvent = false)
        {
            Check(newItems);
            IEnumerable<T> oldItems = new List<T>(Items);
            Items.Clear();
            foreach (T item in newItems)
            {
                Items.Add(item);
            }

            RaiseChanged(NotifyCollectionChangedAction.Reset);
            OnReplaced(new CollectionReplacedEventArgs<T>(oldItems, newItems));
        }

        private void Replace(T old, T @new, bool breakFirst)
        {
            List<T> Cloned = new(Items);
            int i = 0;
            foreach (T item in Cloned)
            {
                if (item.Equals(old))
                {
                    Items.Remove(item);
                    Items.Insert(i, @new);
                    if (breakFirst)
                    {
                        break;
                    }
                }

                i++;
            }
        }

        /// <summary>
        ///     Replaces the first occurence of a single item with specified item.
        /// </summary>
        public void Replace(T old, T @new)
        {
            Check(old, @new);
            Replace(old, @new, true);
            RaiseChanged(NotifyCollectionChangedAction.Reset);
            OnItemReplaced(new ItemReplacedEventArgs<T>(old, @new));
        }

        #endregion

        #region New Methods

        /// <summary>
        ///     Removes a single item.
        /// </summary>
        /// <param name="item"></param>
        public new void Remove(T item)
        {
            Check(item);
            base.Remove(item);
            OnItemRemoved(new ItemRemovedEventArgs<T>(item));
        }

        /// <summary>
        ///     Removes a single item at specified index.
        /// </summary>
        /// <param name="i"></param>
        public new void RemoveAt(int i)
        {
            T oldItem = Items[i]; //This will throw first if null
            base.RemoveAt(i);
            OnItemRemoved(new ItemRemovedEventArgs<T>(oldItem));
        }

        /// <summary>
        ///     Clears this.
        /// </summary>
        public new void Clear()
        {
            IEnumerable<T> oldItems = new List<T>(Items);
            Items.Clear();
            RaiseChanged(NotifyCollectionChangedAction.Reset);
            OnCleared(new ItemsClearedEventArgs<T>(oldItems));
        }

        /// <summary>
        ///     Adds a single item to end of this.
        /// </summary>
        /// <param name="t"></param>
        public new void Add(T item)
        {
            Check(item);
            base.Add(item);
            OnItemAdded(new ItemAddedEventArgs<T>(item));
        }

        /// <summary>
        ///     Inserts a single item at specified index.
        /// </summary>
        /// <param name="i"></param>
        /// <param name="t"></param>
        public new void Insert(int i, T item)
        {
            Check(item);
            base.Insert(i, item);
            OnItemInserted(new ItemInsertedEventArgs<T>(item, i));
        }

        /// <summary>
        ///     Returns list of T.ToString().
        /// </summary>
        /// <returns></returns>
        public new IEnumerable<string> ToString()
        {
            foreach (T Item in this)
            {
                yield return Item.ToString();
            }
        }

        #endregion

        #region Event Methods

        private void OnItemAdded(ItemAddedEventArgs<T> i)
        {
            if (ItemAdded != null)
            {
                ItemAdded(this, new ItemAddedEventArgs<T>(i.NewItem));
            }
        }

        private void OnItemInserted(ItemInsertedEventArgs<T> i)
        {
            if (ItemInserted != null)
            {
                ItemInserted(this, new ItemInsertedEventArgs<T>(i.NewItem, i.Index));
            }
        }

        private void OnItemRemoved(ItemRemovedEventArgs<T> i)
        {
            if (ItemRemoved != null)
            {
                ItemRemoved(this, new ItemRemovedEventArgs<T>(i.OldItem));
            }
        }

        private void OnItemReplaced(ItemReplacedEventArgs<T> i)
        {
            if (ItemReplaced != null)
            {
                ItemReplaced(this, new ItemReplacedEventArgs<T>(i.OldItem, i.NewItem));
            }
        }

        private void OnItemsAdded(ItemsAddedEventArgs<T> i)
        {
            if (ItemsAdded != null)
            {
                ItemsAdded(this, new ItemsAddedEventArgs<T>(i.NewItems));
            }
        }

        private void OnItemsRemoved(ItemsRemovedEventArgs<T> i)
        {
            if (ItemsRemoved != null)
            {
                ItemsRemoved(this, new ItemsRemovedEventArgs<T>(i.OldItems));
            }
        }

        private void OnItemsReplaced(ItemsReplacedEventArgs<T> i)
        {
            if (ItemsReplaced != null)
            {
                ItemsReplaced(this, new ItemsReplacedEventArgs<T>(i.OldItems, i.NewItems));
            }
        }

        private void OnCleared(ItemsClearedEventArgs<T> i)
        {
            if (ItemsCleared != null)
            {
                ItemsCleared(this, new ItemsClearedEventArgs<T>(i.OldItems));
            }
        }

        private void OnReplaced(CollectionReplacedEventArgs<T> i)
        {
            if (CollectionReplaced != null)
            {
                CollectionReplaced(this, new CollectionReplacedEventArgs<T>(i.OldItems, i.NewItems));
            }
        }

        #endregion

        #region RangeCollection

        /// <summary>
        ///     Initializes a new instance.
        /// </summary>
        public RangeCollection()
        {
        }

        /// <summary>
        ///     Initializes a new instance from specified enumerable.
        /// </summary>
        public RangeCollection(IEnumerable<T> collection) : base(collection) { }

        /// <summary>
        ///     Initializes a new instance from specified list.
        /// </summary>
        public RangeCollection(List<T> list) : base(list) { }

        /// <summary>
        ///     Initializes a new instance with variable T.
        /// </summary>
        public RangeCollection(params T[] items)
        {
            AddRange(items);
        }

        /// <summary>
        ///     Initializes a new instance with variable enumerable.
        /// </summary>
        public RangeCollection(params IEnumerable<T>[] items)
        {
            AddRange(items);
        }

        #endregion
    }

    public class CollectionReplacedEventArgs<T> : ReplacedEventArgs<T>
    {
        public CollectionReplacedEventArgs(IEnumerable<T> old, IEnumerable<T> @new) : base(old, @new) { }
    }

    public class ItemAddedEventArgs<T> : EventArgs
    {
        public T NewItem;

        public ItemAddedEventArgs(T t)
        {
            NewItem = t;
        }
    }

    public class ItemInsertedEventArgs<T> : EventArgs
    {
        public int Index;
        public T NewItem;

        public ItemInsertedEventArgs(T t, int i)
        {
            NewItem = t;
            Index = i;
        }
    }

    public class ItemRemovedEventArgs<T> : EventArgs
    {
        public T OldItem;

        public ItemRemovedEventArgs(T t)
        {
            OldItem = t;
        }
    }

    public class ItemReplacedEventArgs<T> : EventArgs
    {
        public T NewItem;
        public T OldItem;

        public ItemReplacedEventArgs(T old, T @new)
        {
            OldItem = old;
            NewItem = @new;
        }
    }

    public class ItemsAddedEventArgs<T> : EventArgs
    {
        public IEnumerable<T> NewItems;

        public ItemsAddedEventArgs(IEnumerable<T> t)
        {
            NewItems = t;
        }
    }

    public class ItemsClearedEventArgs<T> : RemovedEventArgs<T>
    {
        public ItemsClearedEventArgs(IEnumerable<T> old) : base(old) { }
    }

    public class ItemsRemovedEventArgs<T> : RemovedEventArgs<T>
    {
        public ItemsRemovedEventArgs(IEnumerable<T> old) : base(old) { }
    }

    public class ItemsReplacedEventArgs<T> : ReplacedEventArgs<T>
    {
        public ItemsReplacedEventArgs(IEnumerable<T> old, IEnumerable<T> @new) : base(old, @new) { }
    }

    public class RemovedEventArgs<T> : EventArgs
    {
        public IEnumerable<T> OldItems;

        public RemovedEventArgs(IEnumerable<T> old)
        {
            OldItems = old;
        }
    }

    public class ReplacedEventArgs<T> : EventArgs
    {
        public IEnumerable<T> NewItems;
        public IEnumerable<T> OldItems;

        public ReplacedEventArgs(IEnumerable<T> old, IEnumerable<T> @new)
        {
            OldItems = old;
            NewItems = @new;
        }
    }
}