using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BigHelp.Threading
{
    /// <summary>
    /// Provides named objects that can be used to do lock operations based on names.
    /// </summary>
    public class NamedLocks
    {
        private readonly ConcurrentDictionary<string, object> _locksTable;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="keyComparer">The named key comparer. Defaults to Case Sensitive ordinal comparison</param>
        public NamedLocks(IEqualityComparer<string> keyComparer = null)
        {
            if (keyComparer == null) keyComparer = StringComparer.Ordinal;
            _locksTable = new ConcurrentDictionary<string, object>(keyComparer);
        }

        public object AquireLock(string name)
        {
            //https://stackoverflow.com/questions/10486579/concurrentdictionary-pitfall-are-delegates-factories-from-getoradd-and-addorup
            return _locksTable.GetOrAdd(name, new object());
        }
        public TResult ExecuteWithLock<TResult>(string name, Func<TResult> function)
        {
            lock (AquireLock(name)) return function();
        }
        public void ExecuteWithLock(string name, Action function)
        {
            lock (AquireLock(name)) function();
        }

        public bool DeleteLock(string name)
        {
            return _locksTable.TryRemove(name, out _);
        }
    }
}
