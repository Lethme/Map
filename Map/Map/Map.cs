using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Map
{
    public class Map<TKey, TData> : IEnumerable<MapItem<TKey, TData>> where TKey : IComparable<TKey>, IEquatable<TKey> where TData : IComparable<TData>, IEquatable<TData>
    {
        private List<MapItem<TKey, TData>> Items { get; } = new List<MapItem<TKey, TData>>();
        public int ItemCount => Items.Count;
        public int DataCount => this.Select(item => item.Data).Aggregate((x, y) => x.Concat(y).ToList()).Count;
        public IReadOnlyList<TKey> Keys => Items.Select(item => item.Key).ToList().AsReadOnly();
        public Map() { }
        public Map(params MapItem<TKey, TData>[] mapItems)
        {
            foreach (var item in mapItems) this.Add(item);
        }
        public void Sort(SortType sortType = SortType.Ascending)
        {
            SortItems(sortType);
            SortData(sortType);
        }
        public void SortItems(SortType sortType = SortType.Ascending)
        {
            switch (sortType)
            {
                case SortType.Ascending:
                    {
                        Items.Sort((x, y) => x.Key.CompareTo(y.Key));
                        return;
                    }
                case SortType.Descending:
                    {
                        Items.Sort((x, y) => y.Key.CompareTo(x.Key));
                        return;
                    }
                default: throw new ArgumentException(nameof(sortType));
            }
        }
        public void SortItems(Comparison<MapItem<TKey, TData>> comparison)
        {
            Items.Sort(comparison);
        }
        public void SortData(SortType sortType = SortType.Ascending)
        {
            switch (sortType)
            {
                case SortType.Ascending:
                    {
                        foreach (var item in Items)
                        {
                            item.Data.Sort((x, y) => x.CompareTo(y));
                        }
                        return;
                    }
                case SortType.Descending:
                    {
                        foreach (var item in Items)
                        {
                            item.Data.Sort((x, y) => y.CompareTo(x));
                        }
                        return;
                    }
                default: throw new ArgumentException(nameof(sortType));
            }
        }
        public void SortData(Comparison<TData> comparison)
        {
            foreach (var item in Items)
            {
                item.Data.Sort(comparison);
            }
        }
        public void Add(MapItem<TKey, TData> mapItem)
        {
            if (mapItem == null) throw new ArgumentNullException(nameof(mapItem));

            if (Items.Any(item => item.Key.Equals(mapItem.Key)))
            {
                throw new ArgumentException($"Map already contains item with key {mapItem.Key}.", nameof(mapItem));
            }

            Items.Add(mapItem);
        }
        public void Add(TKey key, TData data)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (data == null) throw new ArgumentNullException(nameof(data));
            
            if (Items.Any(item => item.Key.Equals(key)))
            {
                var item = Items.Find(item => item.Key.Equals(key));
                if (item.Data.Any(d => d.Equals(data)))
                {
                    throw new ArgumentException($"Item with key {key} already contains the same data.");
                }
                item.Add(data);
            }
            else
            {
                this.Add(MapItem.Create(key, data));
            }
        }
        public void Add(TKey key, params TData[] dataSequense)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (dataSequense == null) throw new ArgumentNullException(nameof(dataSequense));

            foreach (var data in dataSequense)
            {
                this.Add(key, data);
            }
        }
        public void Add(TKey key, IEnumerable<TData> dataCollection)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (dataCollection == null) throw new ArgumentNullException(nameof(dataCollection));

            foreach (var data in dataCollection)
            {
                this.Add(key, data);
            }
        }
        public bool Remove(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var item = GetItem(key);
            if (item != null)
            {
                Items.Remove(item);
                return true;
            }

            return false;
        }
        public int Remove(IEnumerable<TKey> keyCollection)
        {
            if (keyCollection == null) throw new ArgumentNullException(nameof(keyCollection));

            var count = 0;
            foreach (var key in keyCollection)
            {
                if (this.Remove(key)) count++;
            }

            return count;
        }
        public int Remove(params TKey[] keySequence)
        {
            if (keySequence == null) throw new ArgumentNullException(nameof(keySequence));

            var count = 0;
            foreach (var key in keySequence)
            {
                if (this.Remove(key)) count++;
            }

            return count;
        }
        public bool Remove(TKey key, TData data)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (data == null) throw new ArgumentNullException(nameof(data));

            var item = GetItem(key);
            if (item != null)
            {
                if (item.Remove(data) && item.Empty) this.Remove(key);
            }

            return false;
        }
        public int RemoveAll(TData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            var count = 0;
            foreach (var item in Items)
            {
                if (item.Remove(data))
                {
                    count++;
                    if (item.Empty) this.Remove(item.Key);
                }
            }

            return count;
        }
        public int RemoveAll(IEnumerable<TData> dataCollection)
        {
            if (dataCollection == null) throw new ArgumentNullException(nameof(dataCollection));

            var count = 0;
            foreach (var data in dataCollection)
            {
                count += this.RemoveAll(data);
            }
            
            return count;
        }
        public int RemoveAll(params TData[] dataSequence)
        {
            if (dataSequence == null) throw new ArgumentNullException(nameof(dataSequence));

            var count = 0;
            foreach (var data in dataSequence)
            {
                count += this.RemoveAll(data);
            }

            return count;
        }
        public bool Update(TKey key, TData currentData, TData newData)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (currentData == null) throw new ArgumentNullException(nameof(currentData));
            if (newData == null) throw new ArgumentNullException(nameof(newData));

            var item = GetItem(key);
            if (item != null)
            {
                return item.Update(currentData, newData);
            }

            return false;
        }
        public int Update(TData currentData, TData newData)
        {
            if (currentData == null) throw new ArgumentNullException(nameof(currentData));
            if (newData == null) throw new ArgumentNullException(nameof(newData));

            var items = GetItems(currentData);
            var count = 0;
            foreach (var item in items)
            {
                if (item.Update(currentData, newData)) count++;
            }

            return count;
        }
        public MapItem<TKey, TData> GetItem(TKey key) => Items.SingleOrDefault(item => item.Key.Equals(key));
        public List<MapItem<TKey, TData>> GetItems(TData data) => Items.Where(item => item.Data.Contains(data)).ToList();
        public MapItem<TKey, TData> Get(TKey key)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            var item = Items.SingleOrDefault(item => item.Key.Equals(key)) ??
                throw new ArgumentException($"Map doesn't contain any items with key {key}.", nameof(key));

            return item;
        }
        IEnumerator<MapItem<TKey, TData>> IEnumerable<MapItem<TKey, TData>>.GetEnumerator()
        {
            foreach (var item in Items) yield return item;
        }
        public IEnumerator GetEnumerator()
        {
            foreach (var item in Items) yield return item;
        }
        public override string ToString() => this.AsIndentedString("\n\n");
        public static Map<TKey, TData> operator +(Map<TKey, TData> map, MapItem<TKey, TData> mapItem)
        {
            map.Add(mapItem);
            return map;
        }
        public static Map<TKey, TData> operator +(Map<TKey, TData> map, (TKey key, TData data) item)
        {
            map.Add(item.key, item.data);
            return map;
        }
        public static Map<TKey, TData> operator +(Map<TKey, TData> map, (TKey key, IEnumerable<TData> dataList) item)
        {
            foreach (var data in item.dataList)
            {
                map.Add(item.key, data);
            }
            return map;
        }
        public static Map<TKey, TData> operator -(Map<TKey, TData> map, MapItem<TKey, TData> mapItem)
        {
            foreach (var data in mapItem.Data)
            {
                map.Remove(mapItem.Key, data);
            }

            return map;
        }
        public static Map<TKey, TData> operator -(Map<TKey, TData> map, (TKey key, TData data) item)
        {
            map.Remove(item.key, item.data);
            return map;
        }
        public static Map<TKey, TData> operator -(Map<TKey, TData> map, (TKey key, IEnumerable<TData> dataList) item)
        {
            foreach (var data in item.dataList)
            {
                map.Remove(item.key, data);
            }
            return map;
        }
    }
    public static class Map
    {
        public static Map<TKey, TData> Create<TKey, TData>() where TKey : IComparable<TKey>, IEquatable<TKey> where TData : IComparable<TData>, IEquatable<TData> => new Map<TKey, TData>();
        public static Map<TKey, TData> Create<TKey, TData>(params MapItem<TKey, TData>[] mapItems) where TKey : IComparable<TKey>, IEquatable<TKey> where TData : IComparable<TData>, IEquatable<TData> => new Map<TKey, TData>(mapItems);
    }
}
