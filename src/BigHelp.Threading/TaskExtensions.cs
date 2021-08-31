using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BigHelp.Threading
{
    /// <summary>
    /// This class was based on the article found on this website:
    /// https://www.ryadel.com/en/asyncutil-c-helper-class-async-method-sync-result-wait/
    /// </summary>
    public static class TaskExtensions
    {
        private static readonly TaskFactory SyncTaskFactory = new TaskFactory(CancellationToken.None,
                TaskCreationOptions.None,
                TaskContinuationOptions.None, TaskScheduler.Default);

        /// <summary>
        /// Executes an async Task method which has a void return value in a safely synchronously way
        /// </summary>
        /// <example>task.RunSync(() => AsyncMethod());</example>
        /// <param name="task">Task method to execute</param>
        public static void RunSync(this Func<Task> task)
            => SyncTaskFactory
                .StartNew(task)
                .Unwrap()
                .GetAwaiter()
                .GetResult();

        /// <summary>
        /// Executes an async Task<T> method which has a T return type in a safely synchronously way
        /// </summary>
        /// <example>T result = task.RunSync(() => AsyncMethod<T>());</example>
        /// <typeparam name="T">Return Type</typeparam>
        /// <param name="task">Task method of type TResult to execute</param>
        /// <returns></returns>
        public static T RunSync<T>(this Func<Task<T>> task)
            => SyncTaskFactory
                .StartNew(task)
                .Unwrap()
                .GetAwaiter()
                .GetResult();


        /// <summary>
        /// This method is similar to the Result proprerty of a Task, but it returns the original exception instead of a Aggregate exception
        /// if only on exception is throw by the task.
        /// </summary>
        /// <example>T result = task.Result();</example>
        /// <returns>T</returns>
        public static T Result<T>(this Task<T> task)
        {
            try
            {
                return task.Result;
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count <= 1) throw ex.InnerExceptions[0];
                throw;
            }
        }

        /// <summary>
        /// This method is similar to the Result proprerty of a Task, but it returns the original exception instead of a Aggregate exception
        /// if only on exception is throw by the task.
        /// </summary>
        /// <example>task.Result();</example>
        /// <returns>Task</returns>
        public static Task Result(this Task task)
        {
            try
            {
                task.GetAwaiter().GetResult();
                return task;
            }
            catch (AggregateException ex)
            {
                if (ex.InnerExceptions.Count <= 1) throw ex.InnerExceptions[0];
                throw;
            }
        }
    }
}
