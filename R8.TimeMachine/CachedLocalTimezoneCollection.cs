using System.Collections;
using System.Collections.Generic;

namespace R8.TimeMachine
{
    public class CachedLocalTimezoneCollection : IEnumerable<LocalTimezone>
    {
        private readonly Dictionary<string, LocalTimezone> _dictionary = new Dictionary<string, LocalTimezone>();

        private readonly object _syncRoot = new object();

        internal CachedLocalTimezoneCollection()
        {
        }

        public IEnumerator<LocalTimezone> GetEnumerator()
        {
            lock (_syncRoot)
            {
                return _dictionary.Values.GetEnumerator();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Add a new timezone to the cache.
        /// </summary>
        public void Add(LocalTimezone timezone)
        {
            lock (_syncRoot)
            {
                _dictionary[timezone.IanaId] = timezone;
            }
        }

        /// <summary>
        ///     Clears the cache.
        /// </summary>
        public void Clear()
        {
            lock (_syncRoot)
            {
                _dictionary.Clear();
            }
        }

        /// <inheritdoc cref="IDictionary{TKey,TValue}.TryGetValue" />
        internal bool TryGetValue(string key, out LocalTimezone? value)
        {
            lock (_syncRoot)
            {
                return _dictionary.TryGetValue(key, out value);
            }
        }
    }
}