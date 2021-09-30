using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpertWinnerLang.Extensions
{
    public static class EnumerableExtensions
    {
        public static Queue<T> ToQueue<T>(this IEnumerable<T> source)
        {
            var resultQueue = new Queue<T>();
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
                resultQueue.Enqueue(enumerator.Current);
            return resultQueue;
        }
        
        public static Queue<TProjection> ToQueue<TSource, TProjection>(this IEnumerable<TSource> source, Func<TSource, TProjection> selector)
        {
            var resultQueue = new Queue<TProjection>();
            using var enumerator = source.GetEnumerator();
            while (enumerator.MoveNext())
                resultQueue.Enqueue(selector(enumerator.Current));
            return resultQueue;
        }

        public static IEnumerable<T> DistinctBy<T, TParam>(this IEnumerable<T> source, Func<T,TParam> paramSelector)
        {
            return source
                .Select(x => new
                {
                    Item = x,
                    Param = paramSelector(x)
                })
                .GroupBy(x => x.Param)
                .Select(x => x.First().Item);
        }
    }
}