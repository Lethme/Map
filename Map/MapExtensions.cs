using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Map
{
    public static class MapExtensions
    {
        public static string AsIndentedString<TKey, TData>(this Map<TKey, TData> source, String separator = "\n") where TKey : IComparable<TKey>, IEquatable<TKey> where TData : IComparable<TData>, IEquatable<TData>
        {
            return source.Select(item => item.ToString()).Aggregate((x, y) => $"{x}{separator}{y}");
        }
    }
}
