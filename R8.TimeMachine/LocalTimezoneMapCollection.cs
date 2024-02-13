using System;
using System.Collections;
using System.Collections.Generic;

namespace R8.TimeMachine
{
    /// <summary>
    ///     A collection of <see cref="ITimezone" />.
    /// </summary>
    public class LocalTimezoneMapCollection : IEnumerable<LocalTimezoneMap>
    {
        private readonly Dictionary<string, LocalTimezoneMap> _dictionary = new Dictionary<string, LocalTimezoneMap>();

        private readonly object _syncRoot = new object();

        internal LocalTimezoneMapCollection()
        {
            if (!_dictionary.ContainsKey("UTC"))
                this.Add(new UtcTimezone());
        }

        /// <summary>Gets or sets the element with the specified key.</summary>
        /// <param name="ianaId">The key of the element to get or set.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="ianaId" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="KeyNotFoundException">The property is retrieved and <paramref name="ianaId" /> is not found.</exception>
        /// <returns>A <see cref="ITimezone" /> object.</returns>
        public LocalTimezoneMap this[string ianaId]
        {
            get
            {
                if (ianaId == null)
                    throw new ArgumentNullException(nameof(ianaId));
                lock (_syncRoot)
                {
                    return _dictionary[ianaId];
                }
            }
        }

        public IEnumerator<LocalTimezoneMap?> GetEnumerator()
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
        ///     Add the specified resolver to the collection.
        /// </summary>
        /// <param name="value">A resolver that implements <see cref="ITimezone" />.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="value" /> is <see langword="null" />.</exception>
        /// <typeparam name="T">Any type that implements <see cref="ITimezone" />.</typeparam>
        public void Add<T>(T value) where T : LocalTimezoneMap
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            lock (_syncRoot)
            {
                _dictionary[value.IanaId] = value;
            }
        }

        /// <summary>
        ///     Removes the resolver with the specified id from the <see cref="LocalTimezoneMapCollection" />
        /// </summary>
        public void Remove(string ianaId)
        {
            if (ianaId == null)
                throw new ArgumentNullException(nameof(ianaId));
            lock (_syncRoot)
            {
                _dictionary.Remove(ianaId);
            }
        }

        /// <summary>
        ///     Removes all timezone resolvers from the <see cref="LocalTimezoneMapCollection" />.
        /// </summary>
        public void Clear()
        {
            lock (_syncRoot)
            {
                _dictionary.Clear();
            }
        }

        /// <inheritdoc cref="IDictionary{TKey,TValue}.TryGetValue" />
        public bool TryGetValue(string key, out LocalTimezoneMap? value)
        {
            lock (_syncRoot)
            {
                return _dictionary.TryGetValue(key, out value);
            }
        }
    }
}