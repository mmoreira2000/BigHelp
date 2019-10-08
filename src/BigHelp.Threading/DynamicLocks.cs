using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace BigHelp.Threading
{
    public class DynamicLocks
    {
        // ReSharper disable once InconsistentNaming
        private const string THREAD_NAME_TEMPLATE = "__THREAD_ID_{0}";

        private readonly ConcurrentDictionary<string, object> _locksTable;
        public DynamicLocks(IEqualityComparer<string> comparer = null)
        {
            if (comparer == null) comparer = StringComparer.Ordinal;
            _locksTable = new ConcurrentDictionary<string, object>(comparer);
        }

        public object AquireThreadLock()
        {
            return AquireLock(string.Format(THREAD_NAME_TEMPLATE, System.Threading.Thread.CurrentThread.ManagedThreadId));
        }
        public object AquireLock(string name)
        {
            return _locksTable.GetOrAdd(name, s => new object());
        }

        public TResult ExecuteWithThreadLock<TResult>(Func<TResult> function)
        {
            lock (AquireThreadLock()) return function();
        }
        public TResult ExecuteWithLock<TResult>(string name, Func<TResult> function)
        {
            lock (AquireLock(name)) return function();
        }

        public void ExecuteWithThreadLock(Action function)
        {
            lock (AquireThreadLock()) function();
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
