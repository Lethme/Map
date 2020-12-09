using System;
using System.Collections.Generic;
using System.Text;

namespace Map.Extensions
{
    public struct SearchingResult<TKey, TData>
        where TKey : IComparable<TKey>, IEquatable<TKey>
        where TData : IComparable<TData>, IEquatable<TData>
    {
        public MapItem<TKey, TData> MapItem { get; }
        public int ComparisonCount { get; }
        public SearchingResult(MapItem<TKey, TData> mapItem, int comparisonCount)
        {
            MapItem = mapItem;
            ComparisonCount = comparisonCount;
        }
        public override bool Equals(object obj)
        {
            return obj is SearchingResult<TKey, TData> other &&
                   EqualityComparer<MapItem<TKey, TData>>.Default.Equals(MapItem, other.MapItem) &&
                   ComparisonCount == other.ComparisonCount;
        }
        public override int GetHashCode() => HashCode.Combine(MapItem, ComparisonCount);
        public void Deconstruct(out MapItem<TKey, TData> mapItem, out int comparisonCount)
        {
            mapItem = MapItem;
            comparisonCount = ComparisonCount;
        }
        public override string ToString() => $"{(MapItem != null ? MapItem.ToString() : "No data found.")}\nComparison count: {ComparisonCount}";
        public static implicit operator (MapItem<TKey, TData> MapItem, int ComparisonCount)(SearchingResult<TKey, TData> value)
        {
            return (value.MapItem, value.ComparisonCount);
        }
        public static implicit operator SearchingResult<TKey, TData>((MapItem<TKey, TData> MapItem, int ComparisonCount) value)
        {
            return new SearchingResult<TKey, TData>(value.MapItem, value.ComparisonCount);
        }
        public static bool operator ==(SearchingResult<TKey, TData> left, SearchingResult<TKey, TData> right)
        {
            return left.Equals(right);
        }
        public static bool operator !=(SearchingResult<TKey, TData> left, SearchingResult<TKey, TData> right)
        {
            return !(left == right);
        }
    }
}
