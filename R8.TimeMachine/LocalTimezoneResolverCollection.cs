using System;
using System.Collections;
using System.Collections.Generic;

namespace R8.TimeMachine
{
    /// <summary>
    ///     A collection of <see cref="ILocalTimezoneResolver" />.
    /// </summary>
    public class LocalTimezoneResolverCollection : IEnumerable<ILocalTimezoneResolver>
    {
        private readonly Dictionary<string, ILocalTimezoneResolver> _dictionary = new Dictionary<string, ILocalTimezoneResolver>();

        private readonly object _syncRoot = new object();

        internal LocalTimezoneResolverCollection()
        {
        }

        /// <summary>Gets or sets the element with the specified key.</summary>
        /// <param name="ianaId">The key of the element to get or set.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     <paramref name="ianaId" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="KeyNotFoundException">The property is retrieved and <paramref name="ianaId" /> is not found.</exception>
        /// <returns>A <see cref="ILocalTimezoneResolver" /> object.</returns>
        public ILocalTimezoneResolver this[string ianaId]
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

        public IEnumerator<ILocalTimezoneResolver> GetEnumerator()
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
        /// <typeparam name="T">A class that implements <see cref="ILocalTimezoneResolver" />.</typeparam>
        public void Add<T>(T resolver) where T : class, ILocalTimezoneResolver
        {
            lock (_syncRoot)
            {
                _dictionary[resolver.IanaId] = resolver;
            }
        }

        /// <summary>
        ///     Removes the resolver with the specified id from the <see cref="LocalTimezoneResolverCollection" />
        /// </summary>
        public void Remove(string ianaId)
        {
            lock (_syncRoot)
            {
                _dictionary.Remove(ianaId);
            }
        }

        /// <summary>
        ///     Removes all timezone resolvers from the <see cref="LocalTimezoneResolverCollection" />.
        /// </summary>
        public void Clear()
        {
            lock (_syncRoot)
            {
                _dictionary.Clear();
            }
        }

        /// <inheritdoc cref="IDictionary{TKey,TValue}.TryGetValue" />
        public bool TryGetValue(string key, out ILocalTimezoneResolver value)
        {
            lock (_syncRoot)
            {
                return _dictionary.TryGetValue(key, out value);
            }
        }
    }
}