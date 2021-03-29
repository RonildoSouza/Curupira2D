using System;
using System.Collections.Generic;

namespace Curupira2D.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<Tuple<T, T>> PairUp<T>(this IEnumerable<T> source)
        {
            using var iterator = source.GetEnumerator();
            while (iterator.MoveNext())
            {
                var first = iterator.Current;
                var second = iterator.MoveNext() ? iterator.Current : default;
                yield return Tuple.Create(first, second);
            }
        }
    }
}
