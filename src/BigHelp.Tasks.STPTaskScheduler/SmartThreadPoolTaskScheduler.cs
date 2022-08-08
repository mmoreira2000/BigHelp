using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Amib.Threading;

namespace BigHelp.Tasks.STPTaskScheduler
{
    public class SmartThreadPoolTaskScheduler : TaskScheduler
    {
        private readonly ConcurrentQueue<Task> _tasksQueue;
        private readonly SmartThreadPool _stp;
        private Thread _threadQueueMonitor;
        internal SmartThreadPoolTaskScheduler()
        {
            _tasksQueue = new ConcurrentQueue<Task>();
        }

        private void QueueMonitor()
        {
            while (true)
            {
                _stp.WaitForIdle();
                if (_tasksQueue.TryDequeue(out var t))
                {
                    _stp.QueueWorkItem(() => t.Start(this));
                }
            }
        }

        public SmartThreadPoolTaskScheduler(int minWorkerThreads, int maxWorkerThreads, ThreadPriority priority = ThreadPriority.Normal, string poolName = default) : this()
        {
            var startInfo = new STPStartInfo
            {
                MinWorkerThreads = minWorkerThreads,
                MaxWorkerThreads = maxWorkerThreads,
                ThreadPriority = priority
            };
            if (!string.IsNullOrEmpty(poolName)) startInfo.ThreadPoolName = poolName;

            _stp = new SmartThreadPool(startInfo);
        }
        public void Start()
        {
            _stp.Start();
            _threadQueueMonitor = new Thread(QueueMonitor);
            _threadQueueMonitor.Start();
        }

        public void Shutdown() => _stp.Shutdown();
        public void Shutdown(TimeSpan timeout) => _stp.Shutdown(timeout);

        protected override IEnumerable<Task> GetScheduledTasks() => _tasksQueue.ToArray();

        protected override void QueueTask(Task task) => _tasksQueue.Enqueue(task);

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            throw new NotImplementedException();
        }

        protected override bool TryDequeue(Task task)
        {
            return base.TryDequeue(task);

        }
    }
}
