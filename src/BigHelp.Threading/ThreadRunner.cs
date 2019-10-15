using System;
using System.Threading;

namespace BigHelp.Threading
{
#if NETSTANDARD2_0
    /// <summary>
    /// This class allows running code on a separate thread in a safe way.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ThreadRunner<T>
    {
        public string ThreadName { get; }

        private readonly int? _timeout;
        private Thread _thread;

        public ThreadRunner(int? timeout) : this(Thread.CurrentThread.Name, timeout) { }
        public ThreadRunner(string threadName, int? timeout)
            : this(threadName)
        {
            _timeout = timeout;
        }

        public ThreadRunner(string threadName)
        {
            ThreadName = threadName;
            State = ThreadRunnerState.NotStarted;
        }

        public ThreadRunnerState State { get; private set; }
        public void Run(Func<T> command)
        {
            State = ThreadRunnerState.Running;
            _thread = new Thread(() =>
            {
                try
                {
                    Result = command();
                    State = ThreadRunnerState.Finished;
                }
                catch (ThreadAbortException ta)
                {
                    Error = ta.InnerException ?? ta;
                    State = ThreadRunnerState.Timeout;
                }
                catch (Exception ex)
                {
                    Error = ex;
                    State = ThreadRunnerState.Error;
                }
            })
            {
                Name = ThreadName
            };
            _thread.Start();
        }

        public Exception Error { get; private set; }

        public T Result { get; private set; }

        public void WaitFinish(int timeOut)
        {
            if (_thread == null) return;
            var terminou = _thread.Join(timeOut);
            if (!terminou)
                State = ThreadRunnerState.Timeout;
        }

        public void WaitFinish(TimeSpan timeOut)
        {
            if (_thread == null) return;
            var terminou = _thread.Join(timeOut);
            if (!terminou)
                State = ThreadRunnerState.Timeout;
        }

        public void WaitFinish()
        {
            if (_thread == null) return;
            if (_timeout == null)
            {
                _thread.Join();
                return;
            }

            var terminou = _thread.Join(_timeout.Value);
            if (!terminou)
                State = ThreadRunnerState.Timeout;
        }
    }

    public enum ThreadRunnerState
    {
        NotStarted,
        Running,
        Finished,
        Error,
        Timeout
    }

#endif
}