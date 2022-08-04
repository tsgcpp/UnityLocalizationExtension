using System;
using System.Collections.Generic;
using System.Linq;

namespace Tsgcpp.Localization.Extension.Editor
{
    public sealed class CacheListConverter<T, TResult> : IListConverter<T, TResult>
        where T : class
        where TResult : class
    {
        private readonly IListConverter<T, TResult> _actualConverter;
        private readonly List<T> _listCache = new List<T>(capacity: 32);
        private IReadOnlyList<TResult> _resultListCache = null;

        public CacheListConverter(IListConverter<T, TResult> actualConverter)
        {
            _actualConverter = actualConverter;
        }

        public IReadOnlyList<TResult> Convert(IReadOnlyList<T> list)
        {
            if (list == null)
            {
                throw new NullReferenceException("list is null");
            }

            if (_resultListCache != null && CompareArgListEquality(list))
            {
                return _resultListCache;
            }

            return CreateAndHoldCache(list);
        }

        private IReadOnlyList<TResult> CreateAndHoldCache(IReadOnlyList<T> list)
        {
            _listCache.Clear();
            _listCache.AddRange(list);
            _resultListCache = _actualConverter.Convert(list);
            return _resultListCache;
        }

        private bool CompareArgListEquality(IReadOnlyList<T> list)
        {
            if (list.Count != _listCache.Count)
            {
                return false;
            }

            return _listCache.SequenceEqual(list);
        }
    }
}
