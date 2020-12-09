using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Map.Extensions;

namespace Map
{
    public static class MapExtensions
    {
        public static string AsIndentedString<TKey, TData>(this Map<TKey, TData> source, String separator = "\n") 
            where TKey : IComparable<TKey>, IEquatable<TKey> 
            where TData : IComparable<TData>, IEquatable<TData>
        {
            return source.Select(item => item.ToString()).Aggregate((x, y) => $"{x}{separator}{y}");
        }
        public static SearchingResult<TKey, TData> Find<TKey, TData>(this Map<TKey, TData> source, TKey key)
            where TKey : IComparable<TKey>, IEquatable<TKey>
            where TData : IComparable<TData>, IEquatable<TData>
        {
            var count = 0;
            foreach (var item in source)
            {
                count++;
                if ((item as MapItem<TKey, TData>).Key.Equals(key)) return ((item as MapItem<TKey, TData>), count);
            }

            return (null, count);
        }
    }
}
