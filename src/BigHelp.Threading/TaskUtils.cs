using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BigHelp.Threading
{
    /// <summary>
    /// This class was ported from this website:
    /// https://www.ryadel.com/en/asyncutil-c-helper-class-async-method-sync-result-wait/
    /// </summary>
    public static class TaskUtils
    {
        private static readonly TaskFactory SyncTaskFactory = new TaskFactory(CancellationToken.None,
                TaskCreationOptions.None,
                TaskContinuationOptions.None, TaskScheduler.Default);

        /// <summary>
        /// Executes an async Task method which has a void return value synchronously
        /// USAGE: TaskUtils.RunSync(() => AsyncMethod());
        /// </summary>
        /// <param name="task">Task method to execute</param>
        public static void RunSync(Func<Task> task)
            => SyncTaskFactory
                .StartNew(task)
                .Unwrap()
                .GetAwaiter()
                .GetResult();

        /// <summary>
        /// Executes an async Task<T> method which has a T return type synchronously
        /// USAGE: T result = TaskUtils.RunSync(() => AsyncMethod<T>());
        /// </summary>
        /// <typeparam name="TResult">Return Type</typeparam>
        /// <param name="task">Task<T> method to execute</param>
        /// <returns></returns>
        public static TResult RunSync<TResult>(Func<Task<TResult>> task)
            => SyncTaskFactory
                .StartNew(task)
                .Unwrap()
                .GetAwaiter()
                .GetResult();
    }
}
