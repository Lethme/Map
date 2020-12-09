using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Map
{
    public class MapItem<TKey, TData> : IComparable<MapItem<TKey, TData>> where TKey : IComparable<TKey>, IEquatable<TKey> where TData : IComparable<TData>, IEquatable<TData>
    {
        public TKey Key { get; set; }
        public List<TData> Data { get; private set; } = new List<TData>();
        public int Count => Data.Count;
        public bool Empty => Data.Count == 0;
        public MapItem() { }
        public MapItem(TKey key, TData data)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (data == null) throw new ArgumentNullException(nameof(data));
            
            Key = key;
            Data.Add(data);
        }
        public MapItem(TKey key, IEnumerable<TData> dataCollection)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (dataCollection == null) throw new ArgumentNullException(nameof(dataCollection));

            Key = key;
            Data.AddRange(dataCollection);
        }
        public MapItem(TKey key, params TData[] dataSequence)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (dataSequence == null) throw new ArgumentNullException(nameof(dataSequence));

            Key = key;
            Data.AddRange(dataSequence);
        }
        public void Add(TData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (Data.Contains(data)) throw new ArgumentException($"Current item already contains the same data.");
            Data.Add(data);
        }
        public void Add(IEnumerable<TData> dataCollection)
        {
            if (dataCollection == null) throw new ArgumentNullException(nameof(dataCollection));
            foreach (var item in dataCollection) this.Add(item);
        }
        public void Add(params TData[] dataSequence)
        {
            if (dataSequence == null) throw new ArgumentNullException(nameof(dataSequence));
            foreach (var item in dataSequence) this.Add(item);
        }
        public bool Remove(TData data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            return Data.Remove(data);
        }
        public int Remove(IEnumerable<TData> dataCollection)
        {
            if (dataCollection == null) throw new ArgumentNullException(nameof(dataCollection));

            int count = 0;
            foreach (var item in dataCollection)
            {
                if (this.Remove(item)) count++;
            }
            
            return count;
        }
        public int Remove(params TData[] dataSequence)
        {
            if (dataSequence == null) throw new ArgumentNullException(nameof(dataSequence));

            int count = 0;
            foreach (var item in dataSequence)
            {
                if (this.Remove(item)) count++;
            }

            return count;
        }
        public bool Update(TData currentData, TData newData)
        {
            if (currentData == null) throw new ArgumentNullException(nameof(currentData));
            if (newData == null) throw new ArgumentNullException(nameof(newData));

            if (this.Contains(currentData))
            {
                Data[Data.IndexOf(currentData)] = newData;
                return true;
            }

            return false;
        }
        public TData this[int index] => Data[index];
        public int CompareTo(MapItem<TKey, TData> other) => Key.CompareTo(other.Key);
        public bool Equals(MapItem<TKey, TData> other) => Key.Equals(other.Key);
        public bool Contains(TData item) => Data.Contains(item);
        public override int GetHashCode() => Key.GetHashCode();
        public override string ToString() => $"### {Key} ###\n" + Data.AsIndentedString();
        public static MapItem<TKey, TData> operator +(MapItem<TKey, TData> mapItem, TData data)
        {
            mapItem.Add(data);
            return mapItem;
        }
        public static MapItem<TKey, TData> operator -(MapItem<TKey, TData> mapItem, TData data)
        {
            mapItem.Remove(data);
            return mapItem;
        }
    }
    public static class MapItem
    {
        public static MapItem<TKey, TData> Create<TKey, TData>() where TKey : IComparable<TKey>, IEquatable<TKey> where TData : IComparable<TData>, IEquatable<TData> => new MapItem<TKey, TData>();
        public static MapItem<TKey, TData> Create<TKey, TData>(TKey key, TData data) where TKey : IComparable<TKey>, IEquatable<TKey> where TData : IComparable<TData>, IEquatable<TData> => new MapItem<TKey, TData>(key, data);
        public static MapItem<TKey, TData> Create<TKey, TData>(TKey key, IEnumerable<TData> dataCollection) where TKey : IComparable<TKey>, IEquatable<TKey> where TData : IComparable<TData>, IEquatable<TData> => new MapItem<TKey, TData>(key, dataCollection);
        public static MapItem<TKey, TData> Create<TKey, TData>(TKey key, params TData[] dataSequence) where TKey : IComparable<TKey>, IEquatable<TKey> where TData : IComparable<TData>, IEquatable<TData> => new MapItem<TKey, TData>(key, dataSequence);
    }
}
