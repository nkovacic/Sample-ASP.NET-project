using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> WithoutLast<T>(this IEnumerable<T> xs)
        {
            T lastX = default(T);
            bool first = true;

            foreach (T x in xs)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    yield return lastX;
                }

                lastX = x;
            }
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var knownKeys = new HashSet<TKey>();

            foreach (TSource element in source)
            {
                if (knownKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }
    }
}
